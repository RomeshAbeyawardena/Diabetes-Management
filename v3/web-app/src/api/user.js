import Api from "./index";

let api = null;
export default {
    api: null,
    baseUrl: "api/user",
    create(baseUrl, apiKey) {
        this.api = Api.create(baseUrl, {
            "x-api-key": apiKey
        }, 5000);

        api = this.api;
    },
    async register(emailAddress, displayName, password) {
        var formData = new FormData();
        
        formData.append("emailAddress", emailAddress);
        formData.append("displayName", displayName);
        formData.append("password", password);

        return await api.post(this.baseUrl, formData);
    },
    async login(emailAddress, password) {
        var formData = new FormData();
        
        formData.append("emailAddress", emailAddress);
        formData.append("password", password);

        return await this.api.post(this.baseUrl, formData);
    }
}