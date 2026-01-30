export interface RegisterRequest {
    username: string;
    email: string;
}

export interface LoginOTPRequest {
    email: string;
    code: string;
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
    username: string;
    email: string;
    expiresAt: string;
    isUseOTP: boolean;
    permissions: string[]
}

export interface AuthUser {
    username: string,
    email: string,
    isUseOTP: boolean,
    permissions: string[]
}