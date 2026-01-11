<script setup lang="ts">
import type { ProjectResponse } from '~/types/project.type';

defineProps<{
    project: ProjectResponse,
}>();
</script>
<template>
    <div
        class="rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
    >
        <NuxtLink
            :to="`/projects/${project.publicId}`"
            class="block aspect-square w-full"
        >
            <!-- <img alt="placeholder" src="https://placehold.co/400" class="aspect-square" /> -->
            <!-- <img alt="placeholder" :src="`/thumbnails/${project.publicId}.png`" class="aspect-square" /> -->
            <img :alt="project.publicId" :src="project.thumbnailLink" />
        </NuxtLink>

        <div class="p-2">
            <NuxtLink
                :to="`/projects/${project.publicId}`"
                class="text-base text-primary line-clamp-1 font-bold hover:underline"
            >
                {{ project.title }}
            </NuxtLink>
            <NuxtLink
                :to="`/profile/${project.username}`"
                class="mt-1 text-sm text-secondary hover:underline"
            >
                {{ project.username }}
            </NuxtLink>
            <div class="">
                <div class="flex items-center gap-2 text-muted">
                    <UIcon name="material-symbols:favorite-outline" class="size-4" />
                    <span>{{ project.likeCount }}</span>
                </div>
            </div>
        </div>
    </div>
</template>