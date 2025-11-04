<script setup lang="ts">
import type { ProjectsResponse } from '~/types/project.type';

const route = useRoute();
const userName = route.params.userName as string;

const { getProfileDetail } = useAccount();
const { data: profile } = await useAsyncData("profile", () => getProfileDetail(userName));
const { data: projectsResponse } = await useAsyncData(
	`${userName}.projects`,
	() => useAPI<ProjectsResponse>(`projects/user/${userName}`, {
		method: "GET",
	})
);
</script>

<template>
    <UPage>
        <UPageHeader title="Author Username" />

        <UCard class="mt-4">
			<table>
				<tbody>
					<tr>
						<td class="pe-8 py-1.5 font-medium text-default">Username</td>
						<td>{{ profile?.userName }}</td>
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

        <h2 class="text-2xl font-bold mt-4">Projects</h2>
        <div class="mt-4 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-5 gap-8">
			<NuxtLink
				v-for="project in projectsResponse?.projects"
				:to="`/projects/${project.id}`"
				class="block rounded-lg overflow-hidden bg-default ring ring-default divide-y divide-default"
			>
				<img alt="placeholder" src="https://placehold.co/400" />

				<div class="p-4">
				<h3 class="text-lg block h-14 line-clamp-2 font-bold">{{ project.title }}</h3>
				<NuxtLink
					:to="`/profile/${project.userName}`"
					class="mt-1 text-sm text-blue-200 hover:underline"
				>{{ project.userName }}</NuxtLink>
				</div>
			</NuxtLink>
        </div>
    </UPage>
</template>