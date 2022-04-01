import Vue from "vue";
import userApi from "../api/user";

let element = document.getElementById("app");
let attributeValue = element.getAttribute("data-base-url");
userApi.create(attributeValue);

export const UserState = {
    state: {
        userId: "userId",
        account: "account",
    },
    mutations: {
        setUserId: "setUserId",
        setAccount: "setAccount"
    },
    actions: {
        login: "login"
    }
}

export default {
    state: {
        userId: null,
        account: null
    },
    mutations: {
        setUserId(state, userId) {
            Vue.set(state, UserState.state.userId, userId);
        },
        setAccount(state, account) {
            Vue.set(state, UserState.state.account, account);
        }
    },
    actions: {
        async login(context, login)
        {
            console.log(login);
            let data = await userApi.login(login.emailAddress, login.password);

            context.commit(UserState.mutations.setAccount, data);
            context.commit(UserState.mutations.setUserId, data.userId);
        }
    }
}