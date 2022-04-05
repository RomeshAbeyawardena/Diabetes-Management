<script setup>
import Button from 'primevue/button';
import CookiePolicy from './CookiePolicy.vue';
import DatePicker from './DatePicker.vue';
import Dialog from 'primevue/dialog';
import Login from './Login.vue';
import NumberPicker from './NumberPicker.vue';
import Register from './Login.vue';
import TextEntry from './TextEntry.vue';

import { ref, onBeforeMount, markRaw } from 'vue';
import { DialogType, DialogDef } from '../../models/Dialogs';
import { storeToRefs } from "pinia";
import { useStore } from '../../stores';


const store = useStore();
const { dialog } = storeToRefs(store);
const value  = ref(dialog.value.value);

function valueUpdated(newValue) {
    value.value = newValue;
}

function getDialogComponent() {
    return markRaw(store.getDialog(dialog.value.component).component);
}

function acceptChanges() {
    store.setDialogValue(value.value)
}

function rejectChanges() {
    store.voidDialogValue();
}

onBeforeMount(() => {
    store.addDialog(
            new DialogDef(DialogType.CookiePolicy, "cookie-policy", "Cookie policy", CookiePolicy))
        .addDialog(
            new DialogDef(DialogType.DatePicker, "date-picker", "Select a date", DatePicker))
        .addDialog(
            new DialogDef(DialogType.NumberPicker, "number-picker", "Select a value", NumberPicker))
        .addDialog(
            new DialogDef(DialogType.TextEntry, "text-entry", "Select a value", TextEntry));
 
    console.log(store.dialogHelper);
})

</script>
<template>
    <Dialog :header="dialog.title" v-model:visible="dialog.visible">
        <component :is="getDialogComponent()" v-on:value:updated="valueUpdated" :value="dialog.value" />
        <div v-if="dialog.showControls" style="text-align:right">
            <Button v-on:click="acceptChanges" class="p-button-success" style="margin-right:1rem" label="Accept" icon="pi pi-check" />
            <Button v-on:click="rejectChanges" class="p-button-danger" label="Cancel" icon="pi pi-times" />
        </div>
    </Dialog>
</template>
