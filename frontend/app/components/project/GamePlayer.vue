<script setup lang="ts">
const disableGamePlayer = true;

const prop = defineProps<{
  projectLink: string;
}>();

const unityLoaded = ref(false);
const unityCanvas = ref<HTMLIFrameElement>();
const running = ref(true);
const controlDisable = ref(true);

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

onMounted(() => {
  window.addEventListener("message", handleUnityMessage);
});

onBeforeUnmount(() => {
  window.removeEventListener("message", handleUnityMessage);
});
</script>
<template>
  <div class="relative aspect-[16/9] w-full">
    <ClientOnly>
      <div
        v-if="disableGamePlayer"
        class="size-full border-none z-0 bg-amber-700"
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
  <div class="flex">
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
  </div>
</template>
