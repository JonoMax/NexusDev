import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const requiredRole = route.data?.['role'];

  if (!auth.isLoggedIn()) {
    return router.createUrlTree(['/login']);
  }

  if (requiredRole && !auth.isAdmin()) {
    return router.createUrlTree(['/cars']);
  }

  return true;
};