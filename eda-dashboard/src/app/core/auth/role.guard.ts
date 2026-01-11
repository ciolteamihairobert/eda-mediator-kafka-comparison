import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

export const roleGuard: CanActivateFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (!auth.isLoggedIn) {
    router.navigateByUrl('/');
    return false;
  }

  const allowed: string[] = route.data?.['roles'] ?? [];
  if (allowed.length === 0) return true;

  const role = auth.role;
  if (role && allowed.includes(role)) return true;

  router.navigateByUrl(
    role === 'admin' ? '/dashboard-admin' : '/dashboard-user'
  );
  return false;
};
