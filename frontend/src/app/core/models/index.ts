export interface AuthResponse {
  token: string;
  email: string;
  role: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface Trip {
  id: string;
  name: string;
  budget: number;
  startDate: string;
  endDate: string;
  totalEstimatedCost: number;
  locations?: TripLocation[];
}

export interface CreateTripRequest {
  name: string;
  budget: number;
  startDate: string;
  endDate: string;
}

export interface UpdateTripRequest {
  name: string;
  budget: number;
  startDate: string;
  endDate: string;
}

export interface Location {
  id: string;
  name: string;
  country: string;
  estimatedCost: number;
  visitDurationHours: number;
  priority: number;
}

export interface CreateLocationRequest {
  name: string;
  country: string;
  estimatedCost: number;
  visitDurationHours: number;
  priority: number;
}

export interface TripLocation {
  locationId: string;
  locationName: string;
  country: string;
  order: number;
  scheduledStartTime: string;
  visitDurationHours: number;
  estimatedCost: number;
}

export interface AddLocationToTripRequest {
  locationId: string;
}

export type Itinerary = Record<string, TripLocation[]>;
