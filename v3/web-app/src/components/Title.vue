<script setup>
    import Button from 'primevue/button';
    import { useStore } from '../stores/main';
    import { storeToRefs } from 'pinia';
    import { DialogType } from '../models';
    const store = useStore();
    const { blockEvents, sideBar } = storeToRefs(store);
    
    function expandSidebar() {
        const component = store.getDialog(sideBar.value.type);
        if(component)
        {
        store.showSidebar(component);
        }
    }
</script>
<template>
    <div id="title" class="grid justify-content-center align-items-center">
        <div class="col-2">
            <Button icon="pi pi-bars" :disabled="blockEvents" @click="expandSidebar"
                    class="p-button-rounded p-button-secondary" />
        </div>
        <div class="col-8"><h3>Diabetic Unit Manager</h3></div>
        <div class="col-2">
            <Button :disabled="blockEvents" icon="pi pi-chart-pie" 
                    class="p-button-rounded p-button-secondary" />
        </div>
    </div>
</template>