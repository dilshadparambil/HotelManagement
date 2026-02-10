import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { HotelService } from '../../../core/services/hotel.service';
import { RoomService } from '../../../core/services/room.service'; // 1. Import RoomService
import { AuthService } from '../../../core/services/auth.service';
import { map, Observable } from 'rxjs';

@Component({
  selector: 'app-hotel-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './hotel-details.component.html',
  styleUrl: './hotel-details.component.scss'
})
export class HotelDetailsComponent implements OnInit {

  hotel: any = null;
  availableRoomTypes: any[] = []; // We will store unique room types here
  isLoading = true;
  minPrice: number = 0;

  searchCheckIn: string | null = null;
  searchCheckOut: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hotelService: HotelService,
    private roomService: RoomService,
    private authService: AuthService // 2. Inject it
  ) { }

  ngOnInit(): void {
    // --- 2. READ DATES FROM URL ---
    this.searchCheckIn = this.route.snapshot.queryParamMap.get('checkIn');
    this.searchCheckOut = this.route.snapshot.queryParamMap.get('checkOut');
    // --- END READ ---

    const hotelId = Number(this.route.snapshot.paramMap.get('id'));
    if (hotelId) {
      this.loadData(hotelId);
    }
  }

 loadData(hotelId: number) {
  this.isLoading = true;

  // 1. Load Hotel Details
  this.hotelService.getHotelById(hotelId).subscribe({
    next: (hotel) => {
      this.hotel = hotel; // Display hotel info immediately

      // 2. Prepare to load rooms
      let rooms$: Observable<any[]>;

      if (this.searchCheckIn && this.searchCheckOut) {
        // If we have dates, use the NEW search endpoint
        rooms$ = this.roomService.searchRoomsByHotel(
          hotelId,
          this.searchCheckIn,
          this.searchCheckOut
        );
      } else {
        // If no dates, use the OLD logic (Client-side filtering)
        rooms$ = this.roomService.getRooms().pipe(
          map(allRooms => allRooms.filter(r => r.hotelClassId === hotelId && r.status === 0))
        );
      }

      // 3. Subscribe to the rooms (WITH ERROR HANDLING)
      rooms$.subscribe({
        next: (rooms) => {
          this.processRoomTypes(rooms);
          this.isLoading = false; // Stop loading on success
        },
        error: (err) => {
          console.error('Error loading rooms:', err);
          this.isLoading = false; // Stop loading on error!
          // This prevents the "infinite loading" loop
        }
      });
    },
    error: (err) => {
      console.error('Error loading hotel:', err);
      this.isLoading = false;
    }
  });
}

  processRoomTypes(rooms: any[]) {
    const uniqueTypes = new Map();

    rooms.forEach(room => {
      if (!uniqueTypes.has(room.roomTypeId)) {
        uniqueTypes.set(room.roomTypeId, {
          typeId: room.roomTypeId,
          typeName: room.roomType,
          price: room.pricePerNight,
          // We keep one real 'roomId' to use for booking later
          exampleRoomId: room.id,
          capacity: room.capacity,
          description: room.description
        });
      }
    });

    this.availableRoomTypes = Array.from(uniqueTypes.values());

    // --- ADD THIS LOGIC ---
    // Find the minimum price from the available rooms
    if (this.availableRoomTypes.length > 0) {
      this.minPrice = this.availableRoomTypes.reduce((min, room) => {
        return room.price < min ? room.price : min;
      }, this.availableRoomTypes[0].price); // Start with the first room's price
    }
  }

  // --- ADD THIS FUNCTION ---
  // We'll call this from the "Book Now" button
  scrollToRooms() {
    // Find the element with the ID 'rooms-section' and scroll to it
    document.getElementById('rooms-section')?.scrollIntoView({ behavior: 'smooth' });
  }

  selectRoom(room: any) {
  this.router.navigate(['/checkout'], {
    queryParams: {
      roomId: room.exampleRoomId, // <-- Use the correct property
      hotelId: this.hotel?.id,
      roomType: room.typeName,     // <-- Use the correct property
      price: room.price,           // <-- Use the correct property
      hotelName: this.hotel?.name
    }
  });
  }
  // --- END FIX ---
  }

