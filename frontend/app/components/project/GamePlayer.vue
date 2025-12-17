<script setup lang="ts">
const prop = defineProps<{
    projectLink: string
}>();

const unityLoaded = ref(false);
const unityCanvas = ref<HTMLIFrameElement>();

function handleUnityMessage(event: any) {
    if (event.data?.type === 'unityLoaded') {
        console.log('Unity WebGL loaded!', event.origin);
        unityLoaded.value = true;

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
    <template v-if="!unityLoaded">
        <div class="flex justify-center items-center size-full bg-muted">
            <div class="flex flex-col items-center gap-y-2.5">
                <UIcon name="lucide:loader-circle" class="size-12 animate-spin" />
                <label>Loading project</label>
            </div>
        </div>
    </template>
    <tempalate v-else>
        <iframe
            ref="unityCanvas"
            src="/game-build/index.html"
            width="100%"
            height="100%"
            class="size-full border-none"
            loading="lazy"
        ></iframe>
    </tempalate>
</template>