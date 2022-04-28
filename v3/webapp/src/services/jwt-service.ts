import { ApiClient } from "./api-client-base";

export interface IJwtService {
    encode(payload: any): Promise<string>;
    decode(token: string): any;
}
export const baseUrl = "util";

export class JwtService extends ApiClient implements IJwtService {
    
    async encode(payload: any) : Promise<string> {
        const response = await this.client.request({
            headers: {
                "Content-Type": "application/json"
            },
            url: baseUrl + "/encode",
            method: "POST",
            data: JSON.stringify(payload)
        });
        return JSON.parse(response.data).data;
    }

    async decode(token: string): Promise<any> {
        const form = this.processForm({ token: token, 
            parameters: { "requireExpirationTime": false } });
        const response = await this.client.post(baseUrl + "/decode", form);
        return JSON.parse(response.data).data;
    }
}