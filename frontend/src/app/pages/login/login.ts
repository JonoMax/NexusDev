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
  role = signal<'user' | 'admin'>('user');

  login() {
    if(!this.username) return;

    this.auth.login(this.username, this.role());
    this.router.navigate(['/cars']);  
  }
}
