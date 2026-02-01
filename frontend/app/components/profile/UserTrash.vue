<script setup lang="ts">
import type { Pagination } from "~/types/pagination.type";
import type { ProjectResponse } from "~/types/project.type";
import ProjectList from "../explore/ProjectList.vue";

const route = useRoute();
const username = route.params.username as string;

const pageSize = 10;
const currentPage = ref(1);
let abortController = new AbortController();

const {
  data: projectPage,
  pending: projectPagePending,
  refresh: refreshProjectPage,
} = await useLazyAsyncData(`${username}.trash`, () =>
  useAPI<Pagination<ProjectResponse>>(`projects/users/trash`, {
    method: "GET",
    query: {
      page: currentPage.value,
      limit: pageSize,
    },
    signal: abortController.signal,
  }),
);

async function updatePage(page: number) {
  if (projectPagePending.value) {
    abortController.abort();
  }

  currentPage.value = page;
  abortController = new AbortController();

  refreshProjectPage();
}
</script>
<template>
  <!-- <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-8">
    <template v-if="projectPagePending">
      <div
        v-for="item in pageSize"
        :key="item"
        class="rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
      >
        <USkeleton class="aspect-square w-full" />
        <div class="p-4 h-[84px]">
          <USkeleton class="size-full" />
        </div>
      </div>
    </template>
    <template v-else>
      <ProjectCard
        v-for="project in projectPage?.items"
        :key="project.publicId"
        :project="project"
      />
    </template>
  </div> -->

  <ProjectList :pending="projectPagePending" :pagination="projectPage" />

  <div class="mt-4 flex justify-end">
    <UPagination
      v-if="projectPage?.size != projectPage?.total"
      :default-page="currentPage"
      :items-per-page="pageSize"
      :total="projectPage?.total"
      @update:page="updatePage"
    />
  </div>
</template>
