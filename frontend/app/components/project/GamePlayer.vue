<script setup lang="ts">
const disableGamePlayer = true;

const prop = defineProps<{
  projectLink: string;
}>();

const unityLoaded = ref(false);
const unityCanvas = ref<HTMLIFrameElement>();
const running = ref(true);
const controlDisable = ref(true);
const fullscreen = ref(false);

function handleUnityMessage(event: any) {
  if (event.data?.type === "unityLoaded") {
    console.log("Unity WebGL loaded!", event.origin);
    unityLoaded.value = true;

    if (unityCanvas.value?.contentWindow) {
      unityCanvas.value.contentWindow.postMessage(
        { type: "loadProject", url: prop.projectLink },
        "*",
      );
    }
  }

  if (event.data?.type === "gameLoaded") {
    console.log("Game loaded!", event.origin);
    controlDisable.value = false;
  }

  if (
    event.data?.type === "gameRestarted" ||
    event.data?.type === "gameResumed"
  ) {
    running.value = true;
  }

  if (event.data?.type === "gamePaused") {
    running.value = false;
  }
}

function pauseGame() {
  if (unityCanvas.value?.contentWindow) {
    unityCanvas.value.contentWindow.postMessage({ type: "pause" }, "*");
    unityCanvas.value.focus();
  }
}

function resumeGame() {
  if (unityCanvas.value?.contentWindow) {
    unityCanvas.value.contentWindow.postMessage({ type: "resume" }, "*");
    unityCanvas.value.focus();
  }
}

function restartGame() {
  if (unityCanvas.value?.contentWindow) {
    unityCanvas.value.contentWindow.postMessage({ type: "restart" }, "*");
    unityCanvas.value.focus();
  }
}

function toggleFullscreen() {
  fullscreen.value = !fullscreen.value;
}

onMounted(() => {
  window.addEventListener("message", handleUnityMessage);
});

onBeforeUnmount(() => {
  window.removeEventListener("message", handleUnityMessage);
});

// handle fullscreen responsive
const containerRef = useTemplateRef<HTMLDivElement>("container-ref");
const gameRef = useTemplateRef<HTMLDivElement>("game-ref");
const actionRef = useTemplateRef<HTMLDivElement>("action-ref");

const ASPECT = 16 / 9;

onMounted(() => {
  window.addEventListener("resize", onResize);
});

onBeforeUnmount(() => {
  window.removeEventListener("resize", onResize);
});

watch(
  () => gameRef.value || containerRef.value || actionRef.value,
  () => {
    onResize();
  },
);

watch(
  () => fullscreen.value,
  () => {
    nextTick(() => {
      onResize();
    });
  },
);

function onResize(event: Event | null = null) {
  if (!gameRef.value || !containerRef.value || !actionRef.value) return;

  const w = containerRef.value.clientWidth;
  const h = containerRef.value.clientHeight;

  let width = w;
  let height = w / ASPECT;

  if (height > h) {
    height = h;
    width = h * ASPECT;
  }

  gameRef.value.style.width = `${width}px`;
  gameRef.value.style.height = `${height}px`;
  gameRef.value.style.left = `${(w - width) / 2}px`;
  gameRef.value.style.top = `${(h - height) / 2}px`;

  actionRef.value.style.width = `${width}px`;

  console.log("resize");
}
</script>
<template>
  <div
    :class="`${fullscreen ? 'fixed inset-0 z-100 flex flex-col items-center bg-default' : ''}`"
  >
    <div
      ref="container-ref"
      :class="`relative flex flex-1 w-full justify-center items-center ${!fullscreen ? 'aspect-[16/9]' : ''}`"
    >
      <div ref="game-ref" class="absolute">
        <ClientOnly>
          <div
            v-if="disableGamePlayer"
            class="size-full border-none z-0"
          />
          <iframe
            v-else
            ref="unityCanvas"
            src="/game/index.html"
            width="100%"
            height="100%"
            class="size-full border-none z-0"
            loading="lazy"
          />
        </ClientOnly>
        <template v-if="!unityLoaded">
          <div
            class="absolute top-0 left-0 right-0 bottom-0 flex justify-center items-center size-full bg-muted z-10"
          >
            <div class="flex flex-col items-center gap-y-2.5">
              <UIcon name="lucide:loader-circle" class="size-12 animate-spin" />
            </div>
          </div>
        </template>
      </div>
    </div>
    <div ref="action-ref" :class="`flex ${fullscreen ? 'bg-accented' : ''}`">
      <UButton
        v-if="running"
        size="xs"
        variant="outline"
        color="neutral"
        :ui="{
          base: 'rounded-none',
        }"
        :disabled="controlDisable"
        @click="pauseGame"
      >
        {{ $t("gameplayer.pause") }}
      </UButton>
      <UButton
        v-else
        size="xs"
        variant="outline"
        color="neutral"
        :ui="{
          base: 'rounded-none',
        }"
        :disabled="controlDisable"
        @click="resumeGame"
      >
        {{ $t("gameplayer.resume") }}
      </UButton>
      <UButton
        size="xs"
        variant="outline"
        color="neutral"
        :ui="{
          base: 'rounded-none',
        }"
        :disabled="controlDisable"
        @click="restartGame"
      >
        {{ $t("gameplayer.restart") }}
      </UButton>
      <UButton
        size="xs"
        variant="outline"
        color="neutral"
        :ui="{
          base: 'rounded-none',
        }"
        @click="toggleFullscreen"
      >
        Fullscreen
      </UButton>
    </div>
  </div>
</template>
