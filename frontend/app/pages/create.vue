<script setup lang="ts">
useHead({
    title: "Create",
});

definePageMeta({
    layout: 'engine',
});
</script>
<template>
    <ClientOnly>
        <iframe
            src="/engine-build/index.html"
            width="100%"
            height="100%"
            class="w-screen h-screen border-none"
        ></iframe>
    </ClientOnly>
</template>