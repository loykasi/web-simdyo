<script setup lang="ts">
import { useAuthStore } from './stores/auth.store';

const { fetchProfile } = useLogin();
const { isLoggedIn } = useAuthStore();
// onMounted(() => {
// 	fetchProfile();
// })

await callOnce(async () => {
	fetchProfile();
});

watchEffect(() => {
	console.log(isLoggedIn.value);
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