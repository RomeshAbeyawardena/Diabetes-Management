<script setup>
    import GridItem from './GridItem.vue';
    import { computed } from 'vue';
    import { useInventoryStore } from '../stores/inventory';

    const store = useInventoryStore();
    const items = computed(() => store.currentDateItems);
</script>
<template> 
    <div id="grid" >
        <GridItem v-bind:key="item.id" v-for="item in items" :entry="item" />
    </div>
</template>