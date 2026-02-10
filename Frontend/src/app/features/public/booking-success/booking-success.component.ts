import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { BookingService } from '../../../core/services/booking.service';

/**
 * Component responsible for displaying the confirmation details after a successful booking.
 * It retrieves the booking ID from the URL query parameters and fetches the full details
 * to show a receipt/summary to the user.
 */
@Component({
  selector: 'app-booking-success',
  standalone: true,
  imports: [CommonModule, RouterModule, DatePipe],
  templateUrl: './booking-success.component.html',
  styleUrl: './booking-success.component.scss'
})
export class BookingSuccessComponent implements OnInit {

  booking: any = null;
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private bookingService: BookingService
  ) { }

  /**
   * Lifecycle hook called after component initialization.
   * extract the 'bookingId' from the query string (e.g., /booking-success?bookingId=123).
   */
  ngOnInit(): void {
    // snapshot.queryParamMap gets the params at the moment the route was activated
    const bookingId = Number(this.route.snapshot.queryParamMap.get('bookingId'));

    if (bookingId) {
      this.loadBooking(bookingId);
    }
  }

  /**
   * Fetches the booking details from the backend.
   * params id - The ID of the confirmed booking.
   */
  loadBooking(id: number) {
    this.isLoading = true;
    this.bookingService.getBookingById(id).subscribe({
      next: (data) => {
        this.booking = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
        // Ideally, handle error UI here (e.g., "Booking not found")
      }
    });
  }

  /**
   * Triggers the browser's native print dialog.
   * Allows the user to print the booking confirmation as a PDF or to a physical printer.
   */
  printPage() {
    window.print();
  }
}
