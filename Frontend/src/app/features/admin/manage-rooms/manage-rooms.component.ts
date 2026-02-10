import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoomService } from '../../../core/services/room.service';
import { HotelService } from '../../../core/services/hotel.service';
import { RoomTypeService } from '../../../core/services/roomtype.service';

/**
 * Component for managing the lifecycle of Room entities.
 * Features:
 * - List all rooms with their status and pricing.
 * - Search for specific rooms by ID.
 * - Create new rooms (requiring Hotel and Room Type selection).
 * - Edit existing room details.
 * - Delete rooms.
 */
@Component({
  selector: 'app-manage-rooms',
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-rooms.component.html',
  styleUrl: './manage-rooms.component.scss'
})
export class ManageRoomsComponent implements OnInit {

  rooms: any[] = [];
  hotels: any[] = [];      // Data source for the 'Assigned Hotel' dropdown
  roomTypes: any[] = [];   // Data source for the 'Room Type' dropdown

  // UI State flags
  isLoading = true;
  showModal = false;
  isEditing = false;

  // Search State
  searchId: number | null = null;
  isSearching = false;

  // Form Model initialized with defaults
  currentRoom: any = {
    roomNumber: '',
    pricePerNight: 0,
    status: 0,
    hotelClassId: 0, // Note: Matches your DTO property
    roomTypeId: 0
  };

  // Enum mapping for UI display.
  // 0 = Available, 1 = Booked, 2 = Under Maintenance
  statusOptions = [
    { value: 0, label: 'Available' },
    { value: 1, label: 'Booked' },
    { value: 2, label: 'Under Maintenance' }
  ];

  constructor(
    private roomService: RoomService,
    private hotelService: HotelService,
    private roomTypeService: RoomTypeService
  ) { }

  /**
   * Lifecycle hook called after component initialization.
   * Triggers the initial data fetch.
   */
  ngOnInit(): void {
    this.loadData();
  }

  /**
   * Fetches the main list of rooms and all dependency data (Hotels, Room Types).
   * The dependency data is required to populate the <select> dropdowns in the add/edit modal.
   */
  loadData() {
    this.isLoading = true;
    this.roomService.getRooms().subscribe(data => {
      this.rooms = data;
      this.isLoading = false;
    });

    // --- FIX: Call the new function ---
    // Fetch hotels for the dropdown
    this.hotelService.getAllHotels().subscribe(data => this.hotels = data);

    // Fetch room types for the dropdown
    this.roomTypeService.getRoomTypes().subscribe(data => this.roomTypes = data);
  }

  /**
   * Searches for a specific room by its ID.
   * If found, filters the view to show only that room.
   * If not found, alerts the user and resets the view.
   */
  searchRoom() {
    if (this.searchId) {
      this.isLoading = true;
      this.isSearching = true;

      this.roomService.getRoomById(this.searchId).subscribe({
        next: (data) => {
          // Wrap result in array to reuse table structure
          this.rooms = [data];
          this.isLoading = false;
        },
        error: () => {
          alert('Room not found!');
          this.isLoading = false;
          this.isSearching = false;
          // Revert to full list on error
          this.loadData();
        }
      });
    }
  }

  /**
   * Clears the search filter and reloads the full list of rooms.
   */
  resetSearch() {
    this.searchId = null;
    this.isSearching = false;
    this.loadData();
  }

  /**
   * Prepares the modal for creating a new room.
   * Resets the form model to ensure clean input fields.
   */
  openAddModal() {
    this.isEditing = false;
    this.currentRoom = {
      roomNumber: '',
      pricePerNight: null, // Change to null for clean input
      status: 0,           // Default to Available (valid)
      hotelClassId: null,  // Change to null so 'required' validation works
      roomTypeId: null     // Change to null so 'required' validation works
    };
    this.showModal = true;
  }

  /**
   * Prepares the modal for editing an existing room.
   * params room - The room object to edit.
   */
  openEditModal(room: any) {
    this.isEditing = true;
    // We need to map the flat Room object to our ID-based form
    // (Assuming your RoomResponseDTO has HotelName but not HotelId,
    // ideally your backend returns IDs too. If not, you might need to find the ID
    // from the name, but let's assume for now we're editing properties we have).

    // Since your GetById DTO doesn't return IDs for Hotel/RoomType (only names),
    // editing the Hotel/Type might be tricky without those IDs.
    // For now, we will pre-fill what we can.
    this.currentRoom = { ...room };
    this.showModal = true;
  }

  /**
   * Closes the modal without saving changes.
   */
  closeModal() {
    this.showModal = false;
  }

  /**
   * Submits the form data to create or update a room.
   * Handles type conversion to ensure numeric IDs are sent to the API.
   */
  saveRoom() {
    // Ensure numbers are sent as numbers (HTML inputs often bind as strings)
    this.currentRoom.pricePerNight = Number(this.currentRoom.pricePerNight);
    this.currentRoom.hotelClassId = Number(this.currentRoom.hotelClassId);
    this.currentRoom.roomTypeId = Number(this.currentRoom.roomTypeId);
    this.currentRoom.status = Number(this.currentRoom.status);

    if (this.isEditing) {
      // Update existing room
      this.roomService.updateRoom(this.currentRoom.id, this.currentRoom).subscribe(() => {
        this.loadData();
        this.closeModal();
        alert('Room updated!');
      });
    } else {
      // Create new room
      this.roomService.createRoom(this.currentRoom).subscribe(() => {
        this.loadData();
        this.closeModal();
        alert('Room added!');
      });
    }
  }

  /**
   * Deletes a room record after user confirmation.
   * params id - The ID of the room to delete.
   */
  deleteRoom(id: number) {
    if (confirm('Delete this room?')) {
      this.roomService.deleteRoom(id).subscribe(() => {
        this.loadData();
        alert('Room deleted!');
      });
    }
  }

  /**
   * Helper to convert numeric status codes into human-readable labels.
   * params value - The status code (e.g., 0, 1).
   * returns {string} - The label (e.g., "Available", "Booked").
   */
  getStatusLabel(value: number): string {
    return this.statusOptions.find(o => o.value === value)?.label || 'Unknown';
  }
}
