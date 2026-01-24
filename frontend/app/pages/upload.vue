<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import { useProject } from '~/composables/useProject';
import type { ProjectCategory } from '~/types/projectCategory.type';
import JSZip from 'jszip';

const toast = useToast();
const { upload } = useProject();

const { data: categories, pending: categoryPending } = await useLazyAsyncData(
	"projectCategories",
	() => useAPI<string[]>(`projects/categories/all`, {
		method: "GET"
	})
);

watchEffect(() => {
    if (categories.value && categories.value[0] !== "Default") {
        categories.value = ["Default", ...categories.value];
    }
})

// const categories = ref(['Default', 'Game', 'Prototype', 'Simulation', 'Animation']);

const MAX_FILE_SIZE = 2 * 1024 * 1024 // 2MB
const MAX_PROJECT_FILE_SIZE = 15 * 1024 * 1024 // 15MB

const formatBytes = (bytes: number, decimals = 2) => {
  if (bytes === 0) return '0 Bytes'
  const k = 1024
  const dm = decimals < 0 ? 0 : decimals
  const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Number.parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i]
}

const schema = z.object({
    title: z.string('Required').min(0, 'Required'),
	description: z.string('Required').min(0, 'Required'),
    category: z.string().default(""),
    projectFile: z
        .instanceof(File, {
            message: 'Please select a file.'
        })
        .refine((file) => file.size <= MAX_PROJECT_FILE_SIZE, {
            message: `The project is too large. Please choose a project smaller than ${formatBytes(MAX_PROJECT_FILE_SIZE)}.`
        }),
    thumbnailFile: z
        .instanceof(File, {
            message: 'Please select a file.'
        })
        .refine((file) => file.size <= MAX_FILE_SIZE, {
            message: `The image is too large. Please choose an image smaller than ${formatBytes(MAX_FILE_SIZE)}.`
        })
})

type schema = z.output<typeof schema>

const state = reactive<Partial<schema>>({
  projectFile: undefined,
  thumbnailFile: undefined,
  category: "Default"
})

const loading = ref(false);

async function onSubmit(event: FormSubmitEvent<schema>) {
    const payload = new FormData();
    payload.append("title", event.data.title);
    payload.append("description", event.data.description);
    payload.append("projectFile", event.data.projectFile);
    payload.append("thumbnailFile", event.data.thumbnailFile);

    if (event.data.category !== "Default") {
        payload.append("category", event.data.category);
    }

    loading.value = true;
    upload(payload)
        .then((res) => {
            loading.value = false;
            navigateTo(`/projects/${res.publicId}`);
        })
        .catch((error) => {
            loading.value = false;
            toast.add({
                title: "Failed",
                description: "Something wrong!",
                color: "error",
            })
        })
}

function createObjectUrl(file: File): string {
    return URL.createObjectURL(file)
}

watch(
    () => state.projectFile,
    async () => {
        // if (oldState.projectFile === newState.projectFile) return;
        if (!state.projectFile) return;

        try {
            let zip = await JSZip.loadAsync(state.projectFile);
            
            let thumbnailEntry = zip.file("thumbnail.png");
            if (!thumbnailEntry) return;

            let thumbnailBlob = await thumbnailEntry.async("blob");
            const thumbnailFile = new File([thumbnailBlob], "thumbnail.png", {
                type: thumbnailBlob.type || "image/png",
                lastModified: thumbnailEntry.date.getTime()
            });

            state.thumbnailFile = thumbnailFile;

            console.log(thumbnailFile);
        } catch (error) {
            console.warn(`No thumbnail in file: ${state.projectFile.name}`);
        }    
    }
)

useHead({
  title: 'Upload',
})

definePageMeta({
    middleware: ['authenticated']
})
</script>
<template>
    <UPage>
        <UPageHeader :title="$t('upload')" />

        <UCard class="mt-4">
            <UForm :schema="schema" :state="state" @submit="onSubmit">
                <div class="grid grid-cols-3 space-x-12 space-y-4">
                    <div>
                        <UFormField :label="$t('upload.thumbnail')" name="thumbnailFile">
                            <UFileUpload
                                v-model="state.thumbnailFile"
                                accept="image/*"
                                v-slot="{ open, removeFile }"
                                class="relative aspect-square mt-2 flex flex-col items-center justify-center bg-default border border-default rounded-lg focus-visible:outline-2"
                            >
                                <img
                                    v-if="state.thumbnailFile"
                                    :src="createObjectUrl(state.thumbnailFile)"
                                    class="size-full rounded-md"
                                />
                                <div
                                    v-if="state.thumbnailFile"
                                    class="absolute size-full flex flex-col items-center justify-center gap-3 rounded-lg transition bg-black/50 opacity-0 hover:opacity-100"
                                >
                                    <UButton
                                        label="Change image"
                                        color="neutral"
                                        variant="outline"
                                        @click="open()"
                                    />

                <UButton
                  label="Remove image"
                  color="warning"
                  variant="link"
                  @click="removeFile()"
                  class="underline"
                />
              </div>
                <div
                  v-else
                  class="absolute size-full flex flex-col items-center justify-center gap-3 rounded-lg"
                >
                  <UAvatar
                    size="lg"
                    icon="material-symbols:upload"
                  />

                                    <UButton
                                        :label="$t('upload.upload_thumbnail')"
                                        color="neutral"
                                        variant="outline"
                                        @click="open()"
                                    />
                                </div>
                            </UFileUpload>
                        </UFormField>
                    </div>
                    <div class="col-span-2 space-y-4">
                        <UFormField :label="$t('upload.file')" name="projectFile">
                            <UFileUpload
                                v-model="state.projectFile"
                                icon="material-symbols:upload"
                                accept=".simdyo"
                                position="inside"
                                layout="list"
                                label="Drop your file here"
                                :interactive="false"
                                class="mt-2"
                            >
                                <template #actions="{ open }">
                                    <UButton
                                        :label="$t('upload.upload_file')"
                                        icon="material-symbols:upload"
                                        color="neutral"
                                        variant="outline"
                                        @click="open()"
                                    />
                                </template>
                            </UFileUpload>
                        </UFormField>

                        <UFormField :label="$t('upload.title')" name="title">
                            <UInput v-model="state.title" class="w-full mt-2" />
                        </UFormField>

                        <UFormField :label="$t('upload.description')" name="description">
                            <UTextarea
                                v-model="state.description"
                                class="w-full mt-2"
                                autoresize
                            />
                        </UFormField>

                        <UFormField :label="$t('upload.category')" name="category">
                            <USelect
                                v-model="state.category"
                                :items="categories"
                                :loading="categoryPending"
                                class="w-full mt-2"
                            >
                                <template #default="{ modelValue }">
                                    {{ modelValue }}
                                </template>
                                <template #item-label="{ item }">
                                    {{ $t(item) }}
                                </template>
                            </USelect>
                        </UFormField>
                    </div>
                </div>
                <UButton type="submit" class="mt-4" :loading="loading">
                    {{ $t('upload.upload') }}
                </UButton>
            </UForm>
        </UCard>
    </UPage>
</template>