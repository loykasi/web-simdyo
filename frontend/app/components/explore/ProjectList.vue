<script setup lang="ts">
import type { Pagination } from "~/types/pagination.type";
import type { ProjectResponse } from "~/types/project.type";

defineProps<{
  pending: boolean;
  pagination: Pagination<ProjectResponse> | undefined;
}>();

const pageSize = 6;
</script>
<template>
  <template v-if="pending && !pagination">
    <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-6 gap-4">
      <div
        v-for="item in pageSize"
        :key="item"
        class="rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
      >
        <USkeleton class="aspect-square w-full" />
        <div class="p-4">
          <USkeleton class="w-full h-[84px]" />
        </div>
      </div>
    </div>
  </template>
  <template v-else-if="pagination && pagination.size > 0">
    <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-2 gap-4">
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
