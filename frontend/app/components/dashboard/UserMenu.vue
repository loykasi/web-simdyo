<script setup lang="ts">
import type { DropdownMenuItem } from "@nuxt/ui";
import { useAuthStore } from "~/stores/auth.store";

defineProps<{
  collapsed?: boolean;
}>();

const colorMode = useColorMode();
const { logout } = useLogin();
const { user } = useAuthStore();

const accountItems = computed<DropdownMenuItem[]>(() => [
  {
    label: "Profile",
    icon: "material-symbols:person-outline",
    to: `/profile/${user.value?.username}`,
  },
  {
    label: "Settings",
    icon: "material-symbols:settings-outline",
    to: "/account",
  },
  {
    label: "Logout",
    icon: "material-symbols:logout",
    onSelect() {
      logout();
    },
  },
  {
    label: "Appearance",
    icon: "material-symbols:lightbulb-2-outline",
    children: [
      {
        label: "Light",
        icon: "material-symbols:sunny-outline",
        type: "checkbox",
        checked: colorMode.value === "light",
        onSelect(e: Event) {
          e.preventDefault();

          colorMode.preference = "light";
        },
      },
      {
        label: "Dark",
        icon: "material-symbols:nightlight-outline",
        type: "checkbox",
        checked: colorMode.value === "dark",
        onUpdateChecked(checked: boolean) {
          if (checked) {
            colorMode.preference = "dark";
          }
        },
        onSelect(e: Event) {
          e.preventDefault();
        },
      },
    ],
  },
]);
</script>
<template>
  <UDropdownMenu
    :items="accountItems"
    :content="{
      align: 'center',
    }"
    :ui="{
      content: collapsed ? 'w-48' : 'w-(--reka-dropdown-menu-trigger-width)',
    }"
  >
    <UButton
      :label="user?.username"
      color="neutral"
      variant="ghost"
      class="w-full"
      block
      trailing-icon="material-symbols:menu-rounded"
    />
  </UDropdownMenu>
</template>
