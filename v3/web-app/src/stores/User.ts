import { defineStore } from 'pinia';
import { ILoginRequest, IRegisterRequest } from '../api/User';

export interface IUserStoreState {
    displayName: string
    userToken: string
}

export const useUserStore = defineStore('inventory', {
    state: (): IUserStoreState => ({
        displayName: null,
        userToken: null
    }),
    getters: {

    },
    actions: {
        async login(user: ILoginRequest) {
            const response = await this.userApi.login(user);
        },
        async register(user: IRegisterRequest) {
            const response = await this.userApi.register(user);
        }
    }
});