import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Car } from '../../models/car.model';


@Injectable({
  providedIn: 'root'
})
export class FavoritesService {

  private http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7146/api/Favorites';

  addFavorite(carId: number, userId: number) {
    return this.http.post(`${this.apiUrl}/${carId}/?userId=${userId}`, {});
  }

  getFavorites(userId: number) {
    return this.http.get<Car[]>(`${this.apiUrl}/${userId}`);
  }

  removeFavorite(carId: number, userId: number) {
    return this.http.delete(`${this.apiUrl}/${carId}/?userId=${userId}`);
  }

}