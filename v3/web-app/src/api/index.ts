import { Axios, AxiosRequestConfig } from "axios";
import { IApiHelper } from "../plugins/ApiHelper";

export interface IApi {
    apiHelper: IApiHelper;
    client: Axios;
}


export interface IResponse<T> {
    data: T;
    statusCode: string;
}


export abstract class ApiBase implements IApi {
    apiHelper: IApiHelper;
    client: Axios;
    constructor(apiHelper: IApiHelper, baseUrl: string,
        action: (config: AxiosRequestConfig) => void) {
        this.apiHelper = apiHelper;
        const requestConfig: AxiosRequestConfig = {
            baseURL: baseUrl
        }

        action(requestConfig);
        this.client = new Axios(requestConfig);
    }
}