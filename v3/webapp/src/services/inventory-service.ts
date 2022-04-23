import { IInventoryHistory, IInventoryRequest, IInventoryService } from "../models/inventory";
import { IResponse } from "../models/response";
import { ApiClient } from "./api-client-base";

export const baseUrl = "inventory";

export class InventoryService extends ApiClient implements IInventoryService {
    async save(request: IInventoryRequest): Promise<IResponse<IInventoryHistory>> {
        const form = this.processForm(request);
        const response = await this.client.post<IResponse<IInventoryHistory>>(baseUrl, form);
        return response.data;
    }
    async get(request: IInventoryRequest): Promise<IResponse<IInventoryHistory>> {
        const response = await this.client.get<IResponse<IInventoryHistory>>(baseUrl, { params: request });
        return response.data;
    }
    
}