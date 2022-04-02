<script setup>
import DatePicker from './DatePicker.vue';
import Dialog from 'primevue/dialog';
import Login from './Login.vue';
import Register from './Login.vue';
import TextEntry from './Login.vue';

import { storeToRefs } from "pinia";
import { useStore } from '../../stores';
const store = useStore();
const { dialog } = storeToRefs(store);

function getDialogComponent() {
    switch(this.dialog.component)
    {
        case "DatePicker":
            return DatePicker;
        case "Login":
            return Login;
        case "Register":
            return Register;
        case "TextEntry":
            return TextEntry;
    }
}

</script>
<template>
    <Dialog :header="dialog.title" v-model:visible="dialog.visible">
      <component :is="getDialogComponent()" />
    </Dialog>
</template>
