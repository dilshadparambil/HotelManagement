import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

/**
 * Component responsible for registering new customers.
 * Provides a form to capture user details and handles the API submission via AuthService.
 */
@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule], // Add RouterModule for routerLink
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.scss'
})
export class SignupComponent {

  // This object will hold all our form data
  // Bound to the template input fields using [(ngModel)]
  signupData = {
    fullName: '',
    email: '',
    phoneNumber: '',
    idProofNumber: '',
    username: '',
    password: ''
  };

  // UI state for displaying API error messages
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  /**
   * Submits the registration form.
   * Handles successful signup redirection and parses backend errors if they occur.
   */
  onSignup() {
    this.errorMessage = ''; // Clear previous errors before new attempt

    this.authService.customerSignup(this.signupData).subscribe({
      next: (response) => {
        // Registration successful
        alert('Signup successful! Please log in.');
        this.router.navigate(['/login']); // Redirect to the login page
      },
      error: (err) => {
        console.error(err);

        // Error Handling Logic:
        // Try to get the specific message from the backend exception.
        // Different backends or middleware might structure the error object differently.
        if (err.error && err.error.error) {
           // This catches our new Backend JSON format: { "error": "Username exists..." }
           this.errorMessage = err.error.error;
        } else if (err.error && err.error.message) {
           // Standard fallback for generic API errors
           this.errorMessage = err.error.message;
        } else {
           // Default fallback if the error format is unknown
           this.errorMessage = 'Signup failed. Please try again.';
        }
      }
    });
  }
}
