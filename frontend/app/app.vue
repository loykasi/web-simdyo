<script setup lang="ts">
const { fetchProfile } = useLogin();
// onMounted(() => {
// 	fetchProfile();
// })

await callOnce(async () => {
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