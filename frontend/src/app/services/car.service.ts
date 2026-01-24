import {inject, Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http'
import { Observable } from 'rxjs';
import {Car} from '../models/car.model'

@Injectable({
  providedIn: 'root',
})
export class CarService {
  private http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7146/api/Car'

  getCars(){
    console.log("What the actual fuck?")
    return this.http.get<Car[]>(this.apiUrl);
  }

  addCar(car: Car) {
    return this.http.post<Car>(this.apiUrl, car);
  }
  
}
 