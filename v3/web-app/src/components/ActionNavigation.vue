<script setup>
    import Button from 'primevue/button';
    import SpeedDial from 'primevue/speeddial';
    import { useStore } from '../stores';
    import { useInventoryStore } from '../stores/inventory';
    import { onMounted, ref } from 'vue';
    const store = useStore();
    const inventoryStore = useInventoryStore();

    let optionsMenuItems = ref([ 
        { label: "Version", icon: "pi pi-sign-in", command: () => register()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  }
    ]);

    let addMenuItems = ref([
        { label: "Reset", icon: "pi pi-refresh", command: () => login()  },
        { label: "Delete mode", icon: "pi pi-times", command: () => login()  },
        { label: "Save", icon: "pi pi-save", command: () => register()  },
        { label: "Add", icon: "pi pi-plus", command: () => add()  },
    ]); 
    
    onMounted(async() => {
        await inventoryStore.getLastId();
    });
    

    function add() {
        inventoryStore.addNew(store.filters.dateRange.fromDate);
    }
</script>
<template>
    <div id="action-navigation" class="grid justify-content-between">
        <div class="col-3 flex align-items-center justify-content-center">
            <SpeedDial  :transitionDelay="120" 
                        :tooltipOptions="{'position':'left'}"
                        showIcon="pi pi-bars" 
                        hideIcon="pi pi-times"
                        :model="optionsMenuItems" />
        </div>
        <div class="col-6">
             
        </div>
        <div class="col-3 flex align-items-center justify-content-center">
            <SpeedDial :model="addMenuItems" />
        </div>
    </div>
</template>