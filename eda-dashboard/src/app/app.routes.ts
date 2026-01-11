import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell/shell.component';
import { HomeComponent } from './features/home/home.component';
import { DashboardUserComponent } from './pages/dashboard-user/dashboard-user.component';

const DummyLoginComponent = HomeComponent;

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'login', component: DummyLoginComponent },
      { path: 'dashboard-user', component: DashboardUserComponent }
    ]
  },
  { path: '**', redirectTo: '' }
];
