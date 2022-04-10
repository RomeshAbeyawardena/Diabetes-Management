import { defineStore } from 'pinia';
import { ILoginRequest, IRegisterRequest } from '../api/User';

export interface IUserStoreState {
    displayName: string
    userToken: string
}

export const useUserStore = defineStore('user', {
    state: (): IUserStoreState => ({
        displayName: null,
        userToken: null
    }),
    getters: {
        isAuthenticated() {
            return this.userToken !== null || this.userToken !== undefined;
        }
    },
    actions: {
        async login(user: ILoginRequest) {
            const response = await this.userApi.login(user);

            const resp = JSON.parse(response);
            
            if(resp.data)
            {
                this.userToken = resp.data.UserId;
                this.displayName = resp.data.DisplayName;
                return resp.data.UserId;
            }
            
            throw "Login failed: " + resp.statusMessage;
        },
        async register(user: IRegisterRequest) {
            const response = await this.userApi.register(user);
        }
    }
});