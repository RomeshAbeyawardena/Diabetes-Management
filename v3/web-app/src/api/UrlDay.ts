import { Axios } from "axios";

export interface IPostResponseData {
    id: string;
    alias: string;
    url: string;
    short_url: string;
    title: string;
    target_type?: string;
    country_target?: string;
    platform_target?: string;
    language_target?: string;
    rotation_target?: string;
    last_rotation?: string;
    disabled?: string;
    privacy?: string;
    privacy_password: string;
    password?: string;
    expiration_url?: string;
    expiration_clicks?: string;
    clicks?: number;
    user_id: number;
    space: string;
    domain?: string;
    pixels: string;
    ends_at?: number;
    created_at: Date;
    updated_at: Date;
}

export interface IPostResponse {
    data?: IPostResponseData;
    status: number;
}

export interface IPostRequest {
    url: string;
    alias?: string;
    space?: string;
}

export interface IUrlDayApi {
    post(request: IPostRequest) : Promise<IPostResponse>
}

export class UrlDayApi implements IUrlDayApi {
    axiosClient: Axios;

    constructor(apiUrl: string, apiKey: string) {
        this.axiosClient = new Axios({
            baseURL: apiUrl,
            headers: {
                Authorization: "Bearer " + apiKey
            }
        });
    }

    async post(request: IPostRequest): Promise<IPostResponse> {
        const formData = new FormData();
        formData.append("url", request.url);
        if(request.alias != undefined)
        {
            formData.append("alias", request.alias);
        }

        if(request.space != undefined)
        {
            formData.append("space", request.space);
        }

        const result = await this.axiosClient.post<IPostResponse>("links", formData);
        if(result.status == 200)
        {
            return result.data;
        }

        throw "Error";
    }

}