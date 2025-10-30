<script setup lang="ts">
const cards = ref([
  {
    id: '1',
    thumbnail: 'https://placehold.co/400',
    title: 'Scene title',
    author: 'Scene author'
  },
  {
    id: '1',
    thumbnail: 'https://placehold.co/400',
    title: 'Scene title',
    author: 'Scene author'
  },
  {
    id: '1',
    thumbnail: 'https://placehold.co/400',
    title: 'Long scene title',
    author: 'Scene author'
  },
  {
    id: '1',
    thumbnail: 'https://placehold.co/400',
    title: 'Very long long Scene title',
    author: 'Scene author'
  },
  {
    id: '1',
    thumbnail: 'https://placehold.co/400',
    title: 'Scene title',
    author: 'Scene author'
  },
])
</script>

<template>
  <UPage>
    <UPageHeader title="Explorer" />

    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-8">
      <a
        v-for="card in cards"
        :href="`/projects/${card.id}`"
        class="block rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
      >
        <img alt="placeholder" :src="card.thumbnail" />

        <div class="p-4">
          <h3 class="text-lg block h-14 line-clamp-2 font-bold">{{ card.title }}</h3>
          <div class="mt-1 text-sm text-blue-200">{{ card.author }}</div>
        </div>
      </a>
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
