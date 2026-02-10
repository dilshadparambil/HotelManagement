import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { timer, switchMap } from 'rxjs'; // <-- IMPORT TIMER & SWITCHMAP

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  credentials = {
    username: '',
    password: ''
  };
  errorMessage = '';
  private returnUrl: string = '/';

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  onLogin() {
    this.errorMessage = '';
    this.authService.customerLogin(this.credentials).subscribe({
      next: () => {
        // --- THIS IS THE FIX ---
        // 1. Wait 50ms for the cookie to be set
        timer(50).pipe(
          // 2. Call the NEW function to load ONLY a customer
          switchMap(() => this.authService.loadUserAfterLogin('Customer'))
        ).subscribe({
          next: (user) => {
            // The service already validated the role is 'Customer'
            if (user) {
              this.router.navigateByUrl(this.returnUrl); // SUCCESS
            } else {
              this.errorMessage = 'Login succeeded but failed to retrieve customer details.';
            }
          },
          error: (err) => {
            console.error(err);
            // This error now means the /customer-auth/me endpoint failed
            this.errorMessage = 'Login failed. This is not a valid customer account.';
          }
        });
        // --- END FIX ---
      },
      error: (err) => {
        console.error(err);
        this.errorMessage = 'Login failed. Please check your username and password.';
      }
    });
  }
}
