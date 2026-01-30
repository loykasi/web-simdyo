<script setup lang="ts">
import RequestOTPForm from './RequestOTPForm.vue';

const open = ref(false);
const email = ref("");
const isOpenLoginForm = ref(false);

function toLoginForm(value: string) {
  email.value = value;
  isOpenLoginForm.value = true;
}

function backToRequestForm() {
  isOpenLoginForm.value = false;
}

</script>
<template>
	<UPopover
		v-model:open="open"
		:content="{
			align: 'end'
		}"
        arrow
	>
		<UButton
			:label="$t('nav.login')"
			color="neutral"
			variant="ghost"
		/>
		<template #content>
      <LoginOTPForm
        v-if="isOpenLoginForm"
        :email="email"
        v-on:back-to-request-form="backToRequestForm"
      />
      <RequestOTPForm
        v-else
        :email="email"
        v-on:to-login-form="toLoginForm"
      />
		</template>
	</UPopover>
</template>

