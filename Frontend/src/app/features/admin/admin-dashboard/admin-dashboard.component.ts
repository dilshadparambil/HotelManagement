import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service'; // Adjust path if needed

/**
 * Component representing the main dashboard for Admin users.
 * Acts as the shell/layout for the admin interface, providing navigation
 * via the router and access to global actions like Logout.
 */
@Component({
  selector: 'app-admin-dashboard',
  // Imports RouterModule to ensure 'routerLink' and '<router-outlet>' work in the HTML template
  imports: [RouterModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss'
})
export class AdminDashboardComponent {

  // Inject the AuthService to handle session management
  constructor(private authService: AuthService) {}

  /**
   * Triggers the logout process.
   * Delegates the actual logic (token removal, redirection) to the AuthService.
   */
  logout() {
    this.authService.logout();
  }
}
