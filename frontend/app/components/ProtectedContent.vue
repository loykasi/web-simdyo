<script setup lang="ts">
import { useAuthStore } from "~/stores/auth.store";

const prop = defineProps<{
  permissions: string[];
}>();

const { user } = useAuthStore();

const isPermitted = computed(() =>
  prop.permissions.every((p) => user.value?.permissions.includes(p)),
);
</script>
<template>
  <slot v-if="isPermitted" />
</template>
