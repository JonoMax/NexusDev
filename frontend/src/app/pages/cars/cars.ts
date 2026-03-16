import { Component, computed, inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CarService } from '../../services/car.service';
import { Car } from '../../models/car.model';
import { FormsModule } from '@angular/forms';
import { signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FavoritesService } from '../../services/favorites/favorites';

@Component({
  selector: 'app-cars',
  imports: [FormsModule],
  templateUrl: './cars.html',
  styleUrl: './cars.scss',
})
export class Cars implements OnInit {
  private carService = inject(CarService);

  // Holds list of cars retrieved from the backend
  cars = signal<Car[]>([]);

  // Indicates whether data is currently being loaded from the API
  isLoading = signal<boolean>(false);

  // Holds any error message from API calls
  errorMessage = signal<string>('');

  filterMake = signal('');
  filterYear = signal<number | null>(null);

  filteredCars = computed(() => {
    const result = this.cars().filter((car) => {
      const matchesMake =
        !this.filterMake() ||
        car.make.toLowerCase().includes(this.filterMake().toLowerCase());
      const matchesYear = !this.filterYear() || car.year === this.filterYear();
      return matchesMake && matchesYear;
    });
    if (this.currentPage() > Math.ceil(result.length / this.pageSize)) {
      this.currentPage.set(1);
    }
    return result;
  });

  currentPage = signal(1);
  // show up to 12 cards per page
  pageSize = 12;

  totalPages = computed(() => {
    return Math.ceil(this.filteredCars().length / this.pageSize);
  });

  pagedCars = computed(() => {
    const start = (this.currentPage() - 1) * this.pageSize;
    const end = start + this.pageSize;

    return this.filteredCars().slice(start, end);
  });

  Id: number = 0;

  //Holds a temp copy of a car while it is being edited
  // If null, no car is currently in edit mode
  editingCar: Car | null = null;
  
  @ViewChild('editSection') editSection?: ElementRef;
  private http = inject(HttpClient);
  private router = inject(Router);
  auth = inject(AuthService);
  private favoritesService = inject(FavoritesService);
  favoriteIds = new Set<number>();

  ngOnInit() {
    //start loading
    this.isLoading.set(true);
    //load cars
    this.carService.getCars().subscribe({
      next: (cars) => {
        this.cars.set(cars);
        this.isLoading.set(false);
      },
      error: (error) => {
        this.errorMessage.set(error.message);
        this.isLoading.set(false);
      },
    });
    // load favorites if logged in
    const user = this.auth.user();

    if (!user) return;

    this.favoritesService.getFavorites(user.id).subscribe((favorites) => {
      this.favoriteIds = new Set(favorites.map((c) => c.id));
    });
  }

  deleteCar(id: number) {
    this.carService.deleteCar(id).subscribe(() => {
      this.cars.update((c) => c.filter((car) => car.id !== id));
    });
  }

  editCar(car: Car) {
    // Clone the car to avoid mutating the list while the user edits
    this.editingCar = { ...car };
    console.log('Edit Car');
    
    // Scroll to the edit form at the bottom
    setTimeout(() => {
      this.editSection?.nativeElement.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }, 100);
  }

  saveCar() {
    if (!this.editingCar) return;
    this.carService.updateCar(this.editingCar).subscribe(() => {
      this.cars.update((cars) =>
        cars.map((c) => (c.id === this.editingCar!.id ? this.editingCar! : c)),
      );

      // Exit edit mode and reset the temporary state
      this.editingCar = null;
    });
  }

  toggleFavorite(carId: number) {
    const user = this.auth.user();

    if (!user) {
      this.router.navigate(['/login']);
      return;
    }

    if (this.favoriteIds.has(carId)) {
      this.favoritesService.removeFavorite(carId, user.id).subscribe(() => {
        this.favoriteIds.delete(carId);
        this.favoritesService.favoriteCount.update(c => c - 1);
      });
    } else {
      this.favoritesService.addFavorite(carId, user.id).subscribe(() => {
        this.favoriteIds.add(carId);
        this.favoritesService.favoriteCount.update(c => c + 1);
      });
    }
  }

  nextPage() {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((p) => p + 1);
    }
  }

  previousPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update((p) => p - 1);
    }
  }
}
