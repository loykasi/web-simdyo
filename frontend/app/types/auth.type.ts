export interface RegisterRequest {
    username: string;
    email: string;
    password: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface ResetPasswordRequest {
    token: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export interface LoginResponse {
    userName: string;
    email: string;
    expiresAt: string;
}

export interface User {
    userName: string,
    email: string,
}