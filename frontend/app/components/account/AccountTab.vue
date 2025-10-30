<script setup lang="ts">
import { useAuthStore } from '~/stores/auth.store';

const { user } = useAuthStore();
</script>
<template>
    <UCard>
        <div class="flex flex-col gap-4">
            <div>
                <label class="block font-medium text-default">Username</label>
                <div class="text-default">{{ user?.userName }}</div>
            </div>
            <div>
                <label class="block font-medium text-default">Email</label>
                <div class="text-default">{{ user?.email }}</div>
            </div>
        </div>
    </UCard>
</template>