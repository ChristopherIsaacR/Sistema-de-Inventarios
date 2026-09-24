import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  username = '';
  password = '';

  error = '';
  cargando = false;

  login(): void {

    if (!this.username || !this.password) {
      this.error = 'Ingresa usuario y contraseña.';
      return;
    }

    this.error = '';
    this.cargando = true;

    this.authService.login({
      nombreUsuario: this.username,
      password: this.password
    }).subscribe({

      next: () => {
        this.cargando = false;
        this.router.navigate(['/dashboard']);
      },

      error: (error) => {
        console.error('Error de login:', error);

        this.cargando = false;
        this.error = 'Usuario o contraseña incorrectos.';
      }

    });
  }
}