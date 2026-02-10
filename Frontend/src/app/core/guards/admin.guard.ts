import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Functional Route Guard to protect admin-specific routes.
 *  This guard checks if the current user is logged in and holds the 'Admin' role.
 * If the check fails, the user is redirected to the admin login page.
 *  param route - The activated route snapshot.
 * param state - The router state snapshot.
 * returns {boolean} - True if access is granted, false otherwise.
 */
export const adminGuard: CanActivateFn = (route, state) => {

  // dependency injection using the modern Angular `inject` function
  // used within a functional guard context.
  const authService = inject(AuthService);
  const router = inject(Router);

  // Retrieve the current state of the user from the Authentication Service
  const user = authService.currentUserValue;

  // Check if a user object exists AND if their role matches 'Admin'
  if (user && user.role === 'Admin') {
    // Access granted to the requested route
    return true; // SUCCESS
  }

  // Access denied: Redirect the user to the admin login page
  // so they can authenticate properly before accessing the route.
  router.navigate(['/admin/login']);

  // Return false to cancel the current navigation attempt
  return false;
};
