import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Service responsible for managing Hotel entities.
 * Handles basic CRUD operations as well as advanced search functionality.
 */
@Injectable({
  providedIn: 'root'
})
export class HotelService {

  // Match this to your running API port
  private apiUrl = 'https://localhost:7231/api/Hotels';

  constructor(private http: HttpClient) { }

  /**
   * Retrieves all hotels from the server.
   * returns {Observable<any[]>} - An observable containing an array of all hotels.
   */
  getAllHotels(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  /**
   * Searches for hotels based on provided filters.
   * Constructs query parameters dynamically to append to the URL.
   * params filters - An object containing criteria like destination, dates, price, etc.
   * returns {Observable<any[]>} - An observable of the filtered hotel list.
   */
  searchHotels(filters: any): Observable<any[]> {
    let params = new HttpParams();

    // Conditionally append parameters if they exist in the filter object
    if (filters.destination) params = params.append('destination', filters.destination);
    if (filters.checkIn) params = params.append('checkIn', filters.checkIn);
    if (filters.checkOut) params = params.append('checkOut', filters.checkOut);
    if (filters.maxPrice) params = params.append('maxPrice', filters.maxPrice);
    if (filters.minRating) params = params.append('minRating', filters.minRating);
    if (filters.sortBy) params = params.append('sortBy', filters.sortBy);

    // Send the GET request to the specific search endpoint with the constructed params
    return this.http.get<any[]>(`${this.apiUrl}/search`, {
      params: params
    });
  }

  /**
   * Retrieves a specific hotel by its unique identifier.
   * params id - The unique ID of the hotel to fetch.
   * returns {Observable<any>} - An observable containing the hotel details.
   */
  getHotelById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Deletes a hotel by its ID.
   * params id - The unique ID of the hotel to delete.
   * returns {Observable<any>} - An observable of the response string.
   */
  deleteHotel(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      // Important: The backend likely returns a plain text string (e.g., "Hotel Deleted Successfully")
      // rather than a JSON object. Setting responseType to 'text' prevents Angular
      // from attempting to parse that string as JSON, which would cause a syntax error.
      responseType: 'text'
    });
  }

  /**
   * Creates a new hotel record.
   * params hotelData - The hotel object payload (DTO) to send to the server.
   * returns {Observable<any>} - An observable of the created hotel or server response.
   */
  createHotel(hotelData: any): Observable<any> {
    return this.http.post(this.apiUrl, hotelData);
  }

  /**
   * Updates an existing hotel.
   * params id - The unique ID of the hotel to update.
   * params hotelData - The updated hotel data.
   * returns {Observable<any>} - An observable of the update result.
   */
  updateHotel(id: number, hotelData: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, hotelData);
  }
}
