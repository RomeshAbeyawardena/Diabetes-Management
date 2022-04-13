<script setup>
import "../../scss/version-picker.scss"
import Dropdown from "primevue/dropdown";
import { onBeforeMount } from "vue-demi";
import { useInventoryStore } from "../../stores/Inventory";
import { computed, ref } from "vue";
import { storeToRefs } from "pinia";
import dayjs from "dayjs";

const dateFormat = "DD MMM yyyy hh:mmZ";
const store = useInventoryStore();
const { currentVersion } = storeToRefs(store);
const selectedInventory = ref(null);
const inventoryVersions = ref([]);
const loading = ref(false);
const placeholder = computed(() => {
    if(loading.value){
        return "Loading versions...";
    }

    return "Select version";
});
function formatDate(date, format)  {
    dayjs(date).format(format);
}


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
          placeholder="Select version"
        >
          <template #value="slotProps">
            <div>Version {{ slotProps.value.version }} - {{ formatDate(slotProps.value.created, dateFormat) }}</div>
          </template>
          <template #option="slotProps">
            <div>Version {{ slotProps.option.version }} - {{ formatDate(slotProps.option.created, dateFormat) }}</div>
          </template>
        </Dropdown>
      </div>
    </div>
  </div>
</template>