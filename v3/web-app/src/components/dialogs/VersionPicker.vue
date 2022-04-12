<script setup>
import MultiSelect from 'primevue/multiselect';
import { onMounted } from 'vue-demi';
import { useInventoryStore } from '../../stores/Inventory';
import { ref } from "vue";

const store = useInventoryStore();
const selectedInventory = ref(null);
const inventoryVersions = ref([]);

onMounted(async() => {
    inventoryVersions.value = await store.loadVersions();
})
</script>
<template>
    <label>Select a version</label>
    <MultiSelect v-model="selectedInventory" :options="inventories" optionLabel="version" placeholder="Select version" />
</template>