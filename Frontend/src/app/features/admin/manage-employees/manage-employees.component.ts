import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HotelService } from '../../../core/services/hotel.service';
import { EmployeeService } from '../../../core/services/employee.service';

/**
 * Manages Employee records.
 * Includes functionality to filter out system administrators and assign employees to hotels.
 */
@Component({
  selector: 'app-manage-employees',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-employees.component.html',
  styleUrl: './manage-employees.component.scss'
})
export class ManageEmployeesComponent implements OnInit {

  employees: any[] = [];
  hotels: any[] = [];

  isLoading = true;
  showModal = false;
  isEditing = false;

  searchId: number | null = null;
  isSearching = false;

  currentEmployee: any = {
    fullName: '',
    email: '',
    role: '',
    hotelClassId: null,
    username: '',
    password: ''
  };

  constructor(
    private employeeService: EmployeeService,
    private hotelService: HotelService
  ) { }

  ngOnInit(): void {
    this.loadData();
  }

  /**
   * Loads hotels and employees.
   * Filters out the root admin account (ID 1) to prevent accidental modification.
   */
  loadData() {
    this.isLoading = true;

    // Fetch hotels first so the dropdowns can be populated
    this.hotelService.getAllHotels().subscribe(data => this.hotels = data);

    this.employeeService.getEmployees().subscribe({
      next: (data) => {
        // We use .filter() here to remove the employee with ID 1 from the view.
        // ID 1 is typically the "Super Admin" or "Root" user created by the system seed.
        // Hiding it prevents lower-level admins from accidentally deleting or locking out the main account.
        this.employees = data.filter(employee => employee.id !== 1);
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load employees', err);
        this.isLoading = false;
        alert('Failed to load employees. Check console for details.');
      }
    });
  }

  /**
   * Searches for an employee by ID.
   */
  searchEmployee() {
    if (this.searchId) {
      this.isLoading = true;
      this.isSearching = true;

      this.employeeService.getEmployeeById(this.searchId).subscribe({
        next: (data) => {
          // The table expects an array, so we wrap the single result object in an array.
          this.employees = [data];
          this.isLoading = false;
        },
        error: () => {
          alert('Employee not found!');
          this.isLoading = false;
          this.isSearching = false;
          // If search fails, revert to showing the full list.
          this.loadData();
        }
      });
    }
  }

  /**
   * Resets the search filter.
   */
  resetSearch() {
    this.searchId = null;
    this.isSearching = false;
    this.loadData();
  }

  openAddModal() {
    this.isEditing = false;
    this.currentEmployee = {
      fullName: '',
      email: '',
      role: '',
      hotelClassId: null,
      username: '',
      password: ''
    };
    this.showModal = true;
  }

  openEditModal(emp: any) {
    this.isEditing = true;
    // Use spread syntax to create a copy of the employee object.
    // This prevents the table row from updating in real-time while the user types in the modal,
    // which can be confusing UI behavior.
    this.currentEmployee = { ...emp };
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  /**
   * Saves employee data.
   * Ensures the foreign key `hotelClassId` is strictly typed as a number.
   */
  saveEmployee() {
    // HTML select inputs bind values as strings by default.
    // We explicitly cast `hotelClassId` to a Number to ensure strict type matching
    // with the backend DTO, preventing validation errors.
    this.currentEmployee.hotelClassId = Number(this.currentEmployee.hotelClassId);

    if (this.isEditing) {
      this.employeeService.updateEmployee(this.currentEmployee.id, this.currentEmployee).subscribe(() => {
        this.loadData();
        this.closeModal();
        alert('Employee updated!');
      });
    } else {
      this.employeeService.createEmployee(this.currentEmployee).subscribe(() => {
        this.loadData();
        this.closeModal();
        alert('Employee added!');
      });
    }
  }

  deleteEmployee(id: number) {
    if (confirm('Delete this employee?')) {
      this.employeeService.deleteEmployee(id).subscribe(() => this.loadData());
    }
  }
}
