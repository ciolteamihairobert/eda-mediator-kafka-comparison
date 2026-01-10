import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { LoginRequest, LoginResponse, Role } from './auth.models';

const STORAGE_KEY = 'eda_auth';

export interface AuthState {
  accessToken: string;
  expiresAtUtc: string;
  username: string;
  role: Role;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private stateSubject = new BehaviorSubject<AuthState | null>(this.read());
  public state$ = this.stateSubject.asObservable();

  constructor(private http: HttpClient) {}

  get state(): AuthState | null { return this.stateSubject.value; }
  get isLoggedIn(): boolean { return !!this.state && !this.isExpired(this.state); }
  get role(): Role | null { return this.state?.role ?? null; }

  public login(req: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/api/auth/login', req).pipe(
      tap(res => {
        const state: AuthState = {
          accessToken: res.accessToken,
          expiresAtUtc: res.expiresAtUtc,
          username: res.username,
          role: res.role
        };
        localStorage.setItem(STORAGE_KEY, JSON.stringify(state));
        this.stateSubject.next(state);
      })
    );
  }

  public logout() {
    localStorage.removeItem(STORAGE_KEY);
    this.stateSubject.next(null);
  }

  public getAccessToken(): string | null {
    if (!this.isLoggedIn) return null;
    return this.state!.accessToken;
  }

  private read(): AuthState | null {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return null;
    try { return JSON.parse(raw) as AuthState; } catch { return null; }
  }

  private isExpired(state: AuthState): boolean {
    const exp = Date.parse(state.expiresAtUtc);
    return Number.isFinite(exp) ? Date.now() >= exp : true;
  }
}
