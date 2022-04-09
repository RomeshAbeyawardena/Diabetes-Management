export interface IQueryString {
    search: string;
    entries: Map<string, string[]>;
    length: number;
    get(key: string): string[];
    has(key: string): boolean;
    flatten(key: string): string;
}

export class QueryString implements IQueryString {
    search: string;
    entries: Map<string, string[]>;
    length: number;

    constructor(search: string) {
        this.entries = new Map<string, string[]>();
        this.search = search;

        const split = search.split(/[?|&]([a-zA-Z0-9_]+[=][a-zA-Z0-9_\/+=]+)/).filter(s => s != '');

        const items = split;
        this.length = 0;
        for(let item of items) {
            const mapItem = item.split("=");
            const key = mapItem[0];
            const value = mapItem[1];
            
            if(!this.has(key)) {
                this.entries.set(key, [value]);
                this.length++;
            }
            else {
                const val = this.get(key);
                val.push(value);
                this.entries.set(key, val);
            }
        }

        
    }
    has(key: string): boolean {
        return this.entries.has(key);
    }
    get(key: string) : string[] {
        return this.entries.get(key);
    }

    flatten(key: string): string {
        const val = this.get(key);
        if(val && val.length) {
            return val.join(",");
        }
    }
}