import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Service responsible for managing Booking entities.
 * Provides standard CRUD operations to interact with the backend Bookings API.
 */
@Injectable({
  providedIn: 'root'
})
export class BookingService {

  // API Endpoint pointing to the Bookings Controller on the backend
  private apiUrl = 'https://localhost:7231/api/Bookings';

  constructor(private http: HttpClient) { }

  /**
   * Retrieves all bookings from the server.
   * returns {Observable<any[]>} - An observable containing an array of booking objects.
   */
  getBookings(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  /**
   * Retrieves a specific booking by its unique identifier.
   * param id - The unique ID of the booking to fetch.
   * returns {Observable<any>} - An observable containing the requested booking details.
   */
  getBookingById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Creates a new booking record.
   * param bookingData - The booking object payload (DTO) to send to the server.
   * returns {Observable<any>} - An observable of the created booking or server response.
   */
  createBooking(bookingData: any): Observable<any> {
    return this.http.post(this.apiUrl, bookingData);
  }

  /**
   * Updates an existing booking.
   * param id - The unique ID of the booking to update.
   * param bookingData - The updated booking data.
   * returns {Observable<any>} - An observable of the update result.
   */
  updateBooking(id: number, bookingData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, bookingData);
  }

  /**
   * Deletes a booking by its ID.
   * param id - The unique ID of the booking to delete.
   * returns {Observable<any>} - An observable of the response string.
   */
  deleteBooking(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      // Important: The backend likely returns a plain text string (e.g., "Booking Deleted Successfully")
      // rather than a JSON object. Setting responseType to 'text' prevents Angular
      // from attempting to parse that string as JSON, which would cause a syntax error.
      responseType: 'text'
    });
  }
}
