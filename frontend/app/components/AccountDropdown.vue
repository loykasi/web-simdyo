<script setup lang="ts">
import type { DropdownMenuItem } from '@nuxt/ui';
import { useAuthStore } from '~/stores/auth.store';

const { logout } = useLogin();
const { user } = useAuthStore();

const accountItems = ref<DropdownMenuItem[]>([
  {
    label: 'Profile',
    icon: 'material-symbols:person-outline',
    to: `/profile/${user.value?.username}`
  },
  {
    label: 'Settings',
    icon: 'material-symbols:settings-outline',
    to: '/account'
  },
  {
    label: 'Log out',
    icon: 'material-symbols:logout',
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
