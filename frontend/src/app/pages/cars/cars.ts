import { Component, inject, OnInit } from '@angular/core';
import { CarService } from '../../services/car.service';
import { Car } from '../../models/car.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-cars',
  imports: [FormsModule],
  templateUrl: './cars.html',
  styleUrl: './cars.scss',
})
export class Cars implements OnInit{
  private carService =  inject(CarService);

  // Holds list of cars retrieved from the backend
  cars: Car[] = [];

  // Object bound to the "Add Car" from inputs
  // Represents a new car before it is sent to the API
  newCar: Car = {
    id: 0,
    make: '',
    model: '',
    year: new Date().getFullYear()
  }

  Id: number = 0;
  
  //Holds a temp copy of a car while it is being edited
  // If null, no car is currently in edit mode
  editingCar: Car | null = null;

  ngOnInit(): void {
    // subscribe is the verb
    // “Go do this work. When the result arrives, call me and I’ll handle it.”
      this.carService.getCars().subscribe(cars => { 
      console.log(cars);
      this.cars = cars;
    });
  }

  // Sends a new car to the backend and immediately updates the UI
  addCar(){
    this.carService.addCar(this.newCar).subscribe(car => {
      this.cars.push(car);
      this.newCar = {
        id: 0,
        make: '',
        model: '',
        year: new Date().getFullYear()
      };
    });
  }

  deleteCar(id: number){
    this.carService.deleteCar(id).subscribe(() => {
      this.cars = this.cars.filter(c => c.id !== id)
    })
  }

  // 
  editCar(car: Car){
    // Clone the car to avoid mutating the list while the user edits
    this.editingCar = { ...car};
    console.log("Edit Car");
  }

  saveCar(){
    if (!this.editingCar) return;
    this.carService.updateCar(this.editingCar).subscribe(() => {

      // Find the index of the car being edited in the local list
      const index = this.cars.findIndex(c => c.id === this.editingCar!.id);

      // Replace the old car with the updated version
      this.cars[index] = this.editingCar!;

      // Exit edit mode and reset the temporary state
      this.editingCar = null;
    })
  }
}