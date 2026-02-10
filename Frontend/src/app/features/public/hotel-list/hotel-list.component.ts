import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HotelService } from '../../../core/services/hotel.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-hotel-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './hotel-list.component.html',
  styleUrl: './hotel-list.component.scss'
})

export class HotelListComponent implements OnInit {

  hotels: any[] = []; // This is now our final, filtered list
  isLoading = true;

  // 3. This object holds the state of ALL our filters
  filters = {
    destination: '',
    checkIn: '',
    checkOut: '',
    maxPrice: 1000,
    minRating: 0, // 0 = All
    sortBy: 'Recommended' // Default sort
  }; // 0 means all ratings

  constructor(
    private hotelService: HotelService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    // 4. When the page loads, get parameters from the URL
    this.route.queryParams.subscribe(params => {
      this.filters.destination = params['destination'] || '';
      this.filters.checkIn = params['checkIn'] || '';
      this.filters.checkOut = params['checkOut'] || '';

      // Now, load the hotels using these filters
      this.loadHotels();
    });
  }

  loadHotels() {
    this.isLoading = true;

    // FIX 1: Call 'searchHotels' instead of 'getHotels'
    this.hotelService.searchHotels(this.filters).subscribe({

      // FIX 2: Add explicit types (data: any, err: any)
      next: (data: any) => {
        this.hotels = data;
        this.isLoading = false;
      },
      error: (err: any) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

}

