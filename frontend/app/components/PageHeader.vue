<script setup lang="ts">
import { UColorModeButton } from "#components";
import type { NavigationMenuItem } from "@nuxt/ui";
import { useAuthStore } from "~/stores/auth.store";
import { en, vi } from "@nuxt/ui/locale";

const { isLoggedIn, isPermitted } = useAuthStore();
const { locale, setLocale } = useI18n();

const items = computed<NavigationMenuItem[]>(() => [
  {
    label: $t("nav.create"),
    to: "/create",
  },
  {
    label: $t("nav.explore"),
    to: "/explore",
  },
  ...(isLoggedIn.value
    ? [
        {
          label: $t("nav.upload"),
          to: "/upload",
        },
      ]
    : []),
  ...(isPermitted(["dashboard_access"])
    ? [
        {
          label: "Dashboard",
          to: "/dashboard/users",
        },
      ]
    : []),
]);
</script>
<template>
  <UHeader>
    <template #left>
      <NuxtLink
        to="/"
        class="focus-visible:outline-primary hover:text-default transition-colors shrink-0 font-bold text-xl text-highlighted flex items-end gap-1.5 me-4"
      >
        Simdyo
      </NuxtLink>
      <UNavigationMenu :items="items" variant="pill" />
    </template>

    <template #right>
      <ClientOnly>
        <ULocaleSelect
          :model-value="locale"
          :locales="[en, vi]"
          @update:model-value="setLocale($event)"
        />
        <UColorModeButton />
        <template v-if="isLoggedIn">
          <AccountDropdown />
        </template>
        <template v-else>
          <LoginPopover />

          <UButton to="/register" variant="subtle">
            {{ $t("nav.register") }}
          </UButton>
        </template>
      </ClientOnly>
    </template>
  </UHeader>
</template>
