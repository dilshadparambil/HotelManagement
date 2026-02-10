import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

/**
 * Landing page component for the application.
 * Provides the main search interface for finding hotels by destination and date,
 * as well as quick links to popular cities.
 */
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  // Model object bound to the search form inputs
  searchCriteria = {
    destination: '',
    checkIn: '',
    checkOut: ''
  };

  constructor(private router: Router) {}

  /**
   * Executed when the main search form is submitted.
   * Validates input and redirects the user to the search results page.
   */
  onSearch() {
    // Prevent searching without a destination
    if (!this.searchCriteria.destination.trim()) {
      alert('Please enter a destination!');
      return;
    }

    // Navigate to the '/search' route, passing the user's input as URL query parameters.
    // This allows the search page to read these values (e.g., ?destination=Paris&checkIn=...)
    // and fetch the appropriate data on load.
    this.router.navigate(['/search'], {
      queryParams: {
        destination: this.searchCriteria.destination,
        checkIn: this.searchCriteria.checkIn,
        checkOut: this.searchCriteria.checkOut
      }
    });
  }

  /**
   * Triggered when a user clicks on a "Popular Destination" card.
   * Immediately navigates to the search page filtered by that specific city.
   * params city - The name of the city to search for.
   */
  searchByCity(city: string) {
    this.router.navigate(['/search'], {
      queryParams: { destination: city }
    });
  }
}
