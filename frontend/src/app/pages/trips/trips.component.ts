import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { DatePipe, CurrencyPipe } from '@angular/common';
import { TripService } from '../../core/services/trip.service';
import { AuthService } from '../../core/services/auth.service';
import { Trip } from '../../core/models';
import { IconComponent } from '../../shared/icon/icon.component';

@Component({
  selector: 'app-trips',
  standalone: true,
  imports: [FormsModule, RouterLink, DatePipe, CurrencyPipe, IconComponent],
  templateUrl: './trips.component.html',
  styleUrl: './trips.component.scss'
})
export class TripsComponent implements OnInit {
  private readonly tripService = inject(TripService);
  private readonly authService = inject(AuthService);

  readonly isAdmin = computed(() => this.authService.currentRole() === 'Admin');

  trips = signal<Trip[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  showForm = signal(false);
  editingId = signal<string | null>(null);

  name = '';
  budget = 0;
  startDate = '';
  endDate = '';

  ngOnInit(): void {
    this.loadTrips();
  }

  loadTrips(): void {
    this.loading.set(true);
    this.tripService.getAll().subscribe({
      next: (data) => {
        this.trips.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load trips.');
        this.loading.set(false);
      }
    });
  }

  openCreate(): void {
    this.editingId.set(null);
    this.name = '';
    this.budget = 0;
    this.startDate = '';
    this.endDate = '';
    this.showForm.set(true);
  }

  openEdit(trip: Trip): void {
    this.editingId.set(trip.id);
    this.name = trip.name;
    this.budget = trip.budget;
    this.startDate = trip.startDate.split('T')[0];
    this.endDate = trip.endDate.split('T')[0];
    this.showForm.set(true);
  }

  cancelForm(): void {
    this.showForm.set(false);
    this.editingId.set(null);
  }

  save(): void {
    const dto = {
      name: this.name,
      budget: this.budget,
      startDate: this.startDate,
      endDate: this.endDate
    };

    const id = this.editingId();
    const onSuccess = () => {
      this.showForm.set(false);
      this.loadTrips();
    };
    const onError = (err: { error?: { message?: string } | string }) => {
      this.error.set(
        (typeof err.error === 'object' ? err.error?.message : err.error) || 'Failed to save trip.'
      );
    };

    if (id) {
      this.tripService.update(id, dto).subscribe({ next: onSuccess, error: onError });
    } else {
      this.tripService.create(dto).subscribe({ next: onSuccess, error: onError });
    }
  }

  deleteTrip(id: string): void {
    if (!confirm('Delete this trip?')) return;

    this.tripService.delete(id).subscribe({
      next: () => this.loadTrips(),
      error: () => this.error.set('Failed to delete trip.')
    });
  }
}
