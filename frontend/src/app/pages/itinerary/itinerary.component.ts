import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DatePipe, CurrencyPipe, KeyValuePipe } from '@angular/common';
import { ItineraryService } from '../../core/services/itinerary.service';
import { TripService } from '../../core/services/trip.service';
import { Itinerary, Trip } from '../../core/models';
import { IconComponent } from '../../shared/icon/icon.component';

@Component({
  selector: 'app-itinerary',
  standalone: true,
  imports: [FormsModule, DatePipe, CurrencyPipe, KeyValuePipe, IconComponent],
  templateUrl: './itinerary.component.html',
  styleUrl: './itinerary.component.scss'
})
export class ItineraryComponent implements OnInit {
  private readonly itineraryService = inject(ItineraryService);
  private readonly tripService = inject(TripService);
  private readonly route = inject(ActivatedRoute);

  trips = signal<Trip[]>([]);
  selectedTripId = '';
  itinerary = signal<Itinerary | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.tripService.getAll().subscribe(trips => {
      this.trips.set(trips);
      const queryId = this.route.snapshot.queryParamMap.get('tripId');
      if (queryId) {
        this.selectedTripId = queryId;
        this.generate();
      } else if (trips.length > 0) {
        this.selectedTripId = trips[0].id;
      }
    });
  }

  generate(): void {
    if (!this.selectedTripId) return;

    this.loading.set(true);
    this.error.set(null);
    this.itinerary.set(null);

    this.itineraryService.getByTrip(this.selectedTripId).subscribe({
      next: (data) => {
        this.itinerary.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set(err.error?.message || err.error || 'Could not generate itinerary.');
        this.loading.set(false);
      }
    });
  }
}
