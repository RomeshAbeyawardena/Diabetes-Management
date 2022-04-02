<script setup>
    import Button from 'primevue/button';
    import SpeedDial from 'primevue/speeddial';
    import { useInventoryStore } from '../stores/inventory';
    import { onMounted } from 'vue';
    const store = useInventoryStore();

    let optionsMenuItems = [ 
        { label: "Version", icon: "pi pi-sign-in", command: () => register()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  }
    ];

    let addMenuItems = [
        { label: "Reset", icon: "pi pi-refresh", command: () => login()  },
        { label: "Delete mode", icon: "pi pi-times", command: () => login()  },
        { label: "Save", icon: "pi pi-save", command: () => register()  },
        { label: "Add", icon: "pi pi-plus", command: () => add()  },
    ]; 

    
    onMounted(async() => {
        await store.getLastId();
    });
    

    function add() {
        store.addNew();
    }
</script>
<template>
    <div id="action-navigation" class="grid">
        <div class="col-3">
            <SpeedDial  :transitionDelay="120" 
                        :tooltipOptions="{'position':'left'}"
                        showIcon="pi pi-bars" 
                        hideIcon="pi pi-times"
                        :model="optionsMenuItems" />
        </div>
        <div class="col-6">
             
        </div>
        <div class="col-3">
            <SpeedDial :model="addMenuItems" />
        </div>
    </div>
</template>