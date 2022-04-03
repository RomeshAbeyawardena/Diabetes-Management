<script setup>
    import GridItem from './GridItem.vue';
    import { computed } from 'vue';
    import { useInventoryStore } from '../stores/inventory';
    import { storeToRefs } from 'pinia';

    const store = useInventoryStore();
    const { isDeleteMode } = storeToRefs(store);
    const items = computed(() => store.currentDateItems);
</script>
<template> 
    <div id="grid">
        {{ isDeleteMode }}
        <div v-bind:key="item.id" v-for="item in items">
            <GridItem :is-delete-mode="isDeleteMode" :entry="item" />
        </div>
    </div>
</template>