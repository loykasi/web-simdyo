<script setup lang="ts">
const prop = defineProps<{
    title: string;
    description: string;
}>();

const emit = defineEmits<{
    close: [boolean],
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
                <UButton color="neutral" label="No" @click="emit('close', false)" class="w-full flex justify-center items-center" />
                <UButton label="Yes" @click="emit('close', true)" class="w-full flex justify-center items-center" />
            </div>
        </template>
    </UModal>
</template>