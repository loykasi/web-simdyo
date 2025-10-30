<script setup lang="ts">
import type { NavigationMenuItem } from '@nuxt/ui'

const items: NavigationMenuItem[] = [
  {
    label: 'Privacy',
    to: '/#',
    target: '_blank'
  },
  {
    label: 'Terms',
    to: '/#',
    target: '_blank'
  },
  {
    label: 'Feedback',
    to: '/#',
    target: '_blank'
  }
]
</script>

<template>
  <UFooter>
    <template #left>
        <p class="text-muted text-sm">v0.1</p>
    </template>

    <UNavigationMenu :items="items" variant="link" />

    <template #right>
        <p class="text-muted text-sm">Copyright © {{ new Date().getFullYear() }}</p>
    </template>
  </UFooter>
</template>
