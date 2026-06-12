import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { AddLocationToTripRequest, TripLocation } from '../models';

@Injectable({ providedIn: 'root' })
export class TripLocationService {
  private readonly base = `${environment.apiUrl}/TripLocation`;

  constructor(private http: HttpClient) {}

  getByTrip(tripId: string) {
    return this.http.get<TripLocation[]>(`${this.base}/${tripId}`);
  }

  add(tripId: string, dto: AddLocationToTripRequest) {
    return this.http.post<void>(`${this.base}/${tripId}`, dto);
  }

  remove(tripId: string, locationId: string) {
    return this.http.delete<void>(`${this.base}/${tripId}/${locationId}`);
  }
}
