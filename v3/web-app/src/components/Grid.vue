<script setup>
    import Button from 'primevue/button';
    import GridItem from './GridItem.vue';
    import { storeToRefs } from "pinia";
    import { useInventoryStore } from '../stores/inventory';
    import { onMounted } from 'vue';

    const store = useInventoryStore();
    const { items } = storeToRefs(store);

    onMounted(async() => {
        await store.getLastId();
    });
    
    function addNew() {
        store.addNew();
    }  
</script>
<template> 
    <div v-bind:key="item.id" v-for="item in items">
        <GridItem :entry="item" />
    </div>
    <Button label="Add" v-on:click="addNew" />
</template>