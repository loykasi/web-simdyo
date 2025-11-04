<script setup lang="ts">
import type { ProjectsResponse } from '~/types/project.type';

const { data: projectsResponse } = await useAsyncData(
	"projects",
	() => useAPI<ProjectsResponse>(`projects`, {
		method: "GET",
	})
);
</script>

<template>
  <UPage>
    <UPageHeader title="Explorer" />

    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-8">
      	<NuxtLink
			v-for="project in projectsResponse?.projects"
			:to="`/projects/${project.id}`"
			class="block rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
		>
			<img alt="placeholder" src="https://placehold.co/400" />

			<div class="p-4">
			<h3 class="text-lg block h-14 line-clamp-2 font-bold">{{ project.title }}</h3>
			<NuxtLink
				:to="`/profile/${project.userName}`"
				class="mt-1 text-sm text-blue-200 hover:underline"
			>{{ project.userName }}</NuxtLink>
			</div>
		</NuxtLink>
    </div>

    <UButton
      type="submit"
      size="lg"
      class="w-full mt-8 flex justify-center items-center"
    >
      Show more
    </UButton>
  </UPage>
</template>
