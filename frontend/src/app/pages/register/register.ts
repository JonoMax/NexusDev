import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';  
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {

  private auth = inject(AuthService);
  private router = inject(Router);
  username = '';
  password = '';
  register() {
    this.auth.register(this.username, this.password).subscribe(() => {
      this.router.navigate(['/login']);
    });
  }
}
