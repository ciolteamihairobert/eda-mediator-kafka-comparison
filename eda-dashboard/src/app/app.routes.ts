import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell/shell.component';
import { HomeComponent } from './features/home/home.component';
import { LoginPanelComponent } from './core/auth/login-panel/login-panel.component';
import { DashboardUserComponent } from './pages/dashboard-user/dashboard-user.component';
import { DashboardAdminComponent } from './pages/dashboard-admin/dashboard-admin.component';
import { roleGuard } from './core/auth/role.guard';

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'login', component: LoginPanelComponent },
      {
        path: 'dashboard-user',
        component: DashboardUserComponent,
        canActivate: [roleGuard],
        data: { roles: ['user', 'admin'] },
      },
      {
        path: 'dashboard-admin',
        component: DashboardAdminComponent,
        canActivate: [roleGuard],
        data: { roles: ['admin'] },
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
