<script setup lang="ts">
import * as locales from '@nuxt/ui/locale';

const { locale } = useI18n();

useHead({
	titleTemplate: (titleChunk) => {
		return titleChunk ? `${titleChunk} - Simdyo` : 'Simdyo'
	},
})
</script>
<template>
	<UApp :locale="locales[locale]">
		<NuxtLayout>
		</NuxtLayout>
	</UApp>
</template>