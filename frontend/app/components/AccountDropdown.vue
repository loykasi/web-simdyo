<script setup lang="ts">
import type { DropdownMenuItem } from '@nuxt/ui';
import { useAuthStore } from '~/stores/auth.store';

const { logout } = useLogin();
const { user } = useAuthStore();

const accountItems = ref<DropdownMenuItem[]>([
  {
    label: 'Profile',
    icon: 'i-lucide-user',
    to: `/profile/${user.value?.username}`
  },
  {
    label: 'Settings',
    icon: 'i-lucide-cog',
    to: '/account'
  },
  {
    label: 'Logout',
    icon: 'i-lucide-credit-card',
    onSelect(e) {
        logout();
    },
  },
])
</script>

<template>
  <UDropdownMenu :items="accountItems">
    <UButton
      color="neutral"
      variant="subtle"
    >
      {{user?.username}}
    </UButton>
  </UDropdownMenu>
</template>
