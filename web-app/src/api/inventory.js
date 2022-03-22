import Api from "./index";

export default {
    api: null,
    create(baseUrl, apiKey) {
        this.api = Api.create(baseUrl, {
            "x-api-key": apiKey
        }, 5000);
    },
    inventory: {
        baseUrl: "inventory",
        async get() {
            return await this.api.get(this.baseUrl);
        },
        async post(items) {
            return await this.api.post(this.baseUrl, items);
        }
    }
}