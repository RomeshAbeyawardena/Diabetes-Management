import { Axios, AxiosRequestConfig } from "axios";
import { IApiHelper } from "../plugins/ApiHelper";
import { IApiDefinition, IApiEndpointDefinition } from "./Definition";

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
    apiDefinition: IApiDefinition
    constructor(apiHelper: IApiHelper, apiDefinition: IApiDefinition) {
        super(apiHelper, apiDefinition.baseUrl, () => { });
        this.apiDefinition = apiDefinition;
    }

    findEndPoint(endpoint: string, method: string): IApiEndpointDefinition {
        const result = this.apiDefinition.endpoints.find(e => e.name == endpoint 
            && e.method == method);

        if(result) {
            return result;
        }

        throw 'Endpoint not found';
    }

    setApiKey(endpoint: string, method: string): void {
        const foundEndpoint = this.findEndPoint(endpoint, method);
        this.client.interceptors.request.use(config => { config.headers = { "x-functions-key" : foundEndpoint.apiKey }; return config; });
    }
}
