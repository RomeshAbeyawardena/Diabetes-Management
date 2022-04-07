<script setup>
    import Button from 'primevue/button';
    import { useStore } from '../stores/main';
    import { storeToRefs } from 'pinia';
    import { DialogType } from '../models';
    const store = useStore();
    const { sideBar } = storeToRefs(store);
    
    function expandSidebar(){
        sideBar.value.type = DialogType.Guest;
        store.showSidebar(store.getDialog(sideBar.value.type));
    }
</script>
<template>
    <div id="title" class="grid justify-content-center align-items-center">
        <div class="col-2">
            <Button icon="pi pi-bars"  v-on:click="expandSidebar"
                    class="p-button-rounded p-button-secondary" />
        </div>
        <div class="col-8"><h3>Diabetic Unit Manager</h3></div>
        <div class="col-2">
            <Button icon="pi pi-chart-pie" 
                    class="p-button-rounded p-button-secondary" />
        </div>
    </div>
</template>