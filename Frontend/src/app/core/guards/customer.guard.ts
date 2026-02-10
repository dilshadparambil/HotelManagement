import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';
import { AuthService } from '../services/auth.service';
import { filter, map, take, Observable } from 'rxjs';

/**
 * Functional Route Guard to protect customer-facing routes.
 * Unlike synchronous guards, this guard returns an `Observable<boolean>`.
 * It handles cases where the app might still be restoring the session (e.g., page refresh)
 * by waiting for the `isAuthCheckComplete$` signal before checking the user state.
 * param route - The activated route snapshot.
 * param state - The router state snapshot (used to capture the return URL).
 * returns {Observable<boolean>} - Emits true if allowed, false if redirected.
 */
export const customerGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
): Observable<boolean> => {

  // Inject dependencies within the functional guard context
  const authService = inject(AuthService);
  const router = inject(Router);

  // Work with the reactive stream to ensure auth state is ready
  return authService.isAuthCheckComplete$.pipe(

    // Filter serves as a "gate": It blocks the stream until `isComplete` becomes true.
    // This prevents the guard from running the check while the auth service
    // is still verifying the token/session from local storage.
    filter((isComplete) => isComplete),

    // Take(1) ensures that once we get a 'true' value, we complete the Observable.
    // Route guards usually expect the Observable to complete; otherwise, navigation hangs.
    take(1),

    // Map transforms the stream result into the final boolean decision
    map(() => {
      const currentUser = authService.currentUserValue;

      if (currentUser) {
        // User is authenticated -> Allow navigation
        return true;
      } else {
        // User is NOT authenticated.
        // Redirect to the login page.
        router.navigate(['/login'], {
          // Pass the URL the user was trying to access as a query param (`returnUrl`).
          // This allows the login page to redirect them back here after successful login.
          queryParams: { returnUrl: state.url },
        });

        // Deny navigation to the requested route
        return false;
      }
    })
  );
};
