<script setup>
import "../../scss/version-picker.scss"
import Dropdown from "primevue/dropdown";
import { onBeforeMount } from "vue-demi";
import { useInventoryStore } from "../../stores/Inventory";
import { ref } from "vue";

const store = useInventoryStore();
const selectedInventory = ref(null);
const inventoryVersions = ref([]);

onBeforeMount(async () => {
  const versions = await store.loadVersions();
  inventoryVersions.value = versions;
});
</script>
<template>
  <div class="version-picker">
    <div class="field grid">
      <div class="col-2">
        <label style="margin-right: 0.5rem">Version:</label>
      </div>
      <div class="col-6">
        <Dropdown
          :filter="true"
          :showClear="true"
          v-model="selectedInventory"
          :options="inventoryVersions"
          optionLabel="version"
          placeholder="Select version"
        >
          <template #value="slotProps">
            <div>Version {{ slotProps.value }}</div>
          </template>
          <template #option="slotProps">
            <div>Version {{ slotProps.option.version }}</div>
          </template>
        </Dropdown>
      </div>
    </div>
  </div>
</template>