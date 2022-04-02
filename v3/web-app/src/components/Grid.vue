<script setup>
    import GridItem from './GridItem.vue';
    import { storeToRefs } from "pinia";
    import { useInventoryStore } from '../stores/inventory';

    const store = useInventoryStore();
    const { items } = storeToRefs(store);
</script>
<template> 
    <div id="grid" >
        <GridItem v-bind:key="item.id" v-for="item in items" :entry="item" />
    </div>
</template>