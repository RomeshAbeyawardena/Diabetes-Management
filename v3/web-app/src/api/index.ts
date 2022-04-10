import { Axios, AxiosRequestConfig } from "axios";
import { IApiHelper } from "../plugins/ApiHelper";
import { IApiDefinition } from "./Definition";

export interface IApi {
    apiHelper: IApiHelper;
    client: Axios;
}


export interface IResponse<T> {
    data: T;
    statusCode: number;
    statusMessage: string;
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

export abstract class ApiBaseWithHeader extends ApiBase {
    constructor(apiHelper: IApiHelper, apiDefinition: IApiDefinition) {
        super(apiHelper, apiDefinition.baseUrl, config => config.headers = {
            "x-api-key": apiDefinition.apiKey
        });
    }
}
