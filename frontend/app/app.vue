<script setup lang="ts">
const toaster = { position: 'top-center' }

useHead({
	titleTemplate: (titleChunk) => {
		return titleChunk ? `${titleChunk} - Simdyo` : 'Simdyo'
	},
})
</script>
<template>
	<UApp :toaster="toaster">
		<NuxtLayout>
		</NuxtLayout>
	</UApp>
</template>