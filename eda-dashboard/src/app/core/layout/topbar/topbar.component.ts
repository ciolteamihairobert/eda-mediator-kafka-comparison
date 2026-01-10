import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthUiService } from '../../auth/auth-ui.service';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss'
})
export class TopbarComponent {

  constructor(
    private auth: AuthService,
    private ui: AuthUiService,
    private router: Router) {}

  public openLogin() {
    this.ui.open();
  }
}
