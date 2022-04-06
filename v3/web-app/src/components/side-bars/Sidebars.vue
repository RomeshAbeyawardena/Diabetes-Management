<script setup>
import Authenticated from './Authenticated.vue';
import Guest from './Guest.vue';
import Sidebar from 'primevue/sidebar';

import { storeToRefs } from "pinia";
import { useStore } from '../../stores/main';
const store = useStore();
const { sideBar } = storeToRefs(store);

function getSideBarComponent() {
    switch(sideBar.value.component)
    {
        case "Authenticated":
            return Authenticated;
        case "Guest":
            return Guest;
    }
}

</script>
<template>
    <Sidebar v-model:visible="sideBar.visible">
      <component :is="getSideBarComponent()" />
    </Sidebar>
</template>
