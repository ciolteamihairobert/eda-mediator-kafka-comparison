export type Role = 'admin' | 'user';

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  expiresAtUtc: string;
  username: string;
  role: Role;
}
