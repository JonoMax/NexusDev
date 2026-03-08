import { Routes } from '@angular/router';
import {Cars} from './pages/cars/cars';
import {Layout} from './layout/layout';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [

  // Login route (outside layout)
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login/login')
        .then(m => m.Login)
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./pages/register/register')
        .then(m => m.Register)
  },
  {
  path: 'favorites',
  loadComponent: () =>
    import('./pages/favorites/favorites')
      .then(m => m.Favorites)
},
  // Layout wrapper
  {
    path: '',
    component: Layout,
    children: [
      { path: 'cars', component: Cars },

      {
        path: 'manage-cars',
        loadComponent: () =>
          import('./pages/manage-cars/manage-cars')
            .then(m => m.ManageCars),
        canActivate: [authGuard],
        data: { role: 'admin' }
      },

      { path: 'terms', component: Cars },

      { path: '', redirectTo: 'cars', pathMatch: 'full' },

    ]
  }
];