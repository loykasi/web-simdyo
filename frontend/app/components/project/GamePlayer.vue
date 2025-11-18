<script setup lang="ts">
const prop = defineProps<{
    projectLink: string
}>();

const unityCanvas = ref<HTMLIFrameElement>();

function handleUnityMessage(event: any) {
    if (event.data?.type === 'unityLoaded') {
        console.log('✅ Unity WebGL loaded!', event.origin);

        if (unityCanvas.value?.contentWindow) {
            unityCanvas.value.contentWindow.postMessage(
                { type: 'loadProject', url: prop.projectLink },
                '*'
            )
        }
    }
}

onMounted(() => {
    window.addEventListener('message', handleUnityMessage)
})

onBeforeUnmount(() => {
    window.removeEventListener('message', handleUnityMessage)
})
</script>
<template>
    <!-- <iframe
        ref="unityCanvas"
        src="/game-build/index.html"
        width="100%"
        height="100%"
        class="size-full border-none"
        loading="lazy"
    ></iframe> -->
</template>