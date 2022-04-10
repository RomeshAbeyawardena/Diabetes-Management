import { IApiHelper } from "../plugins/ApiHelper";
import { IApiDefinition } from "./Definition";
import { IResponse, ApiBaseWithHeader } from "./index";

export interface IInventory {
    inventoryHistoryId: string;
    inventoryId: string;
    version: number;
    type: string;
    items: string;
    hash: string;
    created: Date;
}

export interface IGetRequest {
    key: string;
    type: string;
    userId: string;
}

export interface IPostRequest {
    items: string;
    type: string;
    key: string;
    userId: string;
}

export interface IInventoryApi {
    get(request: IGetRequest): Promise<IResponse<IInventory>>;
    post(request: IPostRequest): Promise<IResponse<IInventory>>;
}

export class InventoryApi extends ApiBaseWithHeader implements IInventoryApi {
    constructor(apiHelper: IApiHelper, apiDefinition: IApiDefinition) {
        super(apiHelper, apiDefinition)
    }

    async get(request: IGetRequest): Promise<IResponse<IInventory>> {
        const response = await this.client
            .get<IResponse<IInventory>>("inventory", {
                params: {
                    key: request.key,
                    type: request.type,
                    userId: request.userId
                }
            });

        return response.data;
    }

    async post(request: IPostRequest): Promise<IResponse<IInventory>> {
        const formData = this.apiHelper.ConvertToFormData(request);
        const response = await this.client.post<IResponse<IInventory>>("inventory", formData);
        return response.data;
    }
}