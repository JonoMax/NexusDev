import { Routes } from '@angular/router';
import {Cars} from './pages/cars/cars';
import {Layout as LayoutComponent} from './layout/layout';

export const routes: Routes = [
    {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'cars', component: Cars },
      { path: '', redirectTo: 'cars', pathMatch: 'full' },
      { path: 'admin', component: Cars }, // temp reuse
      { path: 'terms', component: Cars }  // temp reuse
    ]
  }
];