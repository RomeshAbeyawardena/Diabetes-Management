<script setup>
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import Message from "primevue/message";
import { useToast } from "primevue/usetoast";
import { computed, ref } from "vue";
import { useUserStore } from "../../stores/User";
import { useStore } from "../../stores/main";

const toast = useToast();
const props = defineProps({ value: Number });
const emit = defineEmits(["value:updated"]);
const value = ref(props.value);
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
  return "Login";
});


const emailAddress = ref("");
const password = ref("");
const messages = ref([]);
const store = useStore();
const userStore = useUserStore();

function dismiss() {
  store.resetDialog();
}

function validate() {
  if (!emailAddress.value.length || emailAddress.value.length < 3) {
    throw "Email address must not be empty";
  }

  if (
    !emailAddress.value.match(
      /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/
    )
  ) {
    throw "Email address must be a valid e-mail address";
  }

  if (!password.value.length) {
    throw "Password must not be empty";
  }
}

async function login() {
  try {
    validate();
    isLoading.value = true;
    await userStore.login({
      emailAddress: emailAddress.value,
      password: password.value,
    });
    isLoading.value = false;
    store.resetDialog();

    toast.add({
      severity: "success",
      summary: "Login successful",
      detail: "",
      life: 1500,
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
  <div style="text-align: right">
    <Button
      label="Cancel"
      @click="dismiss()"
      style="margin-right: 1rem"
    ></Button>
    <Button
      :label="loginLabel"
      :icon="icon"
      :disabled="isLoading"
      @click="login()"
    ></Button>
  </div>
</template>
