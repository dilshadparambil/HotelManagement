import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CustomerService } from '../../../core/services/customers.service';

/**
 * Component for managing the lifecycle of Customer entities.
 * Features:
 * - List all registered customers.
 * - Search for a specific customer by ID.
 * - Create new customer profiles (Admin action).
 * - Edit existing customer details.
 * - Delete customer records.
 */
@Component({
  selector: 'app-manage-customers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-customers.component.html',
  styleUrl: './manage-customers.component.scss'
})
export class ManageCustomersComponent implements OnInit {

  customers: any[] = [];

  // UI State flags
  isLoading = true;
  showModal = false;
  isEditing = false;

  // Search State
  searchId: number | null = null;
  isSearching = false;

  // Form Model initialized with empty values
  currentCustomer: any = {
    fullName: '',
    email: '',
    phoneNumber: '',
    idProofNumber: ''
  };

  constructor(private customerService: CustomerService) { }

  /**
   * Lifecycle hook called after component initialization.
   * Triggers the initial fetch of the customer list.
   */
  ngOnInit(): void {
    this.loadCustomers();
  }

  /**
   * Fetches the list of all customers from the backend.
   * Updates the local array and handles the loading spinner state.
   */
  loadCustomers() {
    this.isLoading = true;
    this.customerService.getCustomers().subscribe({
      next: (data) => {
        this.customers = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading customers', err);
        this.isLoading = false;
      }
    });
  }

  /**
   * Searches for a specific customer by their ID.
   * If found, replaces the current table list with the single result.
   * If not found, shows an alert and resets the list.
   */
  searchCustomer() {
    if (this.searchId) {
      this.isLoading = true;
      this.isSearching = true;

      this.customerService.getCustomerById(this.searchId).subscribe({
        next: (data) => {
          // Wrap the single result in an array to reuse the existing table structure
          this.customers = [data];
          this.isLoading = false;
        },
        error: () => {
          alert('Customer not found!');
          this.isLoading = false;
          // On error, reset search state so user can try again or see full list
          this.isSearching = false;
          this.loadCustomers();
        }
      });
    }
  }

  /**
   * Clears the current search filter and reloads the full customer list.
   */
  resetSearch() {
    this.searchId = null;
    this.isSearching = false;
    this.loadCustomers();
  }

  /**
   * Prepares the modal for creating a new customer.
   * Resets the form model to blank values.
   */
  openAddModal() {
    this.isEditing = false;
    this.currentCustomer = {
      fullName: '',
      email: '',
      phoneNumber: '',
      idProofNumber: ''
    };
    this.showModal = true;
  }

  /**
   * Prepares the modal for editing an existing customer.
   * params customer - The customer object to edit.
   */
  openEditModal(customer: any) {
    this.isEditing = true;
    // Create a shallow copy to avoid mutating the table data immediately
    this.currentCustomer = { ...customer };
    this.showModal = true;
  }

  /**
   * Closes the modal without saving changes.
   */
  closeModal() {
    this.showModal = false;
  }

  /**
   * Submits the form data to create or update a customer record.
   * Handles the API call based on the `isEditing` flag.
   */
  saveCustomer() {
    if (this.isEditing) {
      // Update existing customer
      this.customerService.updateCustomer(this.currentCustomer.id, this.currentCustomer).subscribe(() => {
        this.loadCustomers();
        this.closeModal();
        alert('Customer updated!');
      });
    } else {
      // Create new customer
      this.customerService.createCustomer(this.currentCustomer).subscribe(() => {
        this.loadCustomers();
        this.closeModal();
        alert('Customer added!');
      });
    }
  }

  /**
   * Deletes a customer record after user confirmation.
   * params id - The ID of the customer to delete.
   */
  deleteCustomer(id: number) {
    if (confirm('Are you sure you want to delete this customer?')) {
      this.customerService.deleteCustomer(id).subscribe(() => {
        this.loadCustomers();
        alert('Customer deleted!');
      });
    }
  }
}
