<script setup>
import Button from 'primevue/button';
import DatePicker from './DatePicker.vue';
import Dialog from 'primevue/dialog';
import Login from './Login.vue';
import NumberPicker from './NumberPicker.vue';
import Register from './Login.vue';
import TextEntry from './TextEntry.vue';

import { DialogTypes } from '../../models/Dialogs';
import { storeToRefs } from "pinia";
import { useStore } from '../../stores';

const store = useStore();
const { dialog } = storeToRefs(store);

function valueUpdated(newValue) {
    dialog.value.value = newValue;
}

function getDialogComponent() {
    switch(dialog.value.component)
    {
        case DialogTypes.DatePicker:
            return DatePicker;
        case DialogTypes.Login:
            return Login;
        case DialogTypes.Register:
            return Register;
        case DialogTypes.TextEntry:
            return TextEntry;
        case DialogTypes.NumberPicker:
            return NumberPicker;
    }
}

function acceptChanges() {
    dialog.value.itemSubject.next(dialog.value.value);
}

function rejectChanges() {
    dialog.value.itemSubject.next("dialog.cancel");
}

</script>
<template>
    <Dialog :header="dialog.title" v-model:visible="dialog.visible">
        <component :is="getDialogComponent()" v-on:value:updated="valueUpdated" :value="dialog.value" />
        <div style="text-align:right">
            <Button v-on:click="acceptChanges" class="p-button-success" style="margin-right:1rem" label="Accept" icon="pi pi-check" />
            <Button v-on:click="rejectChanges" class="p-button-danger" label="Cancel" icon="pi pi-times" />
        </div>
    </Dialog>
    

    
</template>
