<script setup lang="ts">
import CategoryBar from "~/components/explore/CategoryBar.vue";
import ProjectList from "~/components/explore/ProjectList.vue";
import type { Pagination } from "~/types/pagination.type";
import type { ProjectResponse } from "~/types/project.type";

const route = useRoute();

const searchQuery = computed(() => route.query.q || "");
const categoryQuery = computed(() => {
  let value = (route.params.category as string).toLowerCase();
  value = value === "all" ? "" : value;
  return value;
});

const pageSize = 12;
const loading = ref(false);

const key = computed(() => `projects-${categoryQuery.value}`);

const { data: pagination, pending } = await useLazyAsyncData(
  key,
  () =>
    useAPI<Pagination<ProjectResponse>>(`projects`, {
      method: "GET",
      query: {
        search: searchQuery.value,
        category: categoryQuery.value,
        limit: pageSize,
      },
    }),
  {
    watch: [searchQuery, categoryQuery],
  },
);

function showMore() {
  loading.value = true;
  useAPI<Pagination<ProjectResponse>>(`projects`, {
    method: "GET",
    query: {
      limit: pageSize,
      search: searchQuery.value,
      category: categoryQuery.value,
      cursor: pagination.value?.lastId,
    },
  })
    .then((res) => {
      if (pagination.value == undefined) return;

      pagination.value.items = [...pagination.value.items, ...res.items];
      pagination.value.size += res.size;
      pagination.value.lastId = res.lastId;
      pagination.value.total = res.total;
    })
    .catch()
    .finally(() => {
      loading.value = false;
    });
}

const searchTerm = ref(route.query.q || "");

function onSearch() {
  console.log(searchTerm.value);
  navigateTo({
    path: `/explore/${categoryQuery.value}`,
    query: {
      q: searchTerm.value,
    },
    force: true,
  });
}

useHead({
  title: "Explore",
});
</script>

<template>
  <UPage>
    <h1 class="my-6 font-bold text-4xl">{{ $t('explore.title') }}</h1>
    <form class="flex items-center bg-accented h-8" @submit.prevent="onSearch">
      <input
        id="search"
        v-model="searchTerm"
        type="text"
        :placeholder="$t('explore.search')"
        required
        class="px-2.5 py-1.5 flex-1 text-sm"
      />
      <button
        type="submit"
        class="h-full flex justify-center items-center px-2 hover:cursor-pointer"
      >
        <UIcon name="material-symbols:search" class="size-5" />
      </button>
    </form>
    <CategoryBar />
    <ProjectList :pending="pending" :pagination="pagination" />
    <UButton
      v-if="pagination?.size != pagination?.total"
      type="submit"
      size="lg"
      color="secondary"
      :loading="loading"
      class="mx-auto w-md mt-8 flex justify-center items-center"
      @click="showMore"
    >
      Show more
    </UButton>
  </UPage>
</template>
