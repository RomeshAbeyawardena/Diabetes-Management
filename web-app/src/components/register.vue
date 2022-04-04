<template>
  <div class="card">
    <div class="field">
      <label for="displayName">Display name</label>
      <InputText v-model="displayName" id="displayName" type="text" class="inputfield w-full" />
    </div>
    <div class="field">
      <label for="emailAddress">Email Address</label>
      <InputText v-model="emailAddress" id="emailAddress" type="text" class="inputfield w-full" />
    </div>
    <div class="field">
      <label for="password">Password</label>
      <InputText v-model="password" id="password" type="password" class="inputfield w-full" />
    </div>
  </div>
</template>
<script>
import InputText from 'primevue/inputtext';

export default {
    components: {
        InputText
    },
    data() {
        return {
            displayName: "",
            emailAddress: "",
            password: ""
        }
    }
};
</script>