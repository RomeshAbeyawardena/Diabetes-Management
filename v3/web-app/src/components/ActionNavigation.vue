<script setup>
    import Button from 'primevue/button';
    import SpeedDial from 'primevue/speeddial';
    import { useInventoryStore } from '../stores/inventory';
    const store = useInventoryStore();

    let optionsMenuItems = [ 
        { label: "Version", icon: "pi pi-sign-in", command: () => register()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  },
        { label: "About", icon: "pi pi-sign-in", command: () => login()  }
    ];

    let addMenuItems = [
        { label: "Save", icon: "pi pi-save", command: () => register()  },
        { label: "Reset", icon: "pi pi-sign-in", command: () => login()  },
        { label: "Add", icon: "pi pi-plus", command: () => add()  },
    ]; 

    function add() {
        store.addNew();
    }
</script>
<template>
    <div id="action-navigation" class="grid justify-content-center">
        <div class="col-3 justify-content-center">
            <SpeedDial  showIcon="pi pi-bars" 
                        hideIcon="pi pi-times"
                        :model="optionsMenuItems" />
        </div>
        <div class="col-6">
             
        </div>
        <div class="col-3 justify-content-center">
            <SpeedDial :model="addMenuItems" />
        </div>
    </div>
</template>