import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common'; // Import DatePipe
import { FormsModule } from '@angular/forms';
import { BookingService } from '../../../core/services/booking.service';
import { RoomService } from '../../../core/services/room.service';
import { CustomerService } from '../../../core/services/customers.service';

/**
 * Component for managing the lifecycle of Bookings.
 * Features:
 * - List all bookings with related Customer and Room data.
 * - Create new bookings.
 * - Edit existing bookings (handling date format conversions).
 * - Delete bookings.
 */
@Component({
  selector: 'app-manage-bookings',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe], // Add DatePipe
  templateUrl: './manage-bookings.component.html',
  styleUrl: './manage-bookings.component.scss'
})
export class ManageBookingsComponent implements OnInit {

  bookings: any[] = [];
  customers: any[] = [];
  rooms: any[] = [];

  // UI State flags
  isLoading = true;
  showModal = false;
  isEditing = false;

  // Model for the form. initialized with empty values.
  currentBooking: any = {
    customerId: null,
    roomId: null,
    checkInDate: '',
    checkOutDate: '',
    status: 0
  };

  // Mapping for integer status codes to readable labels
  statusOptions = [
    { value: 0, label: 'Pending' },
    { value: 1, label: 'Confirmed' },
    { value: 2, label: 'Cancelled' },
    { value: 3, label: 'Completed' }
  ];

  constructor(
    private bookingService: BookingService,
    private customerService: CustomerService,
    private roomService: RoomService
  ) { }

  /**
   * Lifecycle hook called after component initialization.
   * Triggers the initial data fetch.
   */
  ngOnInit(): void {
    this.loadData();
  }

  /**
   * Fetches all necessary data from the backend.
   * Populates bookings, customers (for dropdowns), and rooms (for dropdowns).
   */
  loadData() {
    this.isLoading = true;
    // Load all data needed for the table and dropdowns
    this.bookingService.getBookings().subscribe(data => {
      this.bookings = data;
      this.isLoading = false;
    });
    // Fetch auxiliary data for foreign key selections
    this.customerService.getCustomers().subscribe(data => this.customers = data);
    this.roomService.getRooms().subscribe(data => this.rooms = data);
  }

  /**
   * Prepares the modal for creating a new booking.
   * Resets the form model to default values.
   */
  openAddModal() {
    this.isEditing = false;
    this.currentBooking = {
      customerId: null, // Change to null
      roomId: null,     // Change to null
      checkInDate: '',
      checkOutDate: '',
      status: 0
    };
    this.showModal = true;
  }

  /**
   * Prepares the modal for editing an existing booking.
   * params booking - The booking object to edit.
   */
  openEditModal(booking: any) {
    this.isEditing = true;
    // Create a shallow copy to avoid mutating the table data directly before saving
    this.currentBooking = { ...booking };

    // Format dates for <input type="datetime-local"> or <input type="date">
    // The API sends "2025-11-10T14:00:00". Input type="date" strictly requires "yyyy-MM-dd".
    // We split by 'T' to extract just the date part.
    if (this.currentBooking.checkInDate) {
      this.currentBooking.checkInDate = this.currentBooking.checkInDate.split('T')[0];
    }
    if (this.currentBooking.checkOutDate) {
      this.currentBooking.checkOutDate = this.currentBooking.checkOutDate.split('T')[0];
    }

    this.showModal = true;
  }

  /**
   * Closes the modal without saving changes.
   */
  closeModal() {
    this.showModal = false;
  }

  /**
   * Submits the form data to create or update a booking.
   * Handles type conversion for IDs and Status before sending to API.
   */
  saveBooking() {
    // Convert strings back to numbers/dates if needed
    // HTML select inputs might bind values as strings, so we ensure they are numbers here.
    this.currentBooking.customerId = Number(this.currentBooking.customerId);
    this.currentBooking.roomId = Number(this.currentBooking.roomId);
    this.currentBooking.status = Number(this.currentBooking.status);

    if (this.isEditing) {
      // Update existing booking
      this.bookingService.updateBooking(this.currentBooking.id, this.currentBooking).subscribe(() => {
        this.loadData();
        this.closeModal();
      });
    } else {
      // Create new booking
      this.bookingService.createBooking(this.currentBooking).subscribe(() => {
        this.loadData();
        this.closeModal();
      });
    }
  }

  /**
   * Deletes a booking after user confirmation.
   * params id - The ID of the booking to delete.
   */
  deleteBooking(id: number) {
    if (confirm('Delete this booking?')) {
      this.bookingService.deleteBooking(id).subscribe(() => this.loadData());
    }
  }

  /**
   * Helper to convert numeric status codes into human-readable labels.
   * params value - The status code (e.g., 0, 1).
   * returns {string} - The label (e.g., "Pending", "Confirmed").
   */
  getStatusLabel(value: number): string {
    return this.statusOptions.find(o => o.value === value)?.label || 'Unknown';
  }
}
