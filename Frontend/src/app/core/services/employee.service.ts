import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Service responsible for managing Employee entities.
 * Provides standard CRUD operations to interact with the backend Employees API.
 */
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  // API Endpoint pointing to the Employees Controller on the backend
  private apiUrl = 'https://localhost:7231/api/Employees';

  constructor(private http: HttpClient) { }

  /**
   * Retrieves all employees from the server.
   * returns {Observable<any[]>} - An observable containing an array of employee objects.
   */
  getEmployees(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  /**
   * Retrieves a specific employee by their unique identifier.
   * params id - The unique ID of the employee to fetch.
   * returns {Observable<any>} - An observable containing the requested employee details.
   */
  getEmployeeById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Creates a new employee record.
   * params data - The employee object payload (DTO) to send to the server.
   * returns {Observable<any>} - An observable of the created employee or server response.
   */
  createEmployee(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  /**
   * Updates an existing employee.
   * params id - The unique ID of the employee to update.
   * params data - The updated employee data.
   * returns {Observable<any>} - An observable of the update result.
   */
  updateEmployee(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  /**
   * Deletes an employee by their ID.
   * params id - The unique ID of the employee to delete.
   * returns {Observable<any>} - An observable of the response string.
   */
  deleteEmployee(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      // Important: The backend likely returns a plain text string (e.g., "Employee Deleted Successfully")
      // rather than a JSON object. Setting responseType to 'text' prevents Angular
      // from attempting to parse that string as JSON, which would cause a syntax error.
      responseType: 'text'
    });
  }
}
