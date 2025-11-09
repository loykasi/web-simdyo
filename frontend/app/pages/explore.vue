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
		<ProjectCard
			v-for="project in projectsResponse?.projects"
			:project="project"
		/>
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
