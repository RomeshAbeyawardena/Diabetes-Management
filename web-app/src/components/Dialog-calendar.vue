<template>
    <div class="dialog-calendar">
        <Calendar v-model="value"
                v-on:date-select="dialogOptionSelected"
                :inline="true"
                :show-time="dialog.showTime" />
        <div class="p-buttonset align-centre mt-4">
            <Button label="Cancel"
                    v-on:click="reject" 
                    class="p-button-danger" 
                    icon="pi pi-times" />
            <Button label="Save" 
                    v-on:click="accept" 
                    class="p-button-success" 
                    icon="pi pi-check" />
        </div>
    </div>
</template>
<script>
import Button from "primevue/button";
import Calendar from "primevue/calendar";
import { Store } from "../store";

export default {
    components: {
        Button,
        Calendar
    },
    data() {
        return {
            oldValue: this.dialog.value,
            value: this.dialog.value
        }
    },
    methods: {
        setDialogValue(value, visible) {
            let dialog = this.dialog;
            dialog.value = value;
            if(visible !== null || visible !== undefined)
            {
                dialog.display = visible;
            }
            this.$store.commit(Store.mutations.setDialogOptions, dialog);
            return dialog;
        },
        accept() {
            let dialog = this.setDialogValue(this.value, false);
            this.$emit("dialog-calendar:accepted", dialog);
        },
        reject() {
            this.setDialogValue(this.oldValue, false);
            this.$emit("dialog-calendar:rejected", this.oldValue);
        },
        dialogOptionSelected(e) {
            let dialog = this.setDialogValue(e, true);
            this.$emit("dialog-calendar:selected", dialog);
        }
    },
    name: "dialog-calendar",
    props: {
        dialog: Object
    }
}
</script>