<script setup lang="ts">
import type { ProjectResponse } from '~/types/project.type';

const route = useRoute();
const projectId = route.params.id as string;

const { data: project } = await useAsyncData(
	`project.${projectId}`,
	() => useAPI<ProjectResponse>(`projects/${projectId}`, {
		method: "GET",
	})
);

const detailOpen = ref(false);

function ToggleMoreInformation() {
    detailOpen.value = !detailOpen.value;
}
</script>
<template>
    <UPage>
        <!-- <UPageHeader title="Project Title" /> -->
        <div class="flex justify-between my-4">
            <h1 class="text-3xl font-semibold">{{ project?.title }}</h1>
            <div class="flex gap-x-3">
                <UButton size="xl" color="error">Report</UButton>
                <UButton size="xl">See inside</UButton>
            </div>
        </div>
        
        <div class="aspect-[16/9] w-full bg-blue-800 mb-4"></div>

        <UCard class="mt-4">
            <div class="flex items-center justify-between gap-x-3 w-full">
                <div class="flex items-center gap-x-4">
                    <NuxtLink :to="`/profile/${project?.userName}`">
                        <UUser
                            :name="project?.userName"
                            size="lg"
                            :avatar="{
                                src: 'https://i.pravatar.cc/150?u=john-doe',
                                icon: 'i-lucide-image'
                            }"
                        />
                    </NuxtLink>
                    <span class="text-gray-300">Jun 21, 2025</span>
                </div>
                <div class="flex items-center gap-x-4">
                    <div class="flex items-center">
                        <UIcon name="i-lucide-lightbulb" class="size-5" />
                        <span class="ms-2 font-semibold text-xl">100</span>
                    </div>
                    <div class="flex items-center">
                        <UIcon name="i-lucide-lightbulb" class="size-5" />
                        <span class="ms-2 font-semibold text-xl">100</span>
                    </div>
                </div>
            </div>
        </UCard>

        <UCard class="mt-4">
            <div>
                <div>
                    <span>{{ project?.description }}</span>
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
                                            <td>{{ project?.userName }}</td>
                                        </tr>
                                        <tr>
                                            <td class="pe-4 py-1.5 font-medium text-default">Category</td>
                                            <td>{{ project?.category }}</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </template>
                    </UCollapsible>
                </div>
            </div>
        </UCard>
    </UPage>
</template>