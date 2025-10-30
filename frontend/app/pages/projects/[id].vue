<script setup lang="ts">
const detailOpen = ref(false);

function ToggleMoreInformation() {
    detailOpen.value = !detailOpen.value;
}
</script>
<template>
    <UPage>
        <!-- <UPageHeader title="Project Title" /> -->
        <div class="flex justify-between my-4">
            <h1 class="text-3xl font-semibold">Project Title</h1>
            <div class="flex gap-x-3">
                <UButton size="xl" color="error">Report</UButton>
                <UButton size="xl">See inside</UButton>
            </div>
        </div>
        
        <div class="aspect-[16/9] w-full bg-blue-800 mb-4"></div>

        <UCard class="mt-4">
            <div class="flex items-center justify-between gap-x-3 w-full">
                <div class="flex items-center gap-x-4">
                    <UUser
                        name="John Doe"
                        size="lg"
                        :avatar="{
                            src: 'https://i.pravatar.cc/150?u=john-doe',
                            icon: 'i-lucide-image'
                        }"
                    />
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
                    <span>Description</span>
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
                                <div>
                                    Author
                                </div>
                                <div>
                                    Upload date
                                </div>
                                <div>
                                    Size
                                </div>
                            </div>
                        </template>
                    </UCollapsible>
                </div>
            </div>
        </UCard>
    </UPage>
</template>