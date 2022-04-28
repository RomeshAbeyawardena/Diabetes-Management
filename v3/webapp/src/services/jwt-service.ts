import { ApiClient } from "./api-client-base";

export interface IJwtService {
    encode(payload: any): Promise<string>;
    decode(token: string): any;
}
export const baseUrl = "util";

export class JwtService extends ApiClient implements IJwtService {
    
    async encode(payload: any) : Promise<string> {
        const form = this.processForm(payload);
        const response = await this.client.post(baseUrl + "/encode", form);
        return response.data;
    }

    async decode(token: string): Promise<any> {
        const form = this.processForm({ token: token });
        const response = await this.client.post(baseUrl + "/decode", form);
        return response.data;
    }
}