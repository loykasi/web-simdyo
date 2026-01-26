<script setup lang="ts">
import CategoryBar from '~/components/explore/CategoryBar.vue';
import ProjectList from '~/components/explore/ProjectList.vue';
import type { Pagination } from '~/types/pagination.type';
import type { ProjectResponse } from '~/types/project.type';

const route = useRoute();

const searchQuery = computed(() => route.query.q || "");
const categoryQuery = computed(() => {
  let value = (route.params.category as string).toLowerCase();
  value = value === "all" ? "" : value;
});

const pageSize = 12;
const loading = ref(false);

const key = computed(() => `projects-${categoryQuery.value}`);

const { data: pagination, pending } = await useLazyAsyncData(
	key,
	() => useAPI<Pagination<ProjectResponse>>(`projects`, {
		method: "GET",
    query: {
      search: searchQuery.value,
      category: categoryQuery.value,
      limit: pageSize,
    }
	}), {
    watch: [searchQuery, categoryQuery]
  }
);

// const pagination = ref<Pagination<ProjectResponse>>({
//   total: 2,
//   size: 2,
//   lastId: 1,
//   items: [
//     {
//       publicId: '1',
//       title: 'GG EZ',
//       description: 'lorem isopum',
//       category: 'Gane',
//       projectLink: '#',
//       thumbnailLink: '',
//       username: 'john',
//       likeCount: 0,
//       isBanned: false,
//       createdAt: '#',
//       deletedAt: '#',
//     },
//     {
//       publicId: '1',
//       title: 'GG EZ',
//       description: 'lorem isopum',
//       category: 'Gane',
//       projectLink: '#',
//       thumbnailLink: '',
//       username: 'john',
//       likeCount: 0,
//       isBanned: false,
//       createdAt: '#',
//       deletedAt: '#',
//     },
//     {
//       publicId: '1',
//       title: 'GG EZ',
//       description: 'lorem isopum',
//       category: 'Gane',
//       projectLink: '#',
//       thumbnailLink: '',
//       username: 'john',
//       likeCount: 12,
//       isBanned: false,
//       createdAt: '#',
//       deletedAt: '#',
//     },
//     {
//       publicId: '1',
//       title: 'GG EZ',
//       description: 'lorem isopum',
//       category: 'Gane',
//       projectLink: '#',
//       thumbnailLink: '',
//       username: 'john',
//       likeCount: 0,
//       isBanned: false,
//       createdAt: '#',
//       deletedAt: '#',
//     }
//   ]
// });

// const pending = ref(false);

function showMore() {
  loading.value = true;
  useAPI<Pagination<ProjectResponse>>(`projects`, {
		method: "GET",
    query: {
      limit: pageSize,
      search: searchQuery.value,
      category: categoryQuery.value,
      cursor: pagination.value?.lastId
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

const searchTerm = ref(route.query.q || "");

function onSearch() {
  console.log(searchTerm.value);
	navigateTo({
		path: `/explore/${categoryQuery.value}`,
		query: {
			q: searchTerm.value,
		},
    force: true
	})
}

useHead({
  title: 'Explore',
})
</script>

<template>
  <UPage>
    <h1 class="my-6 font-bold text-4xl">Explorer</h1>
    <form @submit.prevent="onSearch" class="flex items-center bg-accented h-8">
			<input
				id="search"
				type="text"
				v-model="searchTerm"
				:placeholder="$t('nav.search')"
				required
				class="px-2.5 py-1.5 flex-1 text-sm"
			/>
      <button
        type="submit"
        class="h-full flex justify-center items-center px-2 hover:cursor-pointer">
        <UIcon name="material-symbols:search" class="size-5" />
      </button>
		</form>
    <CategoryBar />
    <ProjectList
      :pending="pending"
      :pagination="pagination"
    />
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
