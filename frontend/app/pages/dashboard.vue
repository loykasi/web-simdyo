<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'
import UserMenu from '~/components/dashboard/UserMenu.vue';

const items: NavigationMenuItem[] = [{
    label: 'Users',
    icon: 'material-symbols:person-outline',
    to: '/dashboard/users'
}, {
    label: 'Projects',
    icon: 'material-symbols:view-object-track-outline',
    to: '/dashboard/projects'
}, {
    label: 'Settings',
    icon: 'material-symbols:settings-outline',
    to: '/account'
}]

definePageMeta({
  layout: 'admin',
});
</script>

<template>
  <UDashboardGroup>
    <UDashboardSidebar collapsible resizable :ui="{ footer: 'border-t border-default' }">
      <template #header="{ collapsed }">
        <NuxtLink
          to="/"
          class="p-2.5 focus-visible:outline-primary hover:text-default transition-colors shrink-0 font-bold text-xl text-highlighted flex items-end gap-1.5 me-4"
        >
          CodeVisdoo
        </NuxtLink>
      </template>
      
      <template #default="{ collapsed }">
        <UNavigationMenu
            :collapsed="collapsed"
            :items="items"
            orientation="vertical"
        />
      </template>

      <template #footer="{ collapsed }">
        <!-- <UColorModeButton /> -->
        <UserMenu :collapsed="collapsed" />
      </template>
    </UDashboardSidebar>

    <NuxtPage></NuxtPage>
  </UDashboardGroup>
</template>

