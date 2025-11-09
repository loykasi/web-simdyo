<script setup lang="ts">
import { useAuthStore } from '~/stores/auth.store';
import type { ProjectsResponse } from '~/types/project.type';

const { user, isLoggedIn } = useAuthStore();
const route = useRoute();
const username = route.params.username as string;

const { getProfileDetail } = useAccount();
const { data: profile } = await useAsyncData("profile", () => getProfileDetail(username));
const { data: projectsResponse } = await useAsyncData(
	`${username}.projects`,
	() => useAPI<ProjectsResponse>(`projects/users/${username}`, {
		method: "GET",
	})
);

const isFetchTrash = ref(false);
const deletedProjects = ref<ProjectsResponse>();

type tabType = "all" | "trash";

const tab = ref<tabType>("all");

function toTrashTab() {
	tab.value = 'trash';
	if (!isFetchTrash.value) {
		isFetchTrash.value = true;
		useAPI<ProjectsResponse>("projects/users/trash", {
			method: "GET",
		})
		.then(res => {
			deletedProjects.value = res;
		})
		.catch(err => {
			console.log(err);
		})
	}
}

function toAllTab() {
	tab.value = 'all';
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
</script>
<template>
    <UPage>
        <UPageHeader title="Author Username" />

        <UCard class="mt-4">
			<table>
				<tbody>
					<tr>
						<td class="pe-8 py-1.5 font-medium text-default">Username</td>
						<td>{{ profile?.username }}</td>
					</tr>
					<tr>
						<td class="pe-8 py-1.5 font-medium text-default">Email</td>
						<td>{{ profile?.email }}</td>
					</tr>
					<tr>
						<td class="pe-8 py-1.5 font-medium text-default">Total projects</td>
						<td>{{ profile?.totalProject }}</td>
					</tr>
				</tbody>
			</table>
        </UCard>

		<div class="flex items-center justify-between gap-x-4 mt-8 border-b border-b-default pb-2">
			<h2 class="block text-3xl font-bold">Projects</h2>

			<div
				v-if="isLoggedIn && user?.username == profile?.username"
				class="flex items-center gap-x-2"
			>
				<UButton
					:color="getTabColor('all')"
					:variant="getTabVariant('all')"
					@click="toAllTab()"
				>
					All
				</UButton>
				<UButton
					:color="getTabColor('trash')"
					:variant="getTabVariant('trash')"
					@click="toTrashTab()"
				>
					Trash
				</UButton>
			</div>
		</div>
        
        <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-8">
			<ProjectCard
				v-if="tab === 'all'"
				v-for="project in projectsResponse?.projects"
				:project="project"
			/>

			<ProjectCard
				v-if="tab === 'trash'"
				v-for="project in deletedProjects?.projects"
				:project="project"
			/>
        </div>
    </UPage>
</template>