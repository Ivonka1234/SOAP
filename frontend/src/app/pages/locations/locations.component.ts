import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { LocationService } from '../../core/services/location.service';
import { AuthService } from '../../core/services/auth.service';
import { Location } from '../../core/models';
import { IconComponent } from '../../shared/icon/icon.component';

@Component({
  selector: 'app-locations',
  standalone: true,
  imports: [FormsModule, CurrencyPipe, IconComponent],
  templateUrl: './locations.component.html',
  styleUrl: './locations.component.scss'
})
export class LocationsComponent implements OnInit {
  private readonly locationService = inject(LocationService);
  private readonly authService = inject(AuthService);

  readonly isAdmin = computed(() => this.authService.currentRole() === 'Admin');

  locations = signal<Location[]>([]);
  loading = signal(true);
  error = signal<string | null>(null);
  showForm = signal(false);
  editingId = signal<string | null>(null);

  name = '';
  country = '';
  estimatedCost = 0;
  priority = 1;

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.locationService.getAll().subscribe({
      next: (data) => {
        this.locations.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Failed to load locations.');
        this.loading.set(false);
      }
    });
  }

  openCreate(): void {
    this.editingId.set(null);
    this.resetForm();
    this.showForm.set(true);
  }

  openEdit(loc: Location): void {
    this.editingId.set(loc.id);
    this.name = loc.name;
    this.country = loc.country;
    this.estimatedCost = loc.estimatedCost;
    this.priority = loc.priority;
    this.showForm.set(true);
  }

  cancelForm(): void {
    this.showForm.set(false);
    this.editingId.set(null);
  }

  save(): void {
    const dto = {
      name: this.name,
      country: this.country,
      estimatedCost: this.estimatedCost,
      visitDurationHours: 1,
      priority: this.priority
    };

    const id = this.editingId();
    const request$ = id
      ? this.locationService.update(id, dto)
      : this.locationService.create(dto);

    request$.subscribe({
      next: () => {
        this.showForm.set(false);
        this.load();
      },
      error: (err) => {
        this.error.set(err.error?.message || err.error || 'Failed to save location.');
      }
    });
  }

  delete(id: string): void {
    if (!confirm('Delete this location?')) return;
    this.locationService.delete(id).subscribe({
      next: () => this.load(),
      error: () => this.error.set('Failed to delete location.')
    });
  }

  private resetForm(): void {
    this.name = '';
    this.country = '';
    this.estimatedCost = 0;
    this.priority = 1;
  }
}
