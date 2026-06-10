import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CreateTripRequest, Trip, UpdateTripRequest } from '../models';

@Injectable({ providedIn: 'root' })
export class TripService {
  private readonly base = `${environment.apiUrl}/Trip`;

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Trip[]>(this.base);
  }

  getById(id: string) {
    return this.http.get<Trip>(`${this.base}/${id}`);
  }

  create(dto: CreateTripRequest) {
    return this.http.post<Trip>(this.base, dto);
  }

  update(id: string, dto: UpdateTripRequest) {
    return this.http.put<Trip>(`${this.base}/${id}`, dto);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.base}/${id}`);
  }

  getCost(id: string) {
    return this.http.get<number>(`${this.base}/${id}/cost`);
  }

  getDuration(id: string) {
    return this.http.get<number>(`${this.base}/${id}/duration`);
  }

  isOverBudget(id: string) {
    return this.http.get<boolean>(`${this.base}/${id}/overbudget`);
  }
}
