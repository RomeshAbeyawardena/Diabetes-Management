import dayjs from "dayjs";

export interface ITimespan {
    value: number;
    unit: string;
}

export interface ICacheEntry {
    key: string,
    value: any,
    expires: Date
}

export interface ICacheHelper {
    get(cache: ICache, key: string) : any;
    set(cache: ICache, key: string, value: any) : void
}

export interface ICache {
    defaultExpiryTimespan: ITimespan;
    evict(): void;
    get(key: string) : ICacheEntry;
    set(key: string, entry : ICacheEntry): void;
}

export interface ICachePlugin {
    cache: ICache;
    cacheHelper: ICacheHelper;
}

export class Cache implements ICache {
    cacheMap: Map<string, ICacheEntry>;
    defaultExpiryTimespan: ITimespan;
    readonly defaultTimeSpan : ITimespan = { value: 15, unit: "minutes" };

    constructor(timeSpan?: ITimespan) {
        this.defaultExpiryTimespan = timeSpan ?? this.defaultTimeSpan;
    }

    evict(key?: string): void {

        if(key) {
            this.cacheMap.delete(key);
            return;
        }

        const date = new Date();
        this.cacheMap.forEach((i: ICacheEntry, key: string) => {
            if(i.expires < date) {
                this.cacheMap.delete(key);
            }
        });
    }

    get(key: string) : ICacheEntry {
        const entry = this.cacheMap.get(key);
        
        if(entry && entry.expires > new Date()) {
            return entry;
        }

        return null;
    }
    
    set(key: string, entry: ICacheEntry): void {
        this.cacheMap.set(key, entry);
        this.evict();
    }
}

export class CacheHelper implements ICacheHelper {
    get(cache: ICache, key: string) {
        const cacheEntry = cache.get(key);

        if(cacheEntry) {
            return cacheEntry.value;
        }

        return null;
    }
    set(cache: ICache, key: string, value: any): void {
        const date = dayjs();
        const timeSpan = cache.defaultExpiryTimespan;
        cache.set(key, {
            key: key,
            value: value,
            expires: date.add(timeSpan.value, timeSpan.unit).toDate()
        })
    }
}

export function useCacheEntryPlugin() : ICachePlugin {
    return {
        cacheHelper: new CacheHelper(),
        cache: new Cache()
    }
}