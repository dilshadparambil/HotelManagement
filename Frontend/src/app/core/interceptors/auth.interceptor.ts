import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { throwError, catchError, switchMap } from 'rxjs';

/**
 * A flag to track if a refresh token operation is currently in progress.
 * This acts as a mutex to prevent multiple concurrent refresh calls
 * if the app triggers multiple 401 errors simultaneously.
 */
let isRefreshing = false;

/**
 * HTTP Interceptor to handle Authentication.
 *
 * Responsibilities:
 * 1. Attaches `withCredentials` to outgoing requests (for cookie-based auth).
 * 2. Catches 401 (Unauthorized) errors.
 * 3. Attempts to refresh the session automatically.
 * 4. Retries the original request upon successful refresh.
 *
 * param req - The outgoing HTTP request.
 * param next - The next interceptor in the chain.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);

  // Clone the request to ensure cookies/credentials are sent with every request.
  // This is crucial for backends relying on HttpOnly cookies.
  const reqWithCreds = req.clone({
    withCredentials: true,
  });

  // Pass the request to the next handler and listen for the response
  return next(reqWithCreds).pipe(
    catchError((error: HttpErrorResponse) => {
      // Check if the error is a 401 (Unauthorized)
      // AND we are not already trying to refresh the token
      // AND the error didn't come from the login or refresh endpoints themselves
      // (to avoid infinite loops).
      if (
        error.status === 401 &&
        !isRefreshing &&
        !req.url.includes('/login') &&
        !req.url.includes('/refresh')
      ) {
        // Lock the process to prevent other requests from triggering a refresh
        isRefreshing = true;

        // Attempt to refresh the token
        return authService.refreshToken().pipe(
          switchMap(() => {
            // REFRESH SUCCESS:
            // Unlock the process
            isRefreshing = false;

            // Retry the original request (reqWithCreds) now that the session is refreshed.
            // Note: Since we use cookies (withCredentials), the browser automatically
            // attaches the new session cookie to this retried request.
            return next(reqWithCreds);
          }),
          catchError((refreshError) => {
            // REFRESH FAILED:
            // The session is likely completely expired or invalid.
            isRefreshing = false;

            // Log the user out to clear invalid state and redirect to login
            authService.logout();

            // Propagate the error to the subscriber
            return throwError(() => refreshError);
          })
        );
      }

      // If it's not a 401, or if it's a 401 from a blocked URL,
      // simply re-throw the error to be handled by the component.
      return throwError(() => error);
    })
  );
};
