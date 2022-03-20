<template>
  <div id="app">
    <confirm-popup />
    <TitleNavigation />
    <Dialog
      :header="dialog.header"
      :visible.sync="dialog.display"
      :modal="dialog.modal"
      :showHeader="dialog.header != null || dialog.header != undefined"
      v-on:show="dialogShow"
      v-on:hide="dialogHide">
      <DialogCalendar :dialog="dialog"
        v-on:dialog-calendar:accepted="dialogOptionSelected"
      />
    </Dialog>
    
    <toast position="center" />
    <date-navigation />
    <inventory-vue :items="filteredItems" />
  </div>
</template>

<script>
import DialogCalendar from "./components/Dialog-calendar.vue";
import ConfirmPopup from "primevue/confirmpopup";
import TitleNavigation from "./components/Title-navigation.vue";
import { Inventory } from "./store/inventory";
import { Store } from "./store";
import InventoryVue from "./components/Inventory.vue";
import DateNavigation from "./components/Date-navigation.vue";
import Toast from "primevue/toast";
import { mapGetters } from "vuex";
import Dialog from "primevue/dialog";

import dayjs from "dayjs";
export default {
  name: "App",
  components: {
    ConfirmPopup,
    DateNavigation,
    Dialog,
    DialogCalendar,
    InventoryVue,
    TitleNavigation,
    Toast,
  },
  data() {
    return {
      value: null,
    };
  },
  computed: {
    dialog() {
      return this.$store.state.dialog;
    },
    ...mapGetters([Inventory.getters.filteredItems]),
  },
  methods: {
    dialogOptionSelected(e) {
      this.$root.$emit("dialog:optionSelected", e);
    },
    dialogShow() {
      this.value = this.dialog.value;
    },
    dialogHide() {
      let dialog = this.dialog;
      dialog.display = false;
      this.$store.commit(Store.mutations.setDialogOptions, dialog);
    },
  },
  created() {
    return this.$store.commit(Inventory.mutations.setFilters, {
      fromDate: dayjs().startOf("day").toDate(),
      toDate: dayjs().endOf("day").toDate(),
    });
  },
};
</script>