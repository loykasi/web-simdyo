<script setup lang="ts">
const cards = ref([
  {
    thumbnail: 'https://placehold.co/400',
    title: 'Scene title',
    author: 'Scene author'
  },
  {
    thumbnail: 'https://placehold.co/400',
    title: 'Scene title',
    author: 'Scene author'
  },
  {
    thumbnail: 'https://placehold.co/400',
    title: 'Long scene title',
    author: 'Scene author'
  },
  {
    thumbnail: 'https://placehold.co/400',
    title: 'Very long long Scene title',
    author: 'Scene author'
  },
  {
    thumbnail: 'https://placehold.co/400',
    title: 'Scene title',
    author: 'Scene author'
  },
])
</script>

<template>
    <UPage>
        <UPageHeader title="Author Username" />

        <UCard class="mt-4">
            <div>
                Username
            </div>
            <div>
                Joined Date
            </div>
            <div>
                Total Projects
            </div>
            <div>
                Total View
            </div>
        </UCard>

        <h2 class="text-2xl font-bold mt-4">Projects</h2>
        <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-8">
        <div
            v-for="card in cards"
            class="rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
        >
            <img alt="placeholder" :src="card.thumbnail" />

            <div class="p-4">
            <h3 class="text-lg block h-14 line-clamp-2 font-bold">{{ card.title }}</h3>
            <div class="mt-1 text-sm text-blue-200">{{ card.author }}</div>
            <button class="mt-2 w-full py-1 text-sm bg-blue-200 text-black font-semibold rounded-sm">Download</button>
            </div>
        </div>
        </div>
    </UPage>
</template>