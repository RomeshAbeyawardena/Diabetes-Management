<script setup>
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import Message from "primevue/message";
import { ref } from "vue";
import { useUserStore } from "../../stores/User";
import { useStore } from "../../stores/main";
const emailAddress = ref("");
const password = ref("");
const messages = ref([]);
const store = useStore();
const userStore = useUserStore();
async function login() {
  try {
    await userStore.login({
      emailAddress: emailAddress.value,
      password: password.value,
    });

    store.resetDialog();

  } catch(err) {
      console.log(err);
      messages.value.push({ severity: "warn", content: err, life:3000 })
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
    <Button label="Cancel" style="margin-right: 1rem"></Button>
    <Button label="Login" @click="login()"></Button>
  </div>
</template>