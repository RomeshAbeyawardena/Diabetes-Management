<script setup>
import Authenticated from './Authenticated.vue';
import Guest from './Guest.vue';
import Sidebar from 'primevue/sidebar';

import { storeToRefs } from "pinia";
import { useStore } from '../../stores/main';
import { onBeforeMount } from 'vue-demi';
import { DialogDef } from '../../models/Dialogs';
import { DialogType } from '../../models';
const store = useStore();
const { sideBar } = storeToRefs(store);

function getSideBarComponent() {
    return sideBar.value.component;
}

onBeforeMount(() => {
     store.addDialog(
         new DialogDef(DialogType.Authenticated, "authenticated-sidebar",
         "Authenticated", Authenticated))
         .addDialog(
         new DialogDef(DialogType.Guest, "guest-sidebar",
         "Guest", Authenticated));
});

</script>
<template>
    <Sidebar v-model:visible="sideBar.visible">
      <component :is="getSideBarComponent()" />
    </Sidebar>
</template>
