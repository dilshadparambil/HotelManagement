import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  Observable,
  BehaviorSubject,
  tap,
  catchError,
  of,
  switchMap,
  finalize,
  map,
  throwError,
} from 'rxjs';
import { Router } from '@angular/router';

/**
 * Service responsible for handling all authentication logic.
 * It manages user state, login/logout for both Customers and Admins,
 * and automatic token refreshing.
 */
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // API Endpoints
  private customerAuthUrl = 'https://localhost:7231/api/customer-auth';
  private adminAuthUrl = 'https://localhost:7231/api/admin-auth';

  // State Management: Holds the current user object
  private currentUserSubject = new BehaviorSubject<any | null>(null);
  // Public observable for components to subscribe to user changes
  public currentUser$ = this.currentUserSubject.asObservable();

  // State Management: Tracks if the initial auth check (on app load) is finished.
  // Guards use this to wait before redirecting users.
  private isAuthCheckComplete = new BehaviorSubject<boolean>(false);
  public isAuthCheckComplete$ = this.isAuthCheckComplete.asObservable();

  // Mutex to prevent multiple refresh calls at once
  private isRefreshing = false;

  constructor(private http: HttpClient, private router: Router) {
    // On service initialization (App Start), check if a valid session exists.
    this.probeCurrentUserOnStart().subscribe({
      next: () => {
        /* User successfully loaded from session */
      },
      error: () => {
        /* No valid session found - User is guest/anonymous */
      },
    });
  }

  /**
   * Authenticates a customer.
   * param loginData - Credentials for customer login.
   */
  customerLogin(loginData: any): Observable<any> {
    return this.http.post<any>(`${this.customerAuthUrl}/login`, loginData);
  }

  /**
   * Authenticates an admin.
   * param loginData - Credentials for admin login.
   */
  adminLogin(loginData: any): Observable<any> {
    return this.http.post<any>(`${this.adminAuthUrl}/login`, loginData);
  }

  /**
   * Registers a new customer.
   * Uses responseType: 'text' because the backend likely returns a plain text success message.
   */
  customerSignup(signupData: any): Observable<any> {
    return this.http.post(`${this.customerAuthUrl}/signup`, signupData, {
      responseType: 'text',
    });
  }

  /**
   * Attempts to refresh the auth token.
   * Strategy:
   * 1. Try refreshing as an Admin.
   * 2. If that fails, catch the error and try refreshing as a Customer.
   * 3. If both fail, logout the user.
   */
  refreshToken(): Observable<any> {
    // Prevent duplicate refresh requests
    if (this.isRefreshing) {
      return of(null);
    }
    this.isRefreshing = true;

    // Attempt Admin Refresh
    return this.http.post<any>(`${this.adminAuthUrl}/refresh`, {}).pipe(
      // If successful, re-fetch user details
      switchMap(() => this.probeCurrentUserOnStart()),

      // If Admin refresh fails, attempt Customer Refresh
      catchError(() => {
        return this.http
          .post<any>(`${this.customerAuthUrl}/refresh`, {})
          .pipe(switchMap(() => this.probeCurrentUserOnStart()));
      }),

      // If BOTH fail, the session is dead.
      catchError((err) => {
        this.isRefreshing = false;
        this.logout();
        return throwError(() => err);
      }),

      // Always unlock the mutex when done
      finalize(() => {
        this.isRefreshing = false;
      })
    );
  }

  /**
   * Manually loads the user profile after a successful login action.
   * This is called explicitly by the Login Component.
   * param role - The expected role of the user ('Admin' or 'Customer').
   */
  loadUserAfterLogin(role: 'Admin' | 'Customer'): Observable<any> {
    // Signal that auth check is starting
    this.isAuthCheckComplete.next(false);

    let endpoint = '';
    let expectedRole = '';

    // Determine which endpoint to hit based on the role provided
    if (role === 'Admin') {
      endpoint = `${this.adminAuthUrl}/me`;
      expectedRole = 'Admin';
    } else {
      endpoint = `${this.customerAuthUrl}/me`;
      expectedRole = 'Customer';
    }

    return this.http.get<any>(endpoint).pipe(
      map((user) => {
        // Validate that the returned user matches the expected role
        if (user && user.role === expectedRole) {
          return user;
        }
        throw new Error(`User role is not ${expectedRole}`);
      }),
      tap((user) => {
        // Update the state
        this.currentUserSubject.next(user);
      }),
      catchError((err) => {
        // On error, clear the user state
        this.currentUserSubject.next(null);
        return throwError(() => err);
      }),
      finalize(() => {
        // Signal that auth check is finished
        this.isAuthCheckComplete.next(true);
      })
    );
  }

  /**
   * Determines the current user on Application Start.
   * Since we don't know if the user is an Admin or Customer from the cookie alone,
   * we try probing the Admin endpoint first, then fall back to the Customer endpoint.
   */
  probeCurrentUserOnStart(): Observable<any> {
    this.isAuthCheckComplete.next(false);

    // 1. Try getting Admin details
    return this.http.get<any>(`${this.adminAuthUrl}/me`).pipe(
      map((user) => {
        if (user && user.role === 'Admin') {
          return user;
        }
        throw new Error('User is not an admin.');
      }),
      tap((user) => this.currentUserSubject.next(user)),

      // 2. If Admin probe fails, catch error and try Customer details
      catchError(() => {
        return this.http.get<any>(`${this.customerAuthUrl}/me`).pipe(
          map((user) => {
            if (user && user.role === 'Customer') {
              return user;
            }
            throw new Error('User is not a customer.');
          }),
          tap((user) => this.currentUserSubject.next(user)),

          // 3. If both fail, no user is logged in
          catchError(() => {
            this.currentUserSubject.next(null);
            return throwError(() => new Error('No authenticated user found.'));
          })
        );
      }),
      finalize(() => {
        // Ensure guards know the check is done, regardless of success/failure
        this.isAuthCheckComplete.next(true);
      })
    );
  }

  /**
   * Logs out the current user.
   * Handles API calls to clear cookies and redirects based on the user's role.
   */
  logout() {
    const role = this.currentUserValue?.role;
    let redirectUrl: string;

    if (role === 'Admin') {
      // Admin Logout Logic
      redirectUrl = '/admin/login';
      this.http.post(`${this.adminAuthUrl}/logout`, {}).subscribe();
    } else if (role === 'Customer') {
      // Customer Logout Logic
      redirectUrl = this.router.url;

      // Prevent redirecting to protected pages after logout
      if (
        redirectUrl.startsWith('/checkout') ||
        redirectUrl.startsWith('/booking-success')
      ) {
        redirectUrl = '/';
      }

      this.http.post(`${this.customerAuthUrl}/logout`, {}).subscribe();
    } else {
      // Default fallback
      redirectUrl = '/login';
    }

    // Clear local state immediately
    this.currentUserSubject.next(null);

    // Perform the navigation
    this.router.navigateByUrl(redirectUrl);
  }

  // Helper getter for the current value of the subject
  public get currentUserValue(): any | null {
    return this.currentUserSubject.value;
  }
}
