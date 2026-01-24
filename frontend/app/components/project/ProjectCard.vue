<script setup lang="ts">
import type { ProjectResponse } from '~/types/project.type';

defineProps<{
  project: ProjectResponse
}>();
</script>
<template>
<!-- <div
    class="rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
>
    <NuxtLink
        :to="`/projects/${project.publicId}`"
        class="block aspect-square w-full"
    >
        <img
            alt="placeholder"
            :src="project.thumbnailLink === '' ? 'https://placehold.co/400' : project.thumbnailLink"
            class="aspect-square"
        />
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
</div> -->
  <div
    class="flex gap-x-4 rounded-lg bg-default ring ring-default divide-y divide-default"
  >
    <NuxtLink
      :to="`/projects/${project.publicId}`"
      class="block aspect-square h-32"
    >
      <img
        alt="placeholder"
        :src="project.thumbnailLink === '' ? 'https://placehold.co/400' : project.thumbnailLink"
        class="aspect-square"
      />
    </NuxtLink>

    <div class="py-2 h-32 flex flex-col">
      <NuxtLink
        :to="`/projects/${project.publicId}`"
        class="text-base text-primary line-clamp-1 font-bold hover:underline"
      >
        {{ project.title }}
      </NuxtLink>
      <div class="text-sm">
        <span>By </span>
        <NuxtLink
          :to="`/profile/${project.username}`"
          class="hover:text-secondary underline"
        >
          {{ project.username }}
        </NuxtLink>
      </div>
      <div class="flex-1 mt-2">
        <UTooltip
          text="Lorem ipsum dolor sit amet, consectetur adipiscing elit."
          :delay-duration="200"
        >
          <p class="line-clamp-2 overflow-hidden">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit.
          </p>
        </UTooltip>
      </div>
      <!-- <div class="">
        <UBadge color="neutral" variant="soft">Game</UBadge>
      </div> -->
    </div>

    <div class="py-2 pe-4 flex flex-col gap-y-2">
      <div class="flex items-center gap-1 text-muted">
        <span class="font-bold text-xl">{{ project.likeCount }}</span>
        <UIcon name="lucide:heart" class="size-5 mt-0.5" />
      </div>
      <div class="flex items-center gap-1 text-muted">
        <span class="font-bold text-xl">{{ project.likeCount }}</span>
        <UIcon name="lucide:meh" class="size-5 mt-0.5" />
      </div>
    </div>
  </div>
</template>