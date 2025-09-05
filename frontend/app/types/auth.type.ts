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
    username: string;
    email: string
}

export interface User {
    username: string,
    email: string,
}