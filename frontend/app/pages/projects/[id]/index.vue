<script setup lang="ts">
import GamePlayer from "~/components/project/GamePlayer.vue";
import ReportModal from "~/components/project/ReportModal.vue";
import { useAuthStore } from "~/stores/auth.store";
import type { ProjectResponse } from "~/types/project.type";
import type { projectReactionType } from "~/types/projectReaction.type";

const { user, isLoggedIn } = useAuthStore();
const route = useRoute();
const projectId = route.params.id as string;

const isLogged = useCookie("isLogged", {
  default: () => false,
});

const { addReaction, deleteReaction } = useProjectReaction();

const { data: project, status } = await useAsyncData(
  `project.${projectId}`,
  () =>
    useAPI<ProjectResponse>(`projects/${projectId}`, {
      method: "GET",
    }),
  {
    server: false,
    lazy: true,
  },
);

const isPending = computed(() => ["idle", "pending"].includes(status.value));

// const { data: likeCount } = await useAsyncData(
// 	`project.${projectId}.likeCount`,
// 	() => useAPI<number>(`projects/${projectId}/like`, {
// 		method: "GET",
// 	})
// );

const reactionStatus = ref<projectReactionType>("");

if (isLogged.value) {
  useAsyncData(
    `project.${projectId}.reactionStatus`,
    () =>
      useAPI<projectReactionType>(`projects/${projectId}/reaction-status`, {
        method: "GET",
      }),
    {
      server: false,
      transform: (data) => {
        reactionStatus.value = data;
      },
    },
  );
}

async function reactLike() {
  if (!project.value) return;

  if (reactionStatus.value === "Like") {
    reactionStatus.value = "";
    project.value.likeCount--;

    try {
      await deleteReaction(projectId);
    } catch {
      reactionStatus.value = "Like";
      project.value.likeCount++;
    }
  } else {
    project.value.likeCount++;
    if (reactionStatus.value === "Okay") {
      project.value.okayCount--;
    }

    reactionStatus.value = "Like";
    try {
      await addReaction(projectId, "Like");
    } catch {
      reactionStatus.value = "";
      project.value.likeCount--;
    }
  }
}

async function reactOkay() {
  if (!project.value) return;

  if (reactionStatus.value === "Okay") {
    reactionStatus.value = "";
    project.value.okayCount--;

    try {
      await deleteReaction(projectId);
    } catch {
      reactionStatus.value = "Okay";
      project.value.okayCount++;
    }
  } else {
    project.value.okayCount++;
    if (reactionStatus.value === "Like") {
      project.value.likeCount--;
    }

    reactionStatus.value = "Okay";
    try {
      await addReaction(projectId, "Okay");
    } catch {
      reactionStatus.value = "";
      project.value.okayCount--;
    }
  }
}

const detailOpen = ref(false);

function ToggleMoreInformation() {
  detailOpen.value = !detailOpen.value;
}

///////

const statusLoading = ref(false);

function isAuthor() {
  return isLoggedIn.value && user.value?.username == project.value?.username;
}

async function deleteProject() {
  statusLoading.value = true;

  useAPI(`projects/${projectId}`, {
    method: "DELETE",
  })
    .then(() => {
      if (project.value !== undefined) {
        console.log("delete");
        project.value.deletedAt = new Date().toISOString();
      }
    })
    .catch()
    .finally(() => {
      statusLoading.value = false;
    });
}

async function restoreProject() {
  statusLoading.value = true;

  useAPI(`projects/${projectId}/restore`, {
    method: "POST",
  })
    .then(() => {
      if (project.value !== undefined) {
        console.log("restore");
        project.value.deletedAt = null;
      }
    })
    .catch()
    .finally(() => {
      statusLoading.value = false;
    });
}

async function editProject() {
  navigateTo(`/projects/${projectId}/edit`);
}

const headTitle = computed(() =>
  project.value ? project.value.title : route.fullPath,
);
useHead({
  title: headTitle,
});

/////

// use for default width reference
const containerRef = useTemplateRef<HTMLDivElement>("container-ref");

// scale this for responsive
const innerContainerRef = useTemplateRef<HTMLDivElement>("inner-container-ref");
const gamePlayerRef = useTemplateRef<HTMLDivElement>("game-player-ref");

const minHeight = 360;
const headerHeight = 64;
const titleHeight = 68;
const controllerHeight = 24;

onMounted(() => {
  window.addEventListener("resize", onResize);
});

onBeforeUnmount(() => {
  window.removeEventListener("resize", onResize);
});

watch(
  () => gamePlayerRef.value && containerRef.value && innerContainerRef.value,
  () => {
    onResize();
  }
)

function onResize(event: Event | null = null) {
  if (!gamePlayerRef.value || !containerRef.value || !innerContainerRef.value)
    return;

  const defaultGameCanvasHeight = (containerRef.value.offsetWidth * 9) / 16;
  const margin = parseFloat(getComputedStyle(gamePlayerRef.value).marginBottom);
  const defaultHeight =
    headerHeight +
    titleHeight +
    defaultGameCanvasHeight +
    controllerHeight +
    margin;

  const target = (event?.target as Window) || window;
  const visibleHeight = Math.max(
    Math.min(target.innerHeight, defaultHeight),
    minHeight,
  );
  const remainingHeight =
    visibleHeight - headerHeight - titleHeight - controllerHeight - margin;
  const targetWidth = (remainingHeight * 16) / 9;

  innerContainerRef.value.style.width = `${targetWidth}px`;
}
</script>
<template>
  <div ref="container-ref">
    <div ref="inner-container-ref" class="mx-auto">
      <template v-if="isPending"> Loading... </template>
      <template v-else-if="project">
        <div class="flex justify-between my-4">
          <h1 class="text-3xl font-semibold">{{ project.title }}</h1>
          <div class="flex gap-x-3">
            <template v-if="isAuthor()">
              <UButton
                v-if="project.deletedAt === null"
                color="error"
                :loading="statusLoading"
                @click="deleteProject"
              >
                {{ $t("project.delete") }}
              </UButton>
              <UButton
                v-else
                color="error"
                :loading="statusLoading"
                @click="restoreProject"
              >
                {{ $t("project.restore") }}
              </UButton>

              <UButton
                color="secondary"
                :loading="statusLoading"
                @click="editProject"
              >
                {{ $t("project.edit") }}
              </UButton>
            </template>
            <UButton :to="project.projectLink" color="secondary">
              {{ $t("project.download") }}
            </UButton>
            <!-- <UButton size="xl">See inside</UButton> -->
            <ReportModal :project="project" />
          </div>
        </div>

        <div ref="game-player-ref" class="mb-4">
          <GamePlayer :project-link="project.projectLink" />
        </div>

        <UCard class="mt-4">
          <div class="flex items-center justify-between gap-x-3 w-full">
            <div class="flex items-center">
              <span class="text-dimmed me-2">{{ $t("project.main.by") }}</span>
              <NuxtLink
                :to="`/profile/${project.username}`"
                class="hover:underline"
              >
                <UUser :name="project.username" size="lg" />
              </NuxtLink>
              <span class="text-dimmed ms-4">
                <NuxtTime :datetime="project.createdAt" />
              </span>
            </div>
            <div class="flex items-center gap-x-2">
              <ClientOnly>
                <UTooltip text="I love this!">
                  <UButton color="neutral" variant="ghost" @click="reactLike">
                    <div
                      v-if="reactionStatus === 'Like'"
                      class="text-primary flex items-center"
                    >
                      <UIcon
                        name="material-symbols:favorite"
                        class="block size-6"
                      />
                      <span class="block ms-2 font-semibold text-xl">{{
                        project.likeCount
                      }}</span>
                    </div>
                    <div v-else class="flex items-center">
                      <UIcon
                        name="material-symbols:favorite-outline"
                        class="block size-6"
                      />
                      <span class="block ms-2 font-semibold text-xl">{{
                        project.likeCount
                      }}</span>
                    </div>
                  </UButton>
                </UTooltip>
                <UTooltip text="Nice try!">
                  <UButton color="neutral" variant="ghost" @click="reactOkay">
                    <div
                      v-if="reactionStatus === 'Okay'"
                      class="text-primary flex items-center"
                    >
                      <UIcon
                        name="material-symbols:sentiment-neutral"
                        class="block size-6"
                      />
                      <span class="block ms-2 font-semibold text-xl">{{
                        project.okayCount
                      }}</span>
                    </div>
                    <div v-else class="flex items-center">
                      <UIcon
                        name="material-symbols:sentiment-neutral-outline"
                        class="block size-6"
                      />
                      <span class="block ms-2 font-semibold text-xl">{{
                        project.okayCount
                      }}</span>
                    </div>
                  </UButton>
                </UTooltip>
              </ClientOnly>
            </div>
          </div>
        </UCard>

        <UCard class="mt-4">
          <div>
            <div>
              <span>{{ project.description }}</span>
            </div>
            <div class="mt-4">
              <span
                class="underline cursor-pointer"
                @click="ToggleMoreInformation"
              >
                {{ $t("project.more") }}
              </span>
              <UCollapsible
                v-model:open="detailOpen"
                class="flex flex-col gap-2 mt-2"
              >
                <template #content>
                  <div class="border border-dashed border-accented px-4 py-3.5">
                    <table>
                      <tbody>
                        <tr>
                          <td class="pe-4 py-1.5 font-medium text-default">
                            {{ $t("author") }}
                          </td>
                          <td>
                            <NuxtLink
                              :to="`/profile/${project.username}`"
                              class="underline"
                            >
                              {{ project.username }}
                            </NuxtLink>
                          </td>
                        </tr>
                        <tr>
                          <td class="pe-4 py-1.5 font-medium text-default">
                            {{ $t("category") }}
                          </td>
                          <td>{{ project.category ?? "All" }}</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </template>
              </UCollapsible>
            </div>
          </div>
        </UCard>
        <ClientOnly>
          <ProjectComment />
        </ClientOnly>
      </template>
      <template v-else>
        <UEmpty
          icon="i-lucide-file"
          title="No projects found"
          description="Make sure you've type the url correctly."
        />
      </template>
    </div>
  </div>
</template>
