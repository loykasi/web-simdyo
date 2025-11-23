<script setup lang="ts">
useHead({
	titleTemplate: (titleChunk) => {
		return titleChunk ? `${titleChunk} - CodeVisdoo` : 'CodeVisdoo'
	},
})
</script>
<template>
	<UApp>
		<NuxtLayout>
		</NuxtLayout>
	</UApp>
</template>