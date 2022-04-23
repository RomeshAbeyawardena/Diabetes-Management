import { Axios, AxiosRequestConfig } from "axios";

export interface IApiClient {
    client: Axios;
}

export abstract class ApiClient implements IApiClient {
    client: Axios;
    constructor(client: Axios) {
        this.client = client;
    }

    processForm(request:any): FormData {
        const formData = new FormData();

        for(let r in request){
            const value = request[r];
            if(value) {
                formData.append(r, value);
            }
        }

        return formData;
    }
}