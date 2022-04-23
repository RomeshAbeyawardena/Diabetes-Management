import { IAccessTokenRequest, IAccessTokenService } from "../models/access-token";
import { IResponse } from "../models/response";
import { ApiClient } from "./api-client-base";

export const baseUrl = "access-token";

export class AccessTokenService extends ApiClient implements IAccessTokenService {
    async generate(request: IAccessTokenRequest): Promise<IResponse<string>> {
        const form = this.processForm(request);
        const response = await this.client.post(baseUrl, form);
        return JSON.parse(response.data);
    }

}