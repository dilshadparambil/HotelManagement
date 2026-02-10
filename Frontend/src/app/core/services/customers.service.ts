import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Service responsible for managing Customer entities.
 * Provides standard CRUD operations to interact with the backend Customers API.
 */
@Injectable({
  providedIn: 'root'
})
export class CustomerService {

  // API Endpoint pointing to the Customers Controller on the backend
  private apiUrl = 'https://localhost:7231/api/Customers';

  constructor(private http: HttpClient) { }

  /**
   * Retrieves all customers from the server.
   * returns {Observable<any[]>} - An observable containing an array of customer objects.
   */
  getCustomers(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  /**
   * Retrieves a specific customer by their unique identifier.
   * params id - The unique ID of the customer to fetch.
   * returns {Observable<any>} - An observable containing the requested customer details.
   */
  getCustomerById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Creates a new customer record.
   * params customerData - The customer object payload (DTO) to send to the server.
   * returns {Observable<any>} - An observable of the created customer or server response.
   */
  createCustomer(customerData: any): Observable<any> {
    return this.http.post(this.apiUrl, customerData);
  }

  /**
   * Updates an existing customer.
   * params id - The unique ID of the customer to update.
   * params customerData - The updated customer data.
   * returns {Observable<any>} - An observable of the update result.
   */
  updateCustomer(id: number, customerData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, customerData);
  }

  /**
   * Deletes a customer by their ID.
   * params id - The unique ID of the customer to delete.
   * returns {Observable<any>} - An observable of the response string.
   */
  deleteCustomer(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      // Important: The backend likely returns a plain text string (e.g., "Customer Deleted Successfully")
      // rather than a JSON object. Setting responseType to 'text' prevents Angular
      // from attempting to parse that string as JSON, which would cause a syntax error.
      responseType: 'text'
    });
  }
}
