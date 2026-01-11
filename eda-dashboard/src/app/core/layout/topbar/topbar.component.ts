import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthUiService } from '../../auth/auth-ui.service';
import { AuthService } from '../../auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [RouterLink, CommonModule, RouterLinkActive],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss',
})
export class TopbarComponent {
  constructor(
    public auth: AuthService,
    private ui: AuthUiService,
    private router: Router
  ) {}

  public openLogin() {
    this.ui.open();
  }

  public logout() {
    this.auth.logout();
    this.router.navigateByUrl('/');
  }
}
