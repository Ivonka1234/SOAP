import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models';

const TOKEN_KEY = 'soap_token';
const EMAIL_KEY = 'soap_email';
const ROLE_KEY = 'soap_role';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly token = signal<string | null>(localStorage.getItem(TOKEN_KEY));
  private readonly email = signal<string | null>(localStorage.getItem(EMAIL_KEY));
  private readonly role = signal<string | null>(localStorage.getItem(ROLE_KEY));

  readonly isLoggedIn = computed(() => !!this.token());
  readonly currentEmail = computed(() => this.email());
  readonly currentRole = computed(() => this.role());

  constructor(private http: HttpClient) {}

  register(dto: RegisterRequest) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/Auth/register`, dto).pipe(
      tap(res => this.setSession(res))
    );
  }

  login(dto: LoginRequest) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/Auth/login`, dto).pipe(
      tap(res => this.setSession(res))
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(EMAIL_KEY);
    localStorage.removeItem(ROLE_KEY);
    this.token.set(null);
    this.email.set(null);
    this.role.set(null);
  }

  getToken(): string | null {
    return this.token();
  }

  private setSession(res: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, res.token);
    localStorage.setItem(EMAIL_KEY, res.email);
    localStorage.setItem(ROLE_KEY, res.role);
    this.token.set(res.token);
    this.email.set(res.email);
    this.role.set(res.role);
  }
}
