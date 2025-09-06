export interface RegisterRequest {
    username: string;
    email: string;
    password: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    userName: string;
    email: string;
    expiresAt: string;
}

export interface User {
    username: string,
    email: string,
}