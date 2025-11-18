<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import { useProject } from '~/composables/useProject';
import type { ProjectResponse } from '~/types/project.type';

const route = useRoute();
const projectId = route.params.id as string;

const toast = useToast();
const { update } = useProject();

const categories = ref(['Default', 'Game', 'Prototype', 'Simulation', 'Animation']);

const MAX_FILE_SIZE = 2 * 1024 * 1024 // 2MB

const formatBytes = (bytes: number, decimals = 2) => {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const dm = decimals < 0 ? 0 : decimals
  const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Number.parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i]
}

const schema = z.object({
    title: z.string('Invalid title'),
	description: z.string().default(""),
    category: z.string().default(""),
    projectFile: z
        .instanceof(File, {
            message: 'Please select a file.'
        })
        .refine((file) => file.size <= MAX_FILE_SIZE, {
            message: `The image is too large. Please choose an image smaller than ${formatBytes(MAX_FILE_SIZE)}.`
        })
        .optional(),
    thumbnailFile: z
        .instanceof(File, {
            message: 'Please select a file.'
        })
        .refine((file) => file.size <= MAX_FILE_SIZE, {
            message: `The image is too large. Please choose an image smaller than ${formatBytes(MAX_FILE_SIZE)}.`
        })
        .optional()
})

type schema = z.output<typeof schema>

const state = reactive<Partial<schema>>({
    projectFile: undefined,
    thumbnailFile: undefined,
    category: "Default"
})

function createObjectUrl(file: File): string {
    return URL.createObjectURL(file)
}

const loading = ref(false);

async function onSubmit(event: FormSubmitEvent<schema>) {
    const payload = new FormData();
    payload.append("title", event.data.title);
    payload.append("description", event.data.description);
    payload.append("category", event.data.category);
    if (event.data.projectFile !== undefined) {
        payload.append("projectFile", event.data.projectFile);
    }
    if (event.data.thumbnailFile !== undefined) {
        payload.append("thumbnailFile", event.data.thumbnailFile);
    }

    // for (const [key, value] of payload.entries()) {
    //     console.log(`${key}: ${value}`);
    // }

    loading.value = true;
    update(projectId, payload)
        .then((res) => {
            navigateTo(`/projects/${res.publicId}`);
        })
        .catch((error) => {
            toast.add({
                title: "Failed",
                description: "Something wrong!",
                color: "error",
            })
        })
        .finally(() => {
            loading.value = false;
        });
}


// const { data: project, pending: projectPending } = await useAsyncData(
// 	`project.${projectId}`,
// 	() => useAPI<ProjectResponse>(`projects/${projectId}`, {
// 		method: "GET",
// 	}),
//     {
//         server: false,
//         lazy: true
//     }
// );

// watchEffect(() => {
//     if (!projectPending.value && project) {
//         console.log(project.value);
//         state.title = project.value?.title;
//         state.description = project.value?.description;
//         state.category = project.value?.category;
//     }
// })

const projectPending = ref(true);
const defaultThumbnail = ref("https://placehold.co/400");
onMounted(() => {
    useAPI<ProjectResponse>(`projects/${projectId}`, {
		method: "GET",
	}).then(res => {
        console.log("run");
        state.title = res.title;
        state.description = res.description;
        state.category = res.category;
        defaultThumbnail.value = res.thumbnailLink;
    }).catch(err => {

    }).finally(() => {
        projectPending.value = false;
    })
})

const headTitle = computed(() => `${state.title || route.fullPath} - Edit`);
useHead({
	title: headTitle,
})
</script>
<template>
    <UPage>
        <UPageHeader title="Edit" />

        <template v-if="projectPending">
            <!-- <UProgress /> -->
        </template>
        <template v-else>
            <UCard class="mt-4">
                <UForm :schema="schema" :state="state" @submit="onSubmit">
                    <div class="grid grid-cols-3 space-x-12 space-y-4">
                        <div>
                            <!-- <UFormField label="Select thumbnail" name="thumbnailFile">
                                <UFileUpload
                                    v-model="state.thumbnailFile"
                                    accept="image/*"
                                    label="Drop your image here"
                                    :interactive="false"
                                    class="aspect-square mt-2"
                                >
                                    
                                    <template #actions="{ open }">
                                        <UButton
                                            label="Upload image"
                                            icon="i-lucide-upload"
                                            color="neutral"
                                            variant="outline"
                                            @click="open()"
                                        />
                                    </template>
                                </UFileUpload>
                            </UFormField> -->
                            <UFormField label="Select thumbnail" name="thumbnailFile">
                                <UFileUpload
                                    v-model="state.thumbnailFile"
                                    accept="image/*"
                                    v-slot="{ open, removeFile }"
                                    class="relative aspect-square mt-2 flex flex-col items-center justify-center bg-neutral-50 dark:bg-neutral-600"
                                >
                                    <img
                                        :src="state.thumbnailFile ? createObjectUrl(state.thumbnailFile) : defaultThumbnail"
                                        class="size-full"
                                    />
                                    <div class="absolute size-full flex flex-col items-center justify-center gap-3 bg-black/50 opacity-0 hover:opacity-100 transition">
                                        <UAvatar
                                            size="lg"
                                            icon="i-lucide-image"
                                        />

                                        <UButton
                                            :label="state.thumbnailFile ? 'Change image' : 'Replace image'"
                                            color="neutral"
                                            variant="outline"
                                            @click="open()"
                                        />

                                        <UButton
                                            v-if="state.thumbnailFile"
                                            label="Remove image"
                                            color="warning"
                                            variant="link"
                                            @click="removeFile()"
                                            class="underline"
                                        />
                                    </div>
                                </UFileUpload>
                            </UFormField>
                        </div>
                        <div class="col-span-2 space-y-4">
                            <UFormField label="Select file (Upload file only if use want to update)" name="projectFile">
                                <UFileUpload
                                    v-model="state.projectFile"
                                    accept="zip/*"
                                    position="inside"
                                    layout="list"
                                    label="Drop your file here"
                                    :interactive="false"
                                    class="mt-2"
                                >
                                    <template #actions="{ open }">
                                        <UButton
                                            label="Upload project"
                                            icon="i-lucide-upload"
                                            color="neutral"
                                            variant="outline"
                                            @click="open()"
                                        />
                                    </template>
                                </UFileUpload>
                            </UFormField>

                            <UFormField label="Title" name="title">
                                <UInput v-model="state.title" class="w-full mt-2" />
                            </UFormField>

                            <UFormField label="Description" name="description">
                                <UTextarea
                                    v-model="state.description"
                                    class="w-full mt-2"
                                    autoresize
                                />
                            </UFormField>

                            <UFormField label="Category" name="category">
                                <USelect v-model="state.category" :items="categories" class="w-full mt-2" />
                            </UFormField>
                        </div>
                    </div>
                    <UButton type="submit" class="mt-4" :loading="loading">
                        Upload
                    </UButton>
                </UForm>
            </UCard>
        </template>
    </UPage>
</template>