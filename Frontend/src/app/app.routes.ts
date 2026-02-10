import { Routes } from '@angular/router';
import { AdminLoginComponent } from './features/admin/admin-login/admin-login.component';
import { HomeComponent } from './features/public/home/home.component';
import { AdminDashboardComponent } from './features/admin/admin-dashboard/admin-dashboard.component';
import { ManageHotelsComponent } from './features/admin/manage-hotels/manage-hotels.component';
import { ManageRoomsComponent } from './features/admin/manage-rooms/manage-rooms.component';
import { ManageBookingsComponent } from './features/admin/manage-bookings/manage-bookings.component';
import { ManageEmployeesComponent } from './features/admin/manage-employees/manage-employees.component';
import { ManageCustomersComponent } from './features/admin/manage-customers/manage-customers.component';
import { HotelListComponent } from './features/public/hotel-list/hotel-list.component';
import { ManageRoomTypesComponent } from './features/admin/manage-room-types/manage-room-types.component';
import { HotelDetailsComponent } from './features/public/hotel-details/hotel-details.component';
import { CheckoutComponent } from './features/public/checkout/checkout.component';
import { BookingSuccessComponent } from './features/public/booking-success/booking-success.component';
import { LoginComponent } from './features/public/login/login.component';
import { SignupComponent } from './features/public/signup/signup.component';
import { adminGuard } from './core/guards/admin.guard';
import { customerGuard } from './core/guards/customer.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'search', component: HotelListComponent },
  { path: 'hotel/:id', component: HotelDetailsComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },

  {
    path: 'checkout',
    component: CheckoutComponent,
    canActivate: [customerGuard] 
  },
  {
    path: 'booking-success',
    component: BookingSuccessComponent,
    canActivate: [customerGuard]
  },

  { path: 'admin/login', component: AdminLoginComponent },
  {
    path: 'admin/dashboard',
    component: AdminDashboardComponent,
    canActivate: [adminGuard],
    children: [
      { path: '', redirectTo: 'hotels', pathMatch: 'full' }, // Default to Hotels
      { path: 'hotels', component: ManageHotelsComponent },
      { path: 'rooms', component: ManageRoomsComponent },
      { path: 'room-types', component: ManageRoomTypesComponent },
      { path: 'bookings', component: ManageBookingsComponent },
      { path: 'employees', component: ManageEmployeesComponent },
      { path: 'customers', component: ManageCustomersComponent },
    ]
  },
  { path: '**', redirectTo: '' }
];
