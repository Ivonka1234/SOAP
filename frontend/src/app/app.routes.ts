import { Routes } from '@angular/router';
import { LayoutComponent } from './shared/layout/layout.component';
import { HomeComponent } from './pages/home/home.component';
import { AuthComponent } from './pages/auth/auth.component';
import { TripsComponent } from './pages/trips/trips.component';
import { TripDetailComponent } from './pages/trip-detail/trip-detail.component';
import { LocationsComponent } from './pages/locations/locations.component';
import { ItineraryComponent } from './pages/itinerary/itinerary.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'auth', component: AuthComponent },
      { path: 'trips', component: TripsComponent, canActivate: [authGuard] },
      { path: 'trips/:id', component: TripDetailComponent, canActivate: [authGuard] },
      { path: 'locations', component: LocationsComponent, canActivate: [authGuard] },
      { path: 'itinerary', component: ItineraryComponent, canActivate: [authGuard] }
    ]
  },
  { path: '**', redirectTo: '' }
];
