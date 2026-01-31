<script setup lang="ts">
const unityLoaded = ref(false);

function handleUnityMessage(event: any) {
  if (event.data?.type === "unityLoaded") {
    unityLoaded.value = true;
  }
}

onMounted(() => {
  window.addEventListener("message", handleUnityMessage);
});

onBeforeUnmount(() => {
  window.removeEventListener("message", handleUnityMessage);
});

useHead({
  title: "Create",
});

definePageMeta({
  layout: "engine",
});
</script>
<template>
  <div>
    <ClientOnly>
      <iframe
        src="/engine/index.html"
        width="100%"
        height="100%"
        class="w-screen h-screen border-none"
      />
    </ClientOnly>
    <template v-if="!unityLoaded">
      <div
        class="absolute top-0 left-0 right-0 bottom-0 flex justify-center items-center size-full bg-muted z-10"
      >
        <div class="flex flex-col items-center gap-y-2.5">
          <UIcon name="lucide:loader-circle" class="size-12 animate-spin" />
          <label>Loading project</label>
        </div>
      </div>
    </template>
  </div>
</template>
