import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Service responsible for managing Room entities.
 * Provides standard CRUD operations and specialized search functionality
 * to find available rooms within specific hotels.
 */
@Injectable({
  providedIn: 'root'
})
export class RoomService {

  // API Endpoint pointing to the Rooms Controller on the backend
  private apiUrl = 'https://localhost:7231/api/Rooms';

  constructor(private http: HttpClient) { }

  /**
   * Retrieves all rooms from the server.
   * returns {Observable<any[]>} - An observable containing an array of all rooms.
   */
  getRooms(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  /**
   * Retrieves a specific room by its unique identifier.
   * params id - The unique ID of the room to fetch.
   * returns {Observable<any>} - An observable containing the room details.
   */
  getRoomById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Creates a new room record.
   * params data - The room object payload (DTO) to send to the server.
   * returns {Observable<any>} - An observable of the created room or server response.
   */
  createRoom(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  /**
   * Updates an existing room.
   * params id - The unique ID of the room to update.
   * params data - The updated room data.
   * returns {Observable<any>} - An observable of the update result.
   */
  updateRoom(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  /**
   * Deletes a room by its ID.
   * params id - The unique ID of the room to delete.
   * returns {Observable<any>} - An observable of the response string.
   */
  deleteRoom(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      // Important: The backend likely returns a plain text string (e.g., "Room Deleted Successfully")
      // rather than a JSON object. Setting responseType to 'text' prevents Angular
      // from attempting to parse that string as JSON, which would cause a syntax error.
      responseType: 'text'
    });
  }

  /**
   * Searches for rooms within a specific hotel that are available for a given date range.
   * params hotelId - The ID of the hotel to search within.
   * params checkIn - The desired check-in date string.
   * params checkOut - The desired check-out date string.
   * returns {Observable<any[]>} - An observable of available rooms matching the criteria.
   */
  searchRoomsByHotel(hotelId: number, checkIn: string, checkOut: string): Observable<any[]> {

    // Construct HTTP parameters for the query string
    // Using .set() is safe here because HttpParams is immutable (returns a new instance)
    let params = new HttpParams()
      .set('checkIn', checkIn)
      .set('checkOut', checkOut);

    // Calls the specialized endpoint: GET /api/Rooms/search-by-hotel/{hotelId}?checkIn=...&checkOut=...
    // The 'params' object is automatically serialized into the URL query string.
    return this.http.get<any[]>(`${this.apiUrl}/search-by-hotel/${hotelId}`, { params: params });
  }
}
