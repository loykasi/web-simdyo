<script setup lang="ts">
const prop = defineProps<{
    projectLink: string
}>();

const unityLoaded = ref(false);
const unityCanvas = ref<HTMLIFrameElement>();
const running = ref(true);

function handleUnityMessage(event: any) {
    if (event.data?.type === 'unityLoaded') {
        console.log('Unity WebGL loaded!', event.origin);
        unityLoaded.value = true;

        if (unityCanvas.value?.contentWindow) {
            unityCanvas.value.contentWindow.postMessage(
                { type: 'loadProject', url: prop.projectLink },
                '*'
            );
        }
    }
}

function pauseGame() {
    if (unityCanvas.value?.contentWindow) {
        unityCanvas.value.contentWindow.postMessage({ type: 'pause' }, '*');
        running.value = false;
    }
}

function resumeGame() {
    if (unityCanvas.value?.contentWindow) {
        unityCanvas.value.contentWindow.postMessage({ type: 'resume' }, '*');
        running.value = true;
    }
}

function restartGame() {
    if (unityCanvas.value?.contentWindow) {
        unityCanvas.value.contentWindow.postMessage({ type: 'restart' }, '*');
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
    <div class="mb-4">
        <div class="relative aspect-[16/9] w-full">
            <!-- <div class="size-full border-none z-0 bg-amber-700"></div> -->
            <iframe
                ref="unityCanvas"
                src="/game/index.html"
                width="100%"
                height="100%"
                class="size-full border-none z-0"
                loading="lazy"
            >
            </iframe>
            <template v-if="!unityLoaded">
                <div class="absolute top-0 left-0 right-0 bottom-0 flex justify-center items-center size-full bg-muted z-10">
                    <div class="flex flex-col items-center gap-y-2.5">
                        <UIcon name="lucide:loader-circle" class="size-12 animate-spin" />
                        <label>Loading project</label>
                    </div>
                </div>
            </template>
        </div>
        <div class="flex ">
            <UButton
                v-if="running"
                size="xs" variant="outline" color="neutral"
                :ui="{
                    base: 'rounded-none'
                }"
                @click="pauseGame"
            >
                Pause
            </UButton>
            <UButton
                v-else
                size="xs" variant="outline" color="neutral"
                :ui="{
                    base: 'rounded-none'
                }"
                @click="resumeGame"
            >
                Resume
            </UButton>
            <UButton
                size="xs" variant="outline" color="neutral"
                :ui="{
                    base: 'rounded-none'
                }"
                @click="restartGame"
            >
                Restart
            </UButton>
        </div>
    </div>
</template>