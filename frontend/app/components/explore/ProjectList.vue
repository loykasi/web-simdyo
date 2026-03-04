<script setup lang="ts">
import type { Pagination } from "~/types/pagination.type";
import type { ProjectResponse } from "~/types/project.type";

defineProps<{
  pending: boolean;
  pagination: Pagination<ProjectResponse> | undefined;
}>();

const pageSize = 4;
</script>
<template>
  <template v-if="pending && !pagination">
    <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 gap-4">
      <USkeleton
        v-for="item in pageSize"
        :key="item"
        class="h-32 w-full"
      />
    </div>
  </template>
  <template v-else-if="pagination && pagination.size > 0">
    <div class="mt-4 grid grid-cols-1 md:grid-cols-2 gap-4">
      <ProjectCard
        v-for="project in pagination?.items"
        :key="project.publicId"
        :project="project"
      />
    </div>
  </template>
  <template v-else>
    <UEmpty
      icon="material-symbols:sad-tab-outline-rounded"
      title="No results found"
    />
  </template>
</template>
