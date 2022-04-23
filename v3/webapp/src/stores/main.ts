import { AxiosRequestConfig } from "axios";
import { defineStore } from "pinia";

export interface IMainState {
    accessTokenId?: string
    sessionId?: string
    userId?: string
}

export interface IMainActions {

}

export const useStore = defineStore('main', {
    state: ():IMainState => {
        return {
            accessTokenId: undefined,
            sessionId: undefined,
            userId: undefined
        }
    },
    actions: {
        init(): void {
            this.client.interceptors.request.use(this.setupHeaders);
        },
        setupHeaders(config: AxiosRequestConfig): AxiosRequestConfig {
            const headers: any = config.headers;
            headers["x-api-acc-token"] = this.accessTokenId; 
            return config;
        },
        async login() : Promise<void> {
            const response = await this.sessionService.login({
                emailAddress: "romesh.a@outlook.com",
                password: "e138llRA1787!"
            });

            console.log(response);
        },
        async authenticate() : Promise<void> {
            let response = await this.accessTokenService.generate({
                apiChallenge: "ZWM5NmI2YTZiNzY2NDk4MzlkMmY4YThjY2M4Mjk5OTI=",
                apiIntent: "diabetic.unit.manager",
                apiKey: "A51E16EC-F4C4-4377-2F5C-08DA256AC687",
                validate: true
            });

            this.accessTokenId = response.data;
        }
    }
});