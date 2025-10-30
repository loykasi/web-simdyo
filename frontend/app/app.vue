<script setup lang="ts">
const { fetchProfile } = useLogin();
onMounted(() => {
	fetchProfile();
})
</script>
<template>
	<UApp>
		<NuxtLayout>
		</NuxtLayout>
	</UApp>
</template>