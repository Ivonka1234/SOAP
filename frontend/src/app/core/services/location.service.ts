import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CreateLocationRequest, Location } from '../models';

@Injectable({ providedIn: 'root' })
export class LocationService {
  private readonly base = `${environment.apiUrl}/Location`;

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<Location[]>(this.base);
  }

  getById(id: string) {
    return this.http.get<Location>(`${this.base}/${id}`);
  }

  create(dto: CreateLocationRequest) {
    return this.http.post<Location>(this.base, dto);
  }

  update(id: string, dto: CreateLocationRequest) {
    return this.http.put<Location>(`${this.base}/${id}`, dto);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
