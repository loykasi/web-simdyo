<script setup lang="ts">
const open = ref(false);

function toggle() {
  open.value = !open.value;
}
</script>
<template>
  <UPopover
    v-model:open="open"
    :content="{
      align: 'end',
    }"
    arrow
  >
    <template #anchor>
      <UChip :show="true">
        <UButton
          icon="material-symbols:notifications-outline"
          color="neutral"
          variant="ghost"
          @click="toggle"
        />
      </UChip>
    </template>

    <template #content>
      <div class="size-48 m-4 inline-flex" />
    </template>
  </UPopover>
</template>
