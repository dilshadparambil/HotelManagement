import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { timer, switchMap } from 'rxjs';

/**
 * Handles Admin authentication.
 * Includes specific logic to handle cookie propagation latency.
 */
@Component({
  selector: 'app-admin-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-login.component.html',
  styleUrl: './admin-login.component.scss'
})
export class AdminLoginComponent {
  credentials = {
    username: '',
    password: ''
  };

  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) { }

  /**
   * Authenticates the admin user.
   * Uses a timer delay to ensure the HTTP-only cookie is set by the browser
   * before requesting user details.
   */
  onLogin() {
    this.errorMessage = '';
    this.authService.adminLogin(this.credentials).subscribe({
      next: (res: any) => {
        // Race Condition Handling:
        // The login response includes a Set-Cookie header. Browsers process this asynchronously.
        // If we immediately fire the next request (`loadUserAfterLogin`), it might send BEFORE the cookie is saved,
        // causing a 401 error.
        // `timer(50)` pauses execution for 50ms to give the browser time to persist the cookie.
        timer(50).pipe(
          // `switchMap` subscribes to the inner Observable (loadUserAfterLogin) only after the timer completes.
          // This ensures sequential execution: Login -> Wait -> Fetch User Details.
          switchMap(() => this.authService.loadUserAfterLogin('Admin'))
        ).subscribe({
          next: (user) => {
            // Double check that the user object exists before navigating.
            if (user) {
              this.router.navigate(['/admin/dashboard']);
            } else {
              this.errorMessage = 'Login succeeded but failed to retrieve admin details.';
            }
          },
          error: (err) => {
            console.error(err);
            // This block catches errors specifically from the `loadUserAfterLogin` call,
            // such as if the user logged in successfully but didn't have the 'Admin' role.
            this.errorMessage = 'Login failed. This is not a valid admin account.';
          }
        });
      },
      error: (err: any) => {
        // This block catches errors from the initial `adminLogin` call (e.g., wrong password).
        this.errorMessage = 'Invalid username or password';
        console.error(err);
      }
    });
  }
}
