import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Itinerary } from '../models';

@Injectable({ providedIn: 'root' })
export class ItineraryService {
  constructor(private http: HttpClient) {}

  getByTrip(tripId: string) {
    return this.http.get<Itinerary>(`${environment.apiUrl}/Itinerary/${tripId}`);
  }
}
