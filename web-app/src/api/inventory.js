import Api from "./index";

export default {
    api: null,
    create(baseUrl, apiKey) {
        this.api = Api.create(baseUrl, {
            "x-api-key": apiKey
        }, 5000);
    },
    baseUrl: "api/inventory",
    async get(userId, key, type, version) {
        return await this.api.get(this.baseUrl, { 
            params: { 
                userId, 
                key, 
                type, 
                version 
            } 
        });
    },
    async post(items) {
        var formData = new FormData();

        for (let v in items) {
            formData.append(v, items[v]);
        }

        return await this.api.post(this.baseUrl, formData);
    }

}