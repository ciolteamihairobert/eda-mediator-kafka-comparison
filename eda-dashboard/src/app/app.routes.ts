import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell/shell.component';
import { HomeComponent } from './features/home/home.component';

const DummyLoginComponent = HomeComponent;

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', component: HomeComponent },
      { path: 'login', component: DummyLoginComponent },
    ]
  },
  { path: '**', redirectTo: '' }
];
