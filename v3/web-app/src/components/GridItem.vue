<script setup>
    import Button from 'primevue/button';
    import InputNumber from 'primevue/inputnumber';
    import InputText from 'primevue/inputtext';
    import ResponsiveDateInput from './ResponsiveDateInput.vue';

    import { useConfirm } from "primevue/useconfirm";
    import { useStore } from '../stores';
    import { ref, computed, watch } from "vue";
    import { Inventory, State } from "../models/Inventory";
    import { DialogTypes, getTitle } from '../models/Dialogs';
    
    const store = useStore();

    const props = defineProps({ 
        isDeleteMode: Boolean,
        showHeader: Boolean, 
        entry: Inventory 
    });

    const showHeader = ref(props.showHeader);
    const localEntry = ref(props.entry);
    const dateFormat = ref("DD/MM/YYYY");
    const format = ref("DD/MM/YYYY HH:mm");
    const mobileFormat = ref("HH:mm");
    const inputFormat = ref("99/99/9999 99:99");

    const inputValue = computed({
        get() {
            let value = localEntry.value.value;
            if(value != null || value != undefined)
            {
                return value.toString();
            }

            return "";
        },
        set(value) {
            if((value != null || value != undefined) && !isNaN(value)) {
                localEntry.value.value = Number(value);
                touchEntry();
            }
        }
    });

     const confirm = useConfirm();

    function touchEntry() {
        if(localEntry.value.state === State.unchanged) {
            localEntry.value.state = State.modified;
        }
    }

    function markAsDeleted(event) {
         confirm.require({
                target: event.currentTarget,
                message: 'Are you sure you want to proceed?',
                icon: 'pi pi-exclamation-triangle',
                accept: () => {
                    localEntry.value.state = State.deleted;
                },
                reject: () => {
                    //callback to execute when user rejects the action
                }
            });
    }

    async function showDialog(component, value) {
        let result = await store.showDialog(component, getTitle(component), value);
        switch (component) {
            case DialogTypes.DatePicker:
                localEntry.value.inputDate = result;
                if(localEntry.value.inputDate !== result)
                {
                    touchEntry();
                }
                break;
            case DialogTypes.TextEntry:
                localEntry.value.description = result;
                
                if(localEntry.value.description !== result)
                {
                    touchEntry();
                }
                break;
            case DialogTypes.NumberPicker:
                localEntry.value.value = result;
                
                if(localEntry.value.value !== result)
                {
                    touchEntry();
                }
                break;
        }
    }

</script>

<template>
    <div>
        <div class="grid" v-if="showHeader">
            <div class="col-4">
                <label for="inputDate">Date</label>
            </div>
            <div class="col-6">
                <label for="description">Description</label>
            </div>
            <div class="col-2">
                <label v-if="!props.isDeleteMode" for="value">Value</label>
                <label v-if="props.isDeleteMode" for="value">Action</label>
            </div>
        </div>
        <div class="grid">
            <div class="col-4">
                    <ResponsiveDateInput    
                        id="inputDate" 
                        :format="format" 
                        :mobile-format="mobileFormat" 
                        :date-format="dateFormat"
                        v-on:input:click="showDialog(DialogTypes.DatePicker, localEntry.inputDate)"
                        v-model="localEntry.inputDate" />
            </div>
            <div class="col-6">
                <div class="p-inputgroup">
                    <InputText id="description" 
                        v-on:input="touchEntry" 
                        type="text" 
                        style="width: 100%"
                        v-model="localEntry.description" />
                    <Button icon="pi pi-pencil" v-on:click="showDialog(DialogTypes.TextEntry, localEntry.description)" class="p-button-primary"/>
                </div>
            </div>
            <div class="col-2">
                <Button icon="pi pi-trash" v-if="props.isDeleteMode"  v-on:click="markAsDeleted($event)"
                        class="p-button-rounded p-button-secondary">
                </Button>
                <InputText id="value" v-if="!props.isDeleteMode" v-model="inputValue" v-on:click="showDialog(DialogTypes.NumberPicker, localEntry.value)"
                            type="number" 
                            style="width: 100%" />
            </div>
        </div>
    </div>
</template>