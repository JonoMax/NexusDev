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
  private $carSub: any;

  cars: Car[] = [];

  newCar: Car = {
    id: 0,
    make: '',
    model: '',
    year: new Date().getFullYear()
  }

  ngOnInit(): void {
      this.carService.getCars().subscribe(cars => { 
      console.log(cars);
      this.cars = cars;
    });
  }

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

}