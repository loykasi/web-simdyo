<script setup lang="ts">
import { useAuthStore } from '~/stores/auth.store';
import type { ProjectResponse } from '~/types/project.type';

const { user, isLoggedIn } = useAuthStore();
const route = useRoute();
const projectId = route.params.id as string;

const { likeProject, unlikeProject } = useLikeProject();

const { data: project, status: fetchProjectStatus } = await useAsyncData(
	`project.${projectId}`,
	() => useAPI<ProjectResponse>(`projects/${projectId}`, {
		method: "GET",
	}),
    {
        server: false,
        lazy: true
    }
);

const { data: likeCount } = await useAsyncData(
	`project.${projectId}.likeCount`,
	() => useAPI<number>(`projects/${projectId}/like`, {
		method: "GET",
	})
);

const { data: likeStatus } = useAsyncData(
	`project.${projectId}.likeStatus`,
	() => useAPI<boolean>(`projects/${projectId}/like-status`, {
		method: "GET",
	}),
    {
        server: false
    }
);

async function like() {
    if (likeStatus.value) {
        likeStatus.value = false;
        likeCount.value! -= 1;

        unlikeProject(projectId)
            .catch(() => {
                likeStatus.value = true;
                likeCount.value! += 1;
            });
    } else {
        likeStatus.value = true;
        likeCount.value! += 1;

        likeProject(projectId)
            .catch(() => {
                likeStatus.value = false;
                likeCount.value! -= 1;
            });
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
        if (project.value !== undefined)
        {
            console.log("delete");
            project.value.deletedAt = new Date().toISOString();
        }
    })
    .catch()
    .finally(() => {
        statusLoading.value = false;
    })
}

async function restoreProject() {
    statusLoading.value = true;
    
    useAPI(`projects/${projectId}/restore`, {
		method: "POST",
	})
    .then(() => {
        if (project.value !== undefined)
        {
            console.log("restore");
            project.value.deletedAt = null;
        }
    })
    .catch()
    .finally(() => {
        statusLoading.value = false;
    })
}

//////

const unityCanvas = ref<HTMLIFrameElement>();

function handleUnityMessage(event: any) {
    if (event.data?.type === 'unityLoaded') {
        console.log('✅ Unity WebGL loaded!', event.origin);

        if (unityCanvas.value?.contentWindow) {
            unityCanvas.value.contentWindow.postMessage(
                { type: 'loadProject', url: project.value?.projectLink },
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
    <UPage>
        <template v-if="fetchProjectStatus === 'pending' || fetchProjectStatus === 'idle'">
            Loading...
        </template>
        <template v-else-if="fetchProjectStatus === 'success' && project">
            <div class="flex justify-between my-4">
                <h1 class="text-3xl font-semibold">{{ project.title }}</h1>
                <div class="flex gap-x-3">
                    <template v-if="isAuthor()">
                        <UButton
                            v-if="project.deletedAt === null"
                            size="xl"
                            color="error"
                            @click="deleteProject"
                            :loading="statusLoading"
                        >
                            Delete
                        </UButton>
                        <UButton
                            v-else
                            size="xl"
                            color="error"
                            @click="restoreProject"
                            :loading="statusLoading"
                        >
                            Restore
                        </UButton>
                    </template>
                    <!-- <UButton size="xl" color="error">Report</UButton>
                    <UButton size="xl">See inside</UButton> -->
                </div>
            </div>
            
            <div class="aspect-[16/9] w-full bg-blue-800 mb-4">
                <!-- <iframe
                    ref="unityCanvas"
                    src="/game-build/index.html"
                    width="100%"
                    height="100%"
                    class="size-full border-none"
                    loading="lazy"
                ></iframe> -->
            </div>

            <UCard class="mt-4">
                <div class="flex items-center justify-between gap-x-3 w-full">
                    <div class="flex items-center">
                        <span class="text-dimmed me-2">By</span>
                        <NuxtLink
                            :to="`/profile/${project.username}`"
                            class="hover:underline"
                        >
                            <UUser
                                :name="project.username"
                                size="lg"
                            />
                        </NuxtLink>
                        <span class="text-dimmed ms-4">
                            <NuxtTime :datetime="project.createdAt"></NuxtTime>
                        </span>
                    </div>
                    <div class="flex items-center gap-x-4">
                        <ClientOnly>
                            <UButton color="neutral" variant="ghost" >
                                <div class="flex items-center" @click="like">
                                    <template v-if="likeStatus">
                                        <UIcon name="material-symbols:favorite" class="block size-5" />
                                    </template>
                                    <template v-else>
                                        <UIcon name="material-symbols:favorite-outline" class="block size-5" />
                                    </template>
                                    
                                    <span class="block ms-2 font-semibold text-xl">{{ likeCount }}</span>
                                </div>
                            </UButton>
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
                            v-on:click="ToggleMoreInformation"
                            class="underline cursor-pointer"
                        >
                            More information
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
                                                <td class="pe-4 py-1.5 font-medium text-default">Author</td>
                                                <td>{{ project.username }}</td>
                                            </tr>
                                            <tr>
                                                <td class="pe-4 py-1.5 font-medium text-default">Category</td>
                                                <td>{{ project.category }}</td>
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
    </UPage>
</template>