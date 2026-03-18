import { Component, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';
import { Car } from '../../models/car.model';
import { FavoritesService } from '../../services/favorites/favorites';
import { RouterLinkActive, RouterLink } from "@angular/router";

@Component({
  selector: 'favorites',
  imports: [RouterLinkActive, RouterLink],
  templateUrl: './favorites.html',
  styleUrl: './favorites.scss',
})
export class Favorites {

  private auth = inject(AuthService);
  private favoritesService = inject(FavoritesService);
  favoriteCount = signal(0);

  cars = signal<Car[]>([]);

  ngOnInit() {
    const user = this.auth.user();

    if (!user) return;

    this.favoritesService
      .getFavorites(user.id)
      .subscribe(cars => { this.cars.set(cars); 
        this.favoriteCount.set(cars.length);
      });
  } 

  removeFavorite(carId: number) {

  const user = this.auth.user();

  if (!user) return;

  this.favoritesService.removeFavorite(carId, user.id)
    .subscribe(() => {

      // remove car from UI instantly
      this.cars.update(cars => cars.filter(c => c.id !== carId));
      this.favoritesService.favoriteCount.update(c => c - 1);

    });

  }
}
