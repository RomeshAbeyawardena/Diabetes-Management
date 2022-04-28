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
            let value = request[r];
            if(value) {
                if(typeof(value) == "object"){
                    value = JSON.stringify(value);
                }

                formData.append(r, value);
            }
        }

        return formData;
    }
}