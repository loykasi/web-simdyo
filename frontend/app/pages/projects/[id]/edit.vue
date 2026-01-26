<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import { useProject } from '~/composables/useProject';
import type { ProjectResponse } from '~/types/project.type';

const route = useRoute();
const projectId = route.params.id as string;

const toast = useToast();
const { update } = useProject();

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
  title: z.string('Required').min(1, 'Required'),
  shortDescription: z.string('Required').min(1, 'Required'),
	description: z.string('Required').min(1, 'Required'),
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

function createObjectUrl(file: File): string {
    return URL.createObjectURL(file)
}

const loading = ref(false);

async function onSubmit(event: FormSubmitEvent<schema>) {
    const payload = new FormData();
    payload.append("title", event.data.title);
    payload.append("shortDescription", event.data.description);
    payload.append("description", event.data.description);

    if (event.data.category !== "Default") {
        payload.append("category", event.data.category);
    }

    if (event.data.projectFile !== undefined) {
        payload.append("projectFile", event.data.projectFile);
    }

    if (event.data.thumbnailFile !== undefined) {
        payload.append("thumbnailFile", event.data.thumbnailFile);
    }

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
        state.shortDescription = res.shortDescription;
        state.description = res.description;
        state.category = res.category ?? "Default";
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
<!-- <template>
    <UPage>
        <UPageHeader title="Edit" />

        <template v-if="projectPending">
        </template>
        <template v-else>
            <UCard class="mt-4">
                <UForm :schema="schema" :state="state" @submit="onSubmit">
                    <div class="grid grid-cols-3 space-x-12 space-y-4">
                        <div>
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
                        Save
                    </UButton>
                </UForm>
            </UCard>
        </template>
    </UPage>
</template> -->

<template>
  <UPage>
    <UPageHeader :title="$t('upload')" />
    <UCard class="mt-4">
      <UForm
        :schema="schema"
        :state="state"
        @submit="onSubmit"
        class="space-y-4"
      >
        <div class="flex space-x-4">
          <UFormField
            :label="$t('upload.thumbnail')"
            name="thumbnailFile"
            class=""
          >
            <UFileUpload
              v-model="state.thumbnailFile"
              accept="image/*"
              v-slot="{ open, removeFile }"
              class="relative aspect-square size-32 mt-2 flex flex-col items-center justify-center bg-default border border-default rounded-lg focus-visible:outline-2"
            >
              <img
                :src="state.thumbnailFile ? createObjectUrl(state.thumbnailFile) : defaultThumbnail"
                class="size-full"
              />
              <div class="absolute size-full flex flex-col items-center justify-center gap-3 bg-black/50 opacity-0 hover:opacity-100 transition">
                <UAvatar
                  size="md"
                  icon="i-lucide-image"
                />

                <UButton
                  :label="state.thumbnailFile ? 'Change' : 'Replace'"
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
        
          <UFormField
            :label="$t('upload.file')"
            name="projectFile"
            class="flex-1"
          >
            <UFileUpload
              v-model="state.projectFile"
              icon="material-symbols:upload"
              accept=".simdyo"
              position="inside"
              layout="list"
              :interactive="false"
              :ui="{
                root: 'mt-2 h-32',
                base: 'flex flex-col items-center justify-center bg-default border border-solid border-default rounded-lg focus-visible:outline-2',
                wrapper: 'p-0 gap-3',
                actions: 'm-0'
              }"
            >
              <template #actions="{ open }">
                <UButton
                  :label="$t('upload.upload_file')"
                  color="neutral"
                  variant="outline"
                  @click="open()"
                />
              </template>
            </UFileUpload>
          </UFormField>
        </div>

            <UFormField :label="$t('upload.title')" name="title">
              <UInput v-model="state.title" class="w-full mt-2" />
            </UFormField>

            <UFormField :label="$t('upload.shortDescription')" name="shortDescription">
              <UInput v-model="state.shortDescription" class="w-full mt-2" />
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

            <UFormField :label="$t('upload.description')" name="description">
              <UTextarea
                v-model="state.description"
                class="w-full mt-2"
                autoresize
              />
            </UFormField>
          
        <UButton type="submit" class="mt-4" :loading="loading">
          Save
        </UButton>
      </UForm>
    </UCard>
  </UPage>
</template>