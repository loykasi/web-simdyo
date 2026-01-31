import { useAuthStore } from "~/stores/auth.store";

export default defineNuxtRouteMiddleware((to, from) => {
  const { isLoggedIn } = useAuthStore();

  if (!isLoggedIn.value) {
    return navigateTo("/");
  }
});
