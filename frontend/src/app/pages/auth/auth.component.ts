import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { IconComponent } from '../../shared/icon/icon.component';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [FormsModule, IconComponent],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  mode = signal<'login' | 'register'>('login');
  error = signal<string | null>(null);
  loading = signal(false);

  fullName = '';
  email = '';
  password = '';

  setMode(mode: 'login' | 'register'): void {
    this.mode.set(mode);
    this.error.set(null);
  }

  submit(): void {
    this.error.set(null);
    this.loading.set(true);

    const request$ = this.mode() === 'login'
      ? this.auth.login({ email: this.email, password: this.password })
      : this.auth.register({ fullName: this.fullName, email: this.email, password: this.password });

    request$.subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/trips']);
      },
      error: (err) => {
        this.loading.set(false);
        this.error.set(err.error?.message || err.error || 'Something went wrong. Please try again.');
      }
    });
  }
}
