<script setup>
    import Button from 'primevue/button';
</script>
<template>
    <div id="navigation" class="grid justify-content-center align-items-center">
        <div class="col-1">
            <Button icon="pi pi-angle-double-left" 
                    class="p-button-rounded p-button-outlined" />
        </div>
        <div class="col-10">
            <h4>2nd April 2022</h4>
        </div>
        <div class="col-1">
            <Button icon="pi pi-angle-double-right" 
                    class="p-button-rounded p-button-outlined" />
        </div>
    </div>
</template>