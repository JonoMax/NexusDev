import { Component, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { Car } from '../../models/car.model';
import { FavoritesService } from '../../services/favorites/favorites';

@Component({
  selector: 'favorites',
  imports: [],
  templateUrl: './favorites.html',
  styleUrl: './favorites.scss',
})
export class Favorites {
  private auth = inject(AuthService);
  private favoritesService = inject(FavoritesService);

  cars = signal<Car[]>([]);

  ngOnInit() {
    const user = this.auth.user();

    if (!user) return;

    this.favoritesService
      .getFavorites(user.id)
      .subscribe(cars => this.cars.set(cars));
  } 
}
