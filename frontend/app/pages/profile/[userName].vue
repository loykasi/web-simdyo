<script setup lang="ts">
import UserProjects from "~/components/profile/UserProjects.vue";
import UserTrash from "~/components/profile/UserTrash.vue";
import { useAuthStore } from "~/stores/auth.store";

const { user, isLoggedIn } = useAuthStore();
const route = useRoute();
const username = route.params.username as string;

console.log(username);

const { getProfileDetail } = useAccount();
const { data: profile, pending: profilePending } = await useLazyAsyncData(
  `${username}`,
  () => getProfileDetail(username),
);

type tabType = "all" | "trash";

const tab = ref<tabType>("all");

function toTrashTab() {
  tab.value = "trash";
}

function toAllTab() {
  tab.value = "all";
}

function getTabColor(type: tabType) {
  if (tab.value === type) {
    return "primary";
  }
  return "neutral";
}

function getTabVariant(type: tabType) {
  if (tab.value === type) {
    return "solid";
  }
  return "outline";
}

const headTitle = computed(() =>
  profile.value ? profile.value.username : route.fullPath,
);
useHead({
  title: headTitle,
});
</script>
<template>
  <UPage>
    <template v-if="profilePending">
      <div class="h-[105px] border-b border-default py-8">
        <USkeleton class="w-40 h-full" />
      </div>
    </template>
    <template v-else>
      <UPageHeader :title="profile?.username" />
    </template>

    <UCard class="mt-4">
      <table>
        <template v-if="profilePending">
          <tbody>
            <tr>
              <td class="pe-8 py-1.5 font-medium text-default">
                {{ $t("profile.username") }}
              </td>
              <td><USkeleton class="w-20 h-6" /></td>
            </tr>
            <tr>
              <td class="pe-8 py-1.5 font-medium text-default">
                {{ $t("profile.email") }}
              </td>
              <td><USkeleton class="w-20 h-6" /></td>
            </tr>
            <tr>
              <td class="pe-8 py-1.5 font-medium text-default">
                {{ $t("profile.total_projects") }}
              </td>
              <td><USkeleton class="w-20 h-6" /></td>
            </tr>
          </tbody>
        </template>
        <template v-else>
          <tbody>
            <tr>
              <td class="pe-8 py-1.5 font-medium text-default">
                {{ $t("profile.username") }}
              </td>
              <td>{{ profile?.username }}</td>
            </tr>
            <tr>
              <td class="pe-8 py-1.5 font-medium text-default">
                {{ $t("profile.email") }}
              </td>
              <td>{{ profile?.email }}</td>
            </tr>
            <tr>
              <td class="pe-8 py-1.5 font-medium text-default">
                {{ $t("profile.total_projects") }}
              </td>
              <td>{{ profile?.totalProject }}</td>
            </tr>
          </tbody>
        </template>
      </table>
    </UCard>

    <div
      class="flex items-center justify-between gap-x-4 mt-8 border-b border-b-default pb-2"
    >
      <h2 class="block text-3xl font-bold">{{ $t("profile.projects") }}</h2>

      <div
        v-if="isLoggedIn && user?.username == profile?.username"
        class="flex items-center gap-x-2"
      >
        <UButton
          :color="getTabColor('all')"
          :variant="getTabVariant('all')"
          @click="toAllTab()"
        >
          {{ $t("profile.all") }}
        </UButton>
        <UButton
          :color="getTabColor('trash')"
          :variant="getTabVariant('trash')"
          @click="toTrashTab()"
        >
          {{ $t("profile.trash") }}
        </UButton>
      </div>
    </div>

    <UserProjects v-if="tab === 'all'" />
    <UserTrash v-if="tab === 'trash'" />
    <!-- <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-8">
			<template v-if="projectPagePending">
				<div
				v-for="item in pageSize"
				:key="item"
				class="rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
				>
					<USkeleton class="aspect-square w-full" />
					<div class="p-4 h-[84px]">
						<USkeleton class="size-full" />
					</div>
				</div>
			</template>
			<template v-else>
				<ProjectCard
					v-if="tab === 'all'"
					v-for="project in projectPage?.items"
					:project="project"
				/>

				<ProjectCard
					v-if="tab === 'trash'"
					v-for="project in trashPage?.items"
					:project="project"
				/>
			</template>
        </div> -->
  </UPage>
</template>
