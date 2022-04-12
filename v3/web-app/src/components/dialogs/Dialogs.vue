<script setup>
import Button from "primevue/button";
import CookiePolicy from "./CookiePolicy.vue";
import DatePicker from "./DatePicker.vue";
import Dialog from "primevue/dialog";
import LocalExport from "./LocalExport.vue";
import Login from "./Login.vue";
import NumberPicker from "./NumberPicker.vue";
import Register from "./Register.vue";
import TextEntry from "./TextEntry.vue";
import VersionPicker from "./VersionPicker.vue";

import { ref, onBeforeMount, markRaw } from "vue";
import { DialogDef } from "../../models/Dialogs";
import { DialogType } from "../../models";
import { storeToRefs } from "pinia";
import { useStore } from "../../stores/main";

const store = useStore();
const { dialog } = storeToRefs(store);
const value = ref(dialog.value.value);

function valueUpdated(newValue) {
  value.value = newValue;
}

function acceptChanges() {
  store.setDialogValue(value.value);
}

function rejectChanges() {
  store.voidDialogValue();
}

onBeforeMount(() => {
  store
    .addDialog(
      new DialogDef(
        DialogType.CookiePolicy,
        "cookie-policy",
        "Cookie policy",
        markRaw(CookiePolicy)
      )
    )
    .addDialog(
      new DialogDef(
        DialogType.DatePicker,
        "date-picker",
        "Select a date",
        markRaw(DatePicker)
      )
    )
    .addDialog(
      new DialogDef(
        DialogType.LocalExport,
        "local-export",
        "Share",
        markRaw(LocalExport)
      )
    )
    .addDialog(
      new DialogDef(
        DialogType.Login,
        "login",
        "Login to sync and share",
        markRaw(Login)
      )
    )
    .addDialog(
      new DialogDef(
        DialogType.NumberPicker,
        "number-picker",
        "Select a value",
        markRaw(NumberPicker)
      )
    )
    .addDialog(
      new DialogDef(
        DialogType.Register,
        "register-user",
        "Register",
        markRaw(Register)
      )
    )
    .addDialog(
      new DialogDef(
        DialogType.TextEntry,
        "text-entry",
        "Select a value",
        markRaw(TextEntry)
      ))
      .addDialog(
      new DialogDef(
        DialogType.VersionPicker,
        "version-picker",
        "Select a version to restore",
        markRaw(VersionPicker)
      )
    );
});
</script>
<template>
  <Dialog @hide="rejectChanges" :header="dialog.title" v-model:visible="dialog.visible">
    <component
      :is="dialog.component"
      @value:updated="valueUpdated"
      :value="dialog.value"
    />
    <div v-if="dialog.showControls" style="text-align: right">
      <Button
        @click="acceptChanges"
        class="p-button-success"
        style="margin-right: 1rem"
        label="Accept"
        icon="pi pi-check"
      />
      <Button
        @click="rejectChanges"
        class="p-button-danger"
        label="Cancel"
        icon="pi pi-times"
      />
    </div>
  </Dialog>
</template>
