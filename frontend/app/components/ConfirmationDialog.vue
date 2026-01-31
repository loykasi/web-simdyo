<script setup lang="ts">
defineProps<{
  title: string;
  description: string;
}>();

const emit = defineEmits<{
  close: [boolean];
}>();
</script>

<template>
  <UModal
    :title="title"
    :description="description"
    :close="{ onClick: () => emit('close', false) }"
  >
    <template #body>
      <div class="flex gap-2">
        <UButton
          color="neutral"
          label="No"
          class="w-full flex justify-center items-center"
          @click="emit('close', false)"
        />
        <UButton
          label="Yes"
          class="w-full flex justify-center items-center"
          @click="emit('close', true)"
        />
      </div>
    </template>
  </UModal>
</template>
