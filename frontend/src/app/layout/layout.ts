import { Component, inject } from '@angular/core';
import { Router, RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { FavoritesService } from '../services/favorites/favorites';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class Layout {
  auth = inject(AuthService);
  router = inject(Router);
  favoritesService = inject(FavoritesService);

  logout() {
    this.auth.logout();
    this.router.navigate(['/cars']);
  }
}
