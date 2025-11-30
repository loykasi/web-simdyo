<script setup lang="ts">
import CategoryBar from '~/components/explore/CategoryBar.vue';
import type { Pagination } from '~/types/pagination.type';
import type { ProjectResponse } from '~/types/project.type';

const route = useRoute();

const searchQuery = computed(() => route.query.q || "");

const pageSize = 6;
const loading = ref(false);

const { data: pagination, pending } = await useLazyAsyncData(
	"projects",
	() => useAPI<Pagination<ProjectResponse>>(`projects`, {
		method: "GET",
        query: {
        q: searchQuery.value,
        limit: pageSize,
        }
	}), {
    watch: [searchQuery]
  }
);

function showMore() {
  loading.value = true;
  useAPI<Pagination<ProjectResponse>>(`projects`, {
		method: "GET",
        query: {
        limit: pageSize,
        q: searchQuery.value,
        lastId: pagination.value?.lastId
        }
	})
    .then(res => {
        if (pagination.value == undefined) return;

        pagination.value.items = [...pagination.value.items, ...res.items];
        pagination.value.size += res.size;
        pagination.value.lastId = res.lastId;
        pagination.value.total = res.total;
    })
    .catch()
    .finally(() => {
        loading.value = false;
    })
}

useHead({
  title: 'Explore',
})
</script>

<template>
  <UPage>
    <h1 class="my-6 font-bold text-4xl">Explorer</h1>

    <CategoryBar />

    <template v-if="pending && !pagination">
      <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-6 gap-8">
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
      <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-6 gap-8">
        <ProjectCard
          v-for="project in pagination?.items"
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
