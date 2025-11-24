<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'
import UserMenu from '~/components/dashboard/UserMenu.vue';
import { useAuthStore } from '~/stores/auth.store';

const { isPermitted } = useAuthStore();

const menu = [{
  label: 'Users',
  icon: 'material-symbols:person-outline',
  to: '/dashboard/users',
  permissions: ['manage_users']
}, {
  label: 'Projects',
  icon: 'material-symbols:view-object-track-outline',
  to: '/dashboard/projects',
  permissions: ['manage_projects']
}, {
  label: 'Reports',
  icon: 'material-symbols:report-outline-rounded',
  to: '/dashboard/project-reports',
  permissions: ['manage_project_reports']
}, {
  label: 'Categories',
  icon: 'material-symbols:category-outline-rounded',
  to: '/dashboard/project-categories',
  permissions: ['manage_categories']
}, {
  label: 'Settings',
  icon: 'material-symbols:settings-outline',
  to: '/account'
}]

const items: NavigationMenuItem[] = menu
    .filter(item => !item.permissions || isPermitted(item.permissions))
    .map(item => { return {
          label: item.label,
          icon: item.icon,
          to: item.to
      } as NavigationMenuItem
    })

// const items: NavigationMenuItem[] = [
// ...(isPermitted(["manage_users"]) ? [ 
//   {
//     label: 'Users',
//     icon: 'material-symbols:person-outline',
//     to: '/dashboard/users'
//   }
// ] : []),
// ...(isPermitted(["manage_projects"]) ? [ 
//   {
//     label: 'Projects',
//     icon: 'material-symbols:view-object-track-outline',
//     to: '/dashboard/projects'
//   }
// ] : []), {
//     label: 'Reports',
//     icon: 'material-symbols:report-outline-rounded',
//     to: '/dashboard/project-reports'
// }, {
//     label: 'Categories',
//     icon: 'material-symbols:category-outline-rounded',
//     to: '/dashboard/project-categories'
// }, {
//     label: 'Settings',
//     icon: 'material-symbols:settings-outline',
//     to: '/account'
// }]

definePageMeta({
  layout: 'admin',
  middleware: ['admin-authorization']
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

