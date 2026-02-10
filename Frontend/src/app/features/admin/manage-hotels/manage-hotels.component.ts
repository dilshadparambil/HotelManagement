import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HotelService } from '../../../core/services/hotel.service';

/**
 * Component for managing the lifecycle of Hotel entities.
 * Features:
 * - List all hotels.
 * - Search for a specific hotel by ID.
 * - Create new hotel entries.
 * - Edit existing hotel details.
 * - Delete hotels.
 */
@Component({
  selector: 'app-manage-hotels',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './manage-hotels.component.html',
  styleUrl: './manage-hotels.component.scss'
})
export class ManageHotelsComponent implements OnInit {

  hotels: any[] = [];

  // UI State flags
  isLoading = true;
  showModal = false;
  isEditing = false;

  // Form Model
  currentHotel: any = {};

  // Search State
  searchId: number | null = null;
  isSearching = false;

  constructor(private hotelService: HotelService) { }

  /**
   * Lifecycle hook called after component initialization.
   * Triggers the initial fetch of the hotel list.
   */
  ngOnInit(): void {
    this.loadHotels();
  }

  /**
   * Fetches the list of all hotels from the backend.
   * Updates the local array and handles the loading spinner state.
   */
  loadHotels() {
    this.isLoading = true;
    // --- FIX: Call the new function ---
    this.hotelService.getAllHotels().subscribe({
      next: (data) => {
        this.hotels = data;
        this.isLoading = false;
      },
      // Placeholder for error handling if needed in future
      // error: (err) => { ... }
    });
  }

  /**
   * Searches for a specific hotel by its ID.
   * If found, replaces the current table list with the single result.
   * If not found, shows an alert and resets the list.
   */
  searchHotel() {
    if (this.searchId) {
      this.isLoading = true;
      this.isSearching = true;

      this.hotelService.getHotelById(this.searchId).subscribe({
        next: (data) => {
          this.hotels = [data]; // Show only this result in the table
          this.isLoading = false;
        },
        error: () => {
          alert('Hotel not found!');
          this.isLoading = false;
          this.isSearching = false;
          this.loadHotels(); // Reset list if not found
        }
      });
    }
  }

  /**
   * Prepares the modal for creating a new hotel.
   * Resets the form model to an empty object.
   */
  openAddModal() {
    this.isEditing = false;
    this.currentHotel = {}; // Clear form
    this.showModal = true;
  }

  /**
   * Prepares the modal for editing an existing hotel.
   * params hotel - The hotel object to edit.
   */
  openEditModal(hotel: any) {
    this.isEditing = true;
    // Create a shallow copy to avoid mutating the table data directly during editing
    this.currentHotel = { ...hotel };
    this.showModal = true;
  }

  /**
   * Closes the modal without saving changes.
   */
  closeModal() {
    this.showModal = false;
  }

  /**
   * Submits the form data to create or update a hotel record.
   * Handles the API call based on the `isEditing` flag.
   */
  saveHotel() {
    if (this.isEditing) {
      // UPDATE Logic
      this.hotelService.updateHotel(this.currentHotel.id, this.currentHotel).subscribe(() => {
        this.loadHotels();
        this.closeModal();
        alert('Hotel updated!');
      });
    } else {
      // CREATE Logic
      this.hotelService.createHotel(this.currentHotel).subscribe(() => {
        this.loadHotels();
        this.closeModal();
        alert('Hotel added!');
      });
    }
  }

  /**
   * Deletes a hotel record after user confirmation.
   * params id - The ID of the hotel to delete.
   */
  deleteHotel(id: number) {
    if (confirm('Are you sure you want to delete this hotel?')) {
      this.hotelService.deleteHotel(id).subscribe({
        next: () => {
          // This runs ONLY after the delete is successful
          this.loadHotels(); // <--- This refreshes the list automatically
          alert('Hotel deleted successfully');
        },
        error: (err) => {
          console.error('Error deleting hotel', err);
          alert('Failed to delete hotel');
        }
      });
    }
  }
}
