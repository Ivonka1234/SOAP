import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { TripService } from '../../core/services/trip.service';
import { TripLocationService } from '../../core/services/trip-location.service';
import { LocationService } from '../../core/services/location.service';
import { Location, Trip } from '../../core/models';
import { IconComponent } from '../../shared/icon/icon.component';

@Component({
  selector: 'app-trip-detail',
  standalone: true,
  imports: [RouterLink, FormsModule, DatePipe, CurrencyPipe, IconComponent],
  templateUrl: './trip-detail.component.html',
  styleUrl: './trip-detail.component.scss'
})
export class TripDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly tripService = inject(TripService);
  private readonly tripLocationService = inject(TripLocationService);
  private readonly locationService = inject(LocationService);

  trip = signal<Trip | null>(null);
  allLocations = signal<Location[]>([]);
  cost = signal<number | null>(null);
  duration = signal<number | null>(null);
  overBudget = signal<boolean | null>(null);
  loading = signal(true);
  error = signal<string | null>(null);
  selectedLocationId = '';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.loadTrip(id);
    this.locationService.getAll().subscribe(loc => this.allLocations.set(loc));
  }

  loadTrip(id: string): void {
    this.loading.set(true);
    this.tripService.getById(id).subscribe({
      next: (trip) => {
        this.trip.set(trip);
        this.loading.set(false);
        this.loadStats(id);
      },
      error: () => {
        this.error.set('Trip not found.');
        this.loading.set(false);
      }
    });
  }

  loadStats(id: string): void {
    this.tripService.getCost(id).subscribe(c => this.cost.set(c));
    this.tripService.getDuration(id).subscribe(d => this.duration.set(d));
    this.tripService.isOverBudget(id).subscribe(o => this.overBudget.set(o));
  }

  addLocation(): void {
    const trip = this.trip();
    if (!trip || !this.selectedLocationId) return;

    this.tripLocationService.add(trip.id, { locationId: this.selectedLocationId }).subscribe({
      next: () => {
        this.selectedLocationId = '';
        this.loadTrip(trip.id);
      },
      error: (err) => {
        this.error.set(err.error || 'Could not add location to trip.');
      }
    });
  }
}
