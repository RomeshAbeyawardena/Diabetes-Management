<script setup>
    import Button from 'primevue/button';
    import InputMask from 'primevue/inputmask';
    import InputNumber from 'primevue/inputnumber';
    import InputText from 'primevue/inputtext';
    import { useConfirm } from "primevue/useconfirm";

    import { ref, computed, watch } from "vue";
    import { Inventory, State } from "../models/Inventory";
    import dayjs from "dayjs";
    import customParseFormat from 'dayjs/plugin/customParseFormat';
    dayjs.extend(customParseFormat)

    const props = defineProps({ 
        isDeleteMode: Boolean,
        showHeader: Boolean, 
        entry: Inventory 
    });

    const showHeader = ref(props.showHeader);
    const localEntry = ref(props.entry);

    const format = "DD/MM/YYYY HH:mm";
    const inputFormat = "99/99/9999 99:99";
    const inputDate = computed({
        get() {
            return dayjs(localEntry.value.inputDate).format(format);
        },
        set(value) {
            let newDate = dayjs(value, format);
            if(newDate.isValid())
            {
                localEntry.value.inputDate = newDate.toDate();
                touchEntry();
            }
        }
    });

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
                <InputMask id="inputDate" style="width: 100%"
                        :mask="inputFormat"
                        v-model="inputDate" />
            </div>
            <div class="col-6">
                <InputText id="description" v-on:input="touchEntry" type="text" style="width: 100%"
                    v-model="localEntry.description" />
            </div>
            <div class="col-2">
                <Button icon="pi pi-trash" v-if="props.isDeleteMode"  v-on:click="markAsDeleted($event)"
                        class="p-button-rounded p-button-secondary">
                </Button>
                <InputText id="value" v-if="!props.isDeleteMode" v-model="inputValue" 
                            type="number" 
                            style="width: 100%" />
            </div>
        </div>
    </div>
</template>