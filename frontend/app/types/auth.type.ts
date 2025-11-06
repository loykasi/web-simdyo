export interface RegisterRequest {
    username: string;
    email: string;
    password: string;
}

export interface LoginRequest {
    username: string;
    password: string;
}

export interface ResetPasswordRequest {
    token: string;
    email: string;
    password: string;
    confirmPassword: string;
}

export interface LoginResponse {
    username: string;
    email: string;
    expiresAt: string;
}

export interface User {
    username: string,
    email: string,
}