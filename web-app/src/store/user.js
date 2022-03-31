import Vue from "vue";

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
        async login(context, login){
            //do login
            console.log(login);
            let data = {};
            context.commit(UserState.mutations.setAccount, data);
            context.commit(UserState.mutations.setUserId, data.userId);
        }
    }
}