import { IResponse } from "../models/response";
import { ISession, ISessionService } from "../models/session";
import { ILoginRequest } from "../models/user";
import { ApiClient } from "./api-client-base";

export const baseUrl = "user/login";
export class SessionService extends ApiClient implements ISessionService {
    async login(request: ILoginRequest): Promise<IResponse<ISession>> {
        const form = this.processForm(request);
        const response = await this.client.post(baseUrl, form);
        return JSON.parse(response.data);
    }
    
}