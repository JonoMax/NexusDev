import { Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.scss',
})
export class Login {
  private auth = inject(AuthService);
  private router = inject(Router);
  username = '';
  password = '';
  role = signal<'user' | 'admin'>('user');

  login(){
    this.auth.login(this.username, this.password).subscribe({
      next: (user) => {
        this.auth.setUser(user);
        this.router.navigate(['/cars']);
      },
      error: (err) => {
        alert('Login failed. Please check your credentials and try again.');
      }
    });
  }

  register(){
    this.router.navigate(['/register']);
  }
}
