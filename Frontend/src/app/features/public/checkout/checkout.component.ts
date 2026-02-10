import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { RoomService } from '../../../core/services/room.service';
import { BookingService } from '../../../core/services/booking.service';
import { AuthService } from '../../../core/services/auth.service';
import { CustomerService } from '../../../core/services/customers.service';

/**
 * Component responsible for finalizing the room booking process.
 * It validates the user session, calculates costs, and submits the booking.
 */
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit {

  room: any = null;
  isLoading = true;
  isProcessing = false;
  loggedInCustomer: any = null;
  today!: string;

  bookingDates = {
    checkInDate: '',
    checkOutDate: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private roomService: RoomService,
    private bookingService: BookingService,
    private authService: AuthService,
    private customerService: CustomerService
  ) { }

  /**
   * Initializes the component.
   * Sets the minimum selectable date to today and verifies the customer identity.
   */
  ngOnInit(): void {
    const now = new Date();

    // We need to format the current date into 'YYYY-MM-DD' string format
    // because the HTML5 <input type="date"> 'min' attribute requires this exact format.
    // The .slice(-2) method handles adding a leading zero (e.g., '9' becomes '09').
    const year = now.getFullYear();
    const month = ('0' + (now.getMonth() + 1)).slice(-2);
    const day = ('0' + now.getDate()).slice(-2);

    this.today = `${year}-${month}-${day}`;

    const roomId = Number(this.route.snapshot.queryParamMap.get('roomId'));

    // Retrieve the current user state from the BehaviorSubject in AuthService.
    // This avoids an asynchronous HTTP call if we already have the user in memory.
    const currentUser = this.authService.currentUserValue;

    // Security Check: Ensure the user is logged in, has a valid ID, and is specifically a 'Customer'.
    // We perform this check before loading the room to prevent unauthorized access to the checkout flow.
    if (currentUser && currentUser.role === 'Customer' && currentUser.id) {

      // Fetch the full customer profile from the database using the ID from the auth state.
      // We do this to ensure we have the most up-to-date customer data (like ID) for the booking record.
      this.customerService.getCustomerById(currentUser.id).subscribe({
        next: (customerData) => {
            this.loggedInCustomer = customerData;

            // Only load the room details once the customer is successfully verified.
            if (roomId) {
              this.loadRoom(roomId);
            }
        },
        error: (err) => {
          console.error("Failed to load customer details", err);
          // If the customer details cannot be fetched, the session is likely stale or invalid.
          // Force a logout to clean up the state.
          this.authService.logout();
        }
      });
    } else {
      // Fallback: If the user state is missing or the role is incorrect (e.g., an Admin trying to checkout),
      // log them out and redirect to login.
      console.error("Checkout page loaded without a valid customer. Logging out.");
      this.isLoading = false;
      this.authService.logout();
    }
  }

  /**
   * Retrieves room details by ID.
   * params id - The ID of the room.
   */
  loadRoom(id: number) {
    this.roomService.getRoomById(id).subscribe({
      next: (data) => {
        this.room = data;
        this.isLoading = false;
      },
      error: () => {
        alert('Room not found');
        this.router.navigate(['/']);
      }
    });
  }

  /**
   * Calculates the total cost based on the number of nights.
   * returns The calculated total price.
   */
  calculateTotal(): number {
    // Return 0 immediately if any required data (room price, start date, end date) is missing.
    if (!this.room || !this.bookingDates.checkInDate || !this.bookingDates.checkOutDate) return 0;

    const start = new Date(this.bookingDates.checkInDate);
    const end = new Date(this.bookingDates.checkOutDate);

    // Get the absolute difference in milliseconds between the two dates.
    const diffTime = Math.abs(end.getTime() - start.getTime());

    // Convert milliseconds to days:
    // (1000 ms/s * 60 s/m * 60 m/h * 24 h/day)
    const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

    return diffDays * this.room.pricePerNight;
  }

  /**
   * Submits the booking to the API.
   * Redirects to success page upon completion.
   */
  confirmBooking() {
    this.isProcessing = true;

    const bookingData = {
      customerId: this.loggedInCustomer.id,
      roomId: this.room.id,
      checkInDate: this.bookingDates.checkInDate,
      checkOutDate: this.bookingDates.checkOutDate,
      status: 0 // We explicitly set status to 0 (Pending) for new bookings.
    };

    this.bookingService.createBooking(bookingData).subscribe({
      next: (newBooking: any) => {
        // On success, redirect to the confirmation page.
        // We pass the ID of the newly created booking so the success page can fetch and display the receipt.
        this.router.navigate(['/booking-success'], {
            queryParams: { bookingId: newBooking.id }
        });
      },
      error: (err) => {
        console.error(err);
        alert('booking done already in the present date please chose another date.');
        this.isProcessing = false;
      }
    });
  }
}
