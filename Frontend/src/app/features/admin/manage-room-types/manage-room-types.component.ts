import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoomTypeService } from '../../../core/services/roomtype.service';

/**
 * Component for managing the lifecycle of Room Type entities.
 * Features:
 * - List all available room types (e.g., Suite, Single, Deluxe).
 * - Create new room definitions.
 * - Edit existing room type details.
 * - Delete room types (with warnings about cascading effects).
 */
@Component({
  selector: 'app-manage-room-types',
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-room-types.component.html',
  styleUrl: './manage-room-types.component.scss'
})
export class ManageRoomTypesComponent implements OnInit {

  roomTypes: any[] = [];

  // UI State flags
  isLoading = true;
  showModal = false;
  isEditing = false;

  // Form Model initialized with default values
  currentType: any = {
    typeName: '',
    description: '',
    capacity: 1
  };

  constructor(private roomTypeService: RoomTypeService) { }

  /**
   * Lifecycle hook called after component initialization.
   * Triggers the initial fetch of the room type list.
   */
  ngOnInit(): void {
    this.loadData();
  }

  /**
   * Fetches the list of all room types from the backend.
   * Updates the local array and handles the loading spinner state.
   */
  loadData() {
    this.isLoading = true;
    this.roomTypeService.getRoomTypes().subscribe({
      next: (data) => {
        this.roomTypes = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  /**
   * Prepares the modal for creating a new room type.
   * Resets the form model to default/empty values.
   */
  openAddModal() {
    this.isEditing = false;
    this.currentType = { typeName: '', description: '', capacity: 1 };
    this.showModal = true;
  }

  /**
   * Prepares the modal for editing an existing room type.
   * params type - The room type object to edit.
   */
  openEditModal(type: any) {
    this.isEditing = true;
    // Create a shallow copy to avoid mutating the table data directly during editing
    this.currentType = { ...type };
    this.showModal = true;
  }

  /**
   * Closes the modal without saving changes.
   */
  closeModal() {
    this.showModal = false;
  }

  /**
   * Submits the form data to create or update a room type.
   * Handles type conversions and API calls based on the `isEditing` flag.
   */
  saveRoomType() {
    // Ensure capacity is treated as a number for the API
    this.currentType.capacity = Number(this.currentType.capacity);

    if (this.isEditing) {
      // Update existing room type
      this.roomTypeService.updateRoomType(this.currentType.id, this.currentType).subscribe(() => {
        this.loadData();
        this.closeModal();
        alert('Room Type updated!');
      });
    } else {
      // Create new room type
      this.roomTypeService.createRoomType(this.currentType).subscribe(() => {
        this.loadData();
        this.closeModal();
        alert('Room Type added!');
      });
    }
  }

  /**
   * Deletes a room type record after user confirmation.
   * Includes a specific warning about potential cascading deletes for associated Rooms.
   * params id - The ID of the room type to delete.
   */
  deleteRoomType(id: number) {
    if (confirm('Delete this Room Type? Warning: This may delete associated Rooms!')) {
      this.roomTypeService.deleteRoomType(id).subscribe(() => {
        this.loadData();
        alert('Room Type deleted!');
      });
    }
  }
}
