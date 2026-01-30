import { useAuthStore } from "~/stores/auth.store";
import type { LoginRequest, LoginResponse, AuthUser, LoginOTPRequest } from "~/types/auth.type";

export function useLogin() {
  async function sendOTP(email: string) {
    return useAPI(`auth/send-otp?email=${email}`, {
      method: "POST",
    });
  }

  async function loginOTP(request: LoginOTPRequest) {
    return useAPI<LoginResponse>("auth/login-otp", {
      method: "POST",
      body: request
    });
  }

  async function login(request: LoginRequest) {
    return useAPI<LoginResponse>("auth/login", {
      method: "POST",
      body: request
    });
  }

  async function fetchProfile() {
    const { user } = useAuthStore();
    const headers = useRequestHeaders(['cookie']); 

    useAPI<AuthUser>("auth/profile", {
      method: "GET",
      headers: headers
    }).then(res => {
      user.value = {
          email: res.email,
          username: res.username
      } as AuthUser;
      console.log("Fetch profile successful");
    }).catch(err => {
      console.log("Fetch profile failed")
    });
  }

  async function logout() {
    const { user } = useAuthStore();
    const isLogged = useCookie("isLogged", {
      default: () => false
    });

    useAPI("auth/logout", {
      method: "POST"
    });
    user.value = null;

    isLogged.value = false;
    navigateTo('/');
  }

  return {
    sendOTP,
    loginOTP,
    login,
    fetchProfile,
    logout
  }
}