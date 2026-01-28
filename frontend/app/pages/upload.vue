<script setup lang="ts">
import * as z from 'zod';
import type { FormSubmitEvent } from '@nuxt/ui';
import { useProject } from '~/composables/useProject';
import JSZip from 'jszip';
import type { UploadProjectRequest, UploadProjectResponse } from '~/types/project.type';

const MAX_FILE_SIZE = 2 * 1024 * 1024 // 2MB
const MAX_PROJECT_FILE_SIZE = 15 * 1024 * 1024 // 15MB

const toast = useToast();
const { upload } = useProject();
const onUploadProcess = ref(false);
const thumbnailUploadProgress = ref(0);
const projectUploadProgress = ref(0);

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

async function onSubmit(event: FormSubmitEvent<schema>) {
  const payload: UploadProjectRequest = {
    title: event.data.title,
    shortDescription: event.data.shortDescription,
    description: event.data.description,
    category: event.data.category === "Default"
      ? null
      : event.data.category,
    projectLength: event.data.projectFile.size.toString(),
    thumbnailLength: event.data.thumbnailFile.size.toString()
  }

  onUploadProcess.value = true;
  upload(payload)
    .then((res) => {
      console.log(res);
      uploadFiles(res)
    })
    .catch((error) => {
      onUploadProcess.value = false;
      toast.add({
        title: "Failed",
        description: "Something wrong!",
        color: "error",
      })
    })
}

async function uploadFiles(data: UploadProjectResponse)
{
  if (!state.thumbnailFile || !state.projectFile) return;

  try {
    await Promise.all([
      uploadWithProgress(data.thumbnaiPresignedUrl, "image/png", state.thumbnailFile, thumbnailUploadProgress),
      uploadWithProgress(data.projectPresignedUrl, "application/x-simdyo", state.projectFile, projectUploadProgress)
    ]);

    toast.add({
      title: "Success",
      description: "Upload successful!",
      color: "success",
    })
  } catch (error) {
    toast.add({
      title: "Failed",
      description: "Something wrong!",
      color: "error",
    })
  } finally {
    onUploadProcess.value = false;
    navigateTo(`/projects/${data.publicId}`);
  }
}

async function uploadWithProgress(url: string, contentType: string, file: File, progress: globalThis.Ref<number, number>) {
  return new Promise((resolve, reject) => {
    const xhr = new XMLHttpRequest();
    xhr.open("PUT", url, true);
    xhr.setRequestHeader("Content-Type", contentType);

    xhr.upload.onprogress = (event) => {
      if (event.lengthComputable) {
        progress.value = (event.loaded / event.total) * 100;
      }
    }

    xhr.onload = () => {
      if (xhr.status >= 200 && xhr.status < 300) {
        resolve(xhr.response);
      } else {
        reject(new Error(`Upload failed with status ${xhr.status}`));
      }
    }

    xhr.onerror = () => reject(new Error("Network error occurred"));

    xhr.send(file);
  })
}

function createObjectUrl(file: File): string {
    return URL.createObjectURL(file)
}

// extract thumbnail from project file if exist
watch(
  () => state.projectFile,
  async () => {
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
                  size="md"
                  icon="i-lucide-image"
                />
                <UButton
                  :label="$t('upload.upload_thumbnail')"
                  color="neutral"
                  variant="outline"
                  @click="open()"
                />
              </div>
            </UFileUpload>
            <UProgress
              v-if="onUploadProcess"
              v-model="thumbnailUploadProgress"
              class="mt-1"
            />
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
            <UProgress
              v-if="onUploadProcess"
              v-model="projectUploadProgress"
              class="mt-1"
            />
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
          
        <UButton type="submit" class="mt-4" :loading="onUploadProcess">
          {{ $t('upload.upload') }}
        </UButton>
      </UForm>
    </UCard>
  </UPage>
</template>