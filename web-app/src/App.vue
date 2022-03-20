<template>
  <div id="app">
    <TitleNavigation />
    <Dialog
      :header="dialog.header"
      :visible.sync="dialog.display"
      :modal="dialog.modal"
      :showHeader="dialog.header != null || dialog.header != undefined"
      v-on:show="dialogShow"
      v-on:hide="dialogHide"
    >
      <Calendar
        v-model="value"
        :inline="true"
        :show-time="dialog.showTime"
        v-on:date-select="dialogOptionSelected"
      />
    </Dialog>
    <confirm-popup />
    <toast position="center" />
    <date-navigation />
    <inventory-vue :items="filteredItems" />
  </div>
</template>

<script>
import Calendar from "primevue/calendar";
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
    Calendar,
    ConfirmPopup,
    DateNavigation,
    Dialog,
    InventoryVue,
    TitleNavigation,
    Toast,
  },
  data() {
    return {
      value: null,
      items: [
        {
          label: "Finder",
          icon: () => (
            <img
              alt="Finder"
              src="demo/images/dock/finder.svg"
              style="width: 100%"
            />
          ),
        },
        {
          label: "App Store",
          icon: () => (
            <img
              alt="App Store"
              src="demo/images/dock/appstore.svg"
              style="width: 100%"
            />
          ),
        },
        {
          label: "Photos",
          icon: () => (
            <img
              alt="Photos"
              src="demo/images/dock/photos.svg"
              style="width: 100%"
            />
          ),
        },
        {
          label: "Trash",
          icon: () => (
            <img
              alt="trash"
              src="demo/images/dock/trash.png"
              style="width: 100%"
            />
          ),
        },
      ],
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
      let dialog = this.dialog;
      dialog.value = e;
      dialog.display = false;
      this.$store.commit(Store.mutations.setDialogOptions, dialog);
      this.$root.$emit("dialog:optionSelected", dialog);
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