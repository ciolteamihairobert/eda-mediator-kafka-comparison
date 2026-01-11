import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { AuthUiService } from '../auth-ui.service';
import { LoginRequest } from '../auth.models';

@Component({
  selector: 'app-login-panel',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login-panel.component.html',
  styleUrl: './login-panel.component.scss',
})
export class LoginPanelComponent {
  public loading = false;
  public error: string | null = null;

  public isOpen$ = this.ui.isOpen$;

  public form = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required],
  });

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    public ui: AuthUiService,
    private router: Router
  ) {}

  public close() {
    this.error = null;
    this.ui.close();
  }

  public submit() {
    this.error = null;
    if (this.form.invalid) return;

    this.loading = true;

    this.auth.login(this.form.value as LoginRequest).subscribe({
      next: (response) => {
        this.loading = false;
        this.close();
        this.router.navigateByUrl(
          response.role === 'admin' ? '/dashboard-admin' : '/dashboard-user'
        );
      },
      error: (err) => {
        this.loading = false;
        this.error = err?.error?.message ?? 'Login failed. Check credentials.';
      },
    });
  }

  @HostListener('document:keydown.escape')
  public onEsc() {
    if (this.ui.isOpen) this.close();
  }
}
