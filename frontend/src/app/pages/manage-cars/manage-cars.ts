import { Component, inject } from '@angular/core';
import { Car } from '../../models/car.model';
import { CarService } from '../../services/car.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-manage-cars',
  imports: [FormsModule],
  templateUrl: './manage-cars.html',
  styleUrl: './manage-cars.scss',
})
export class ManageCars {
   private carService = inject(CarService);
   // Object bound to the "Add Car" from inputs
    // Represents a new car before it is sent to the API
    newCar: Car = {
      id: 0,
      make: '',
      model: '',
      year: new Date().getFullYear(),
      price: 0
    }

    // Sends a new car to the backend and immediately updates the UI
  addCar(){
    this.carService.addCar(this.newCar).subscribe(car => {
      // this.cars.update(cars => [...cars, car]);
      this.newCar = {
        id: 0,
        make: '',
        model: '',
        year: new Date().getFullYear(),
        price: 0
      };
    });
  }
  

}
