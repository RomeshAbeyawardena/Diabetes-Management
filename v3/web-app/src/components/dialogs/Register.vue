<script setup>
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import Message from "primevue/message";

import { computed, ref } from "vue";
import { useStore } from "../../stores/main";
import { useUserStore } from "../../stores/User";
import { useToast } from "primevue/usetoast";

const toast = useToast();
const props = defineProps({ value: Number });
const emit = defineEmits(["value:updated"]);
const value = ref(props.value);
const displayName = ref("");
const emailAddress = ref("");
const password = ref("");
const confirm = ref("");

const messages = ref([]);
const store = useStore();
const userStore = useUserStore();
const isLoading = ref(false);
const icon = computed(() => {
  if (isLoading.value) {
    return "pi pi-spin pi-spinner";
  }
  return null;
});

const loginLabel = computed(() => {
  if (isLoading.value) {
    return "";
  }
  return "Register";
});

function dismiss() {
  store.resetDialog();
}

function validate() {
  if (!displayName.value.length || displayName.value.length < 3) {
    throw "Display name must not be empty";
  }

  if (!emailAddress.value.length || emailAddress.value.length < 3) {
    throw "Email address must not be empty";
  }

  if (
    !emailAddress.value
      .trim()
      .match(
        /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/
      )
  ) {
    throw "Email address must be a valid e-mail address";
  }

  if (!password.value.length) {
    throw "Password must not be empty";
  }

  if (!confirm.value.length) {
    throw "Password must not be empty";
  }

  if (confirm.value != password.value) {
    throw "Passwords must match";
  }
}

async function register() {
  try {
    validate();
    isLoading.value = true;
    await userStore.register({
      emailAddress: emailAddress.value.trim(),
      password: password.value.trim(),
      displayName: displayName.value.trim(),
    });
    isLoading.value = false;
    store.resetDialog();

    toast.add({
      severity: "success",
      summary: "Registration successful you have been signed in",
      detail: "",
      life: 2000,
    });
  } catch (err) {
    messages.value.push({ severity: "warn", content: err, life: 3000 });
    isLoading.value = false;
  }
}
</script>
<template>
  <Message
    v-for="message of messages"
    :life="message.life"
    :sticky="false"
    :severity="message.severity"
    :key="message.content"
  >
    {{ message.content }}
  </Message>
  <div class="field">
    <span class="p-input-icon-left" style="width: 100%">
      <i class="pi pi-at" />
      <InputText
        autocomplete="email"
        placeholder="Display name"
        aria-placeholder="Display name"
        type="text"
        style="width: 100%"
        v-model="displayName"
      />
    </span>
  </div>
  <div class="field">
    <span class="p-input-icon-left" style="width: 100%">
      <i class="pi pi-at" />
      <InputText
        autocomplete="email"
        placeholder="Email Address"
        aria-placeholder="Email address"
        type="text"
        style="width: 100%"
        v-model="emailAddress"
      />
    </span>
  </div>
  <div class="field">
    <span class="p-input-icon-left" style="width: 100%">
      <i class="pi pi-key" />
      <InputText
        type="password"
        placeholder="Password"
        v-model="password"
        style="width: 100%"
      />
    </span>
  </div>
  <div class="field">
    <span class="p-input-icon-left" style="width: 100%">
      <i class="pi pi-key" />
      <InputText
        autocomplete="email"
        type="password"
        placeholder="Confirm password"
        v-model="confirm"
        style="width: 100%"
      />
    </span>
  </div>
  <div style="text-align: right">
    <Button label="Cancel" @click="dismiss()" style="margin-right: 1rem"></Button>
    <Button :label="loginLabel" :disabled="isLoading" :icon="icon" @click="register()"></Button>
  </div>
</template>
