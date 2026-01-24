<script setup lang="ts">
const route = useRoute();
const category: string = (route.params.category as string || "").toLowerCase();

const { data: categories, pending } = await useLazyAsyncData(
	"projectCategories",
	() => useAPI<string[]>(`projects/categories/all`, {
		method: "GET"
	})
);

function toCategory(target?: string) {
  const searchTerm = route.query.q;
  const path = target === undefined ? "/explore" : `/explore/${target}`;
  if (searchTerm != null) {
    navigateTo({
      path: path,
      query: {
        q: searchTerm,
      },
    })
  } else {
    navigateTo({
      path: path,
    })
  }
}
</script>
<template>
    <div class="flex flex-wrap items-center py-4 gap-2 border-b border-default">
      <UButton
        label="All"
        :variant="category === 'all' ? 'solid' : 'outline'"
        @click="toCategory()"
      />
      <UButton
        v-for="item in categories"
        :label="item"
        :variant="item.toLowerCase() === category ? 'solid' : 'outline'"
        @click="toCategory(item.toLowerCase())"
      />
    </div>
</template>