export interface ICookieHelper {
    getCookies() : ICookie[];
    setCookie(cookie: ICookie) : void;
    getCookie(name: string, value: string) : ICookie;
}

export interface ICookie {
    name: string;
    value: string;
    expires: Date;
    toString() : string
}

export class Cookie implements ICookie {
    constructor(name: string, value: string, expires: Date) {
        this.name = name;
        this.value = value;
        this.expires = expires;
    }

    name: string;
    value: string;
    expires: Date;

    toString(): string {
        return this.name.concat("=", this.value).concat(";expires=" + this.expires);
    }
}

export class CookieHelper implements ICookieHelper {
    getCookies(): ICookie[] {
        let cookies = new Array<Cookie>();
        let entries = document.cookie.split("; ");
        for(let entry of entries) {
            let splitEntry = entry.split("=");
            cookies.push(this.getCookie(splitEntry[0], splitEntry[1]));
        }

        return cookies;
    }
    
    setCookie(cookie: ICookie) : void {
        console.log(cookie.toString());
        document.cookie = cookie.toString();
    }

    getCookie(name: string, value?: string): ICookie {
        if(value)
        {
            return new Cookie(name, value, undefined);
        }

        let cookies = this.getCookies();
        return cookies.find(c => c.name === name);
    }
    
    
}