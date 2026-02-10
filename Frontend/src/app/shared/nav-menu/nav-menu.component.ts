import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './nav-menu.component.html',
  styleUrl: './nav-menu.component.scss'
})
export class NavMenuComponent {
  currentUser$: Observable<any | null>;

  constructor(private authService: AuthService ,private router: Router) {
    this.currentUser$ = this.authService.currentUser$;
  }

  logout() {
    this.authService.logout();
  }

  goToLogin() {
    // Get the current URL that the user is on
    const currentUrl = this.router.url;

    // Navigate to /login and pass the current URL
    this.router.navigate(['/login'], {
      queryParams: { returnUrl: currentUrl }
    });
  }
}
