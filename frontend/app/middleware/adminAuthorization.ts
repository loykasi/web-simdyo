import { useAuthStore } from "~/stores/auth.store"

export default defineNuxtRouteMiddleware((to, from) => {
    const { isLoggedIn, user, isPermitted } = useAuthStore();

    if (!isLoggedIn.value || !isPermitted(["dashboard_access"])) {
        return navigateTo('/');
    }

    if (user.value?.isUseOTP) {
        return navigateTo(`/login?redirect=${encodeURIComponent("/dashboard")}`);
    }
})