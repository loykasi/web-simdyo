<script setup lang="ts">
import type { ButtonProps } from '@nuxt/ui';

const links = ref<ButtonProps[]>([
  {
    label: 'Start Creating',
    to: '/create',
  }
])
</script>
<template>
    <UPageHero
        title="Create your game"
        description="Design, code, play with an intuitive visual editor. Shares with other around the world."
        :links="links"
    >
    </UPageHero>
</template>