<script setup lang="ts">
const { fetchProfile } = useLogin();
onMounted(() => {
	fetchProfile();
})

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