import { Routes } from '@angular/router';
import {Cars} from './pages/cars/cars';

export const routes: Routes = [
    {
    path: '',
    redirectTo: 'cars',
    pathMatch: 'full'
  },
  {
    path: 'cars',
    component: Cars
  }
];
