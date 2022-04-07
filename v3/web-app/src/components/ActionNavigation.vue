<script setup>
    import Button from 'primevue/button';
    import SpeedDial from 'primevue/speeddial';
    import Toast from 'primevue/toast';
    import { useStore } from '../stores/main';
    import { useInventoryStore } from '../stores/inventory';
    import { storeToRefs } from 'pinia';
    import { ref } from 'vue';
    import { useToast } from "primevue/usetoast";

    const toast = useToast();
    const store = useStore();
    const inventoryStore = useInventoryStore();
    const { blockEvents } = storeToRefs(store);
    const { isDeleteMode } = storeToRefs(inventoryStore);
    let optionsMenuItems = ref([ 
        { label: "Version", icon: "pi pi-sign-in", command: () => register()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  }
    ]);

    let addMenuItems = ref([
        { label: "Reset", icon: "pi pi-refresh", command: () => reset()  },
        { label: "Delete mode", icon: "pi pi-trash", command: () => toggleDeleteMode()  },
        { label: "Save", icon: "pi pi-save", command: () => save()  },
        { label: "Add", icon: "pi pi-plus", command: () => add()  },
    ]); 
    
    function toggleDeleteMode() {
        isDeleteMode.value = !isDeleteMode.value;

        if(isDeleteMode.value)
        {
            toast.add({ severity:"info", summary: "Delete mode activated", detail: "Tap the same option again to turn this mode off", life: 5000 })

            addMenuItems.value[1].icon = "pi pi-times";
        }
        else {
            toast.add({ severity:"info", summary: "Delete mode deactivated", detail: "", life: 1500 })

            addMenuItems.value[1].icon = "pi pi-trash";
        }
    }

    async function save() {
        await inventoryStore.save();
        toast.add({ severity:"info", summary: "Items saved", detail: "Items have been saved locally successfully", life: 1500 })
    }

    function add() {
        let c = store.filters.dateRange.getDateWithCurrentTime();
        
        inventoryStore.addNew(c);
    }

    function reset() {

    }
</script>
<template>
    <Toast position="bottom-center" />
    <div id="action-navigation" class="grid justify-content-between">
        <div class="col-3 flex align-items-center justify-content-center">
            <SpeedDial  :transitionDelay="120"
                        :disabled="blockEvents"
                        :tooltipOptions="{'position':'left'}"
                        showIcon="pi pi-bars" 
                        hideIcon="pi pi-times"
                        :model="optionsMenuItems" />
        </div>
        <div class="col-6">
             
        </div>
        <div class="col-3 flex align-items-center justify-content-center">
            <SpeedDial  :disabled="blockEvents" :model="addMenuItems" />
        </div>
    </div>
</template>