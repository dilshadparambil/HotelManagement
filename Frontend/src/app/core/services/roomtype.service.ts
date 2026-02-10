import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

/**
 * Service responsible for managing Room Type entities (e.g., Single, Double, Suite).
 * Provides standard CRUD operations to interact with the backend RoomTypes API.
 */
@Injectable({
  providedIn: 'root'
})
export class RoomTypeService {

  // API Endpoint pointing to the RoomTypes Controller on the backend
  private apiUrl = 'https://localhost:7231/api/RoomTypes'; // Check port

  constructor(private http: HttpClient) { }

  /**
   * Retrieves all room types from the server.
   * returns {Observable<any[]>} - An observable containing an array of room type objects.
   */
  getRoomTypes(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  /**
   * Retrieves a specific room type by its unique identifier.
   * params id - The unique ID of the room type to fetch.
   * returns {Observable<any>} - An observable containing the requested room type details.
   */
  getRoomTypeById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  /**
   * Creates a new room type record.
   * params data - The room type object payload (DTO) to send to the server.
   * returns {Observable<any>} - An observable of the created room type or server response.
   */
  createRoomType(data: any): Observable<any> {
    return this.http.post(this.apiUrl, data);
  }

  /**
   * Updates an existing room type.
   * params id - The unique ID of the room type to update.
   * params data - The updated room type data.
   * returns {Observable<any>} - An observable of the update result.
   */
  updateRoomType(id: number, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  /**
   * Deletes a room type by its ID.
   * params id - The unique ID of the room type to delete.
   * returns {Observable<any>} - An observable of the response string.
   */
  deleteRoomType(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, {
      // Important: The backend likely returns a plain text string (e.g., "Room Type Deleted Successfully")
      // rather than a JSON object. Setting responseType to 'text' prevents Angular
      // from attempting to parse that string as JSON, which would cause a syntax error.
      responseType: 'text'
    });
  }
}
