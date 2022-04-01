<template>
  <Menu style="width: 100%" :model="getMenuItems" />
</template>
<script>
import Menu from "primevue/menu";
import { Inventory } from "../store/inventory";

export default {
  computed: {
    isSignedIn() {
      return !(this.userId === undefined || this.userId === null);
    },
    userId() {
      console.log(this.$store.state);
      return this.$store.state.User.userId;
    },
    getMenuItems() {
      let context = this;
      if (this.isSignedIn) {
        return [
          {
            label: "Welcome guest",
            icon: "pi pi-user",
            command() {
              context.login();
            },
          },
          {
            label: "Sync with server",
            icon: "pi pi-refresh",
            command() {
              context.sync();
            },
          },
        ];
      }

      return [
        {
          label: "Welcome guest",
          icon: "pi pi-user",
          command() {
            context.edit();
          },
        },
        {
          label: "Sign in",
          icon: "pi pi-lock",
          command() {
            context.login();
          },
        },
        {
          label: "Sign up",
          icon: "pi pi-send",
          command() {
            context.register();
          },
        },
      ];
    },
  },
  data() {
    return {
      items: [],
    };
  },
  components: {
    Menu,
  },
  methods: {
    login() {},
    register() {},
    edit() {},
    sync() {
      this.$store.dispatch(Inventory.actions.sync);
    },
  },
};
</script>