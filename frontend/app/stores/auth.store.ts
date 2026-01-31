import type { AuthUser } from "~/types/auth.type";

export const useAuthStore = () => {
  const user = useState<AuthUser | null>("userData", () => null);
  const expiresAt = useState<Date | null>("expiresAt", () => null);
  const refreshTokenTimeout = useState<NodeJS.Timeout | null>(
    "refreshTokenTimeout",
    () => null,
  );

  const isLoggedIn = computed(() => user.value !== null);

  function setExpiresAt(isoTime: string) {
    expiresAt.value = new Date(isoTime);
  }

  function startRefreshTokenTimer() {
    if (expiresAt.value == null) {
      console.error("Error on access token expiration");
      return;
    }

    const timeout = expiresAt.value.getTime() - Date.now() - 60 * 1000;
    refreshTokenTimeout.value = setTimeout(refreshToken, timeout);
  }

  function stopRefreshTokenTimer() {
    if (refreshTokenTimeout.value != null)
      clearTimeout(refreshTokenTimeout.value);
  }

  async function refreshToken() {
    await useAPI("auth/refresh", { method: "POST" });
    startRefreshTokenTimer();
  }

  function isPermitted(permissions: string[]) {
    return permissions.every((p) => user.value?.permissions.includes(p));
  }

  return {
    user,
    isLoggedIn,
    startRefreshTokenTimer,
    stopRefreshTokenTimer,
    setExpiresAt,
    isPermitted,
  };
};
