export interface IRange {
    minimum: number;
    maximum: number;
}

export interface IRandomStringOptions {
    generateLowercaseCharacters?: boolean;
    generateUppercaseCharacters?: boolean;
    generateNumbers?: boolean;
    generateSymbols?: boolean;
}

export interface IStringHelper {
    generateRandomString(length: number, options?: IRandomStringOptions): string;
}

export class StringHelper implements IStringHelper {
    readonly lowerCasecharacterRange: IRange;
    readonly upperCasecharacterRange: IRange;
    readonly numberRange: IRange;
    readonly symbolRange: IRange;
    readonly defaultOptions: IRandomStringOptions;

    constructor() {
        this.lowerCasecharacterRange = { maximum: 122, minimum: 97 };
        this.upperCasecharacterRange = { maximum: 90, minimum: 65 };
        this.numberRange = { maximum: 57, minimum: 48 };
        this.symbolRange = { maximum: 47, minimum: 33 };
        this.defaultOptions = {
            generateLowercaseCharacters: true,
            generateUppercaseCharacters: true,
            generateNumbers: true,
            generateSymbols: false
        }
    }

    generate(min: number, max: number) {
        return Math.floor(Math.random() * (max - min + 1) + min);
    }

    buildRandomOptionGenerator(options: IRandomStringOptions): number {
        let optionNumber = 0;

        if (options.generateLowercaseCharacters) {
            optionNumber = 20;
        }

        if (options.generateUppercaseCharacters) {
            optionNumber = 40;
        }

        if (options.generateNumbers) {
            optionNumber = 80;
        }

        if (options.generateSymbols) {
            optionNumber = 160;
        }

        return optionNumber;
    }

    generateRandomString(length: number, options?: IRandomStringOptions): string {
        let result = "";
        if (options == undefined || options == null) {
            options = this.defaultOptions;
        }

        const optionPickerRange = this.buildRandomOptionGenerator(options);

        for (let index = 0; index < length; index++) {

            const optionPicked = this.generate(0, optionPickerRange);

            if (optionPicked <= 20) {
                //isLowercase
                result += String.fromCharCode(this.generate(this.lowerCasecharacterRange.minimum, this.lowerCasecharacterRange.maximum));
            }
            else if (optionPicked > 20 && optionPicked <= 40) {
                result += String.fromCharCode(this.generate(this.upperCasecharacterRange.minimum, this.upperCasecharacterRange.maximum));
            }
            else if (optionPicked > 40 && optionPicked <= 80) {
                result += String.fromCharCode(this.generate(this.numberRange.minimum, this.numberRange.maximum));
            }
            else {
                result += String.fromCharCode(this.generate(this.symbolRange.minimum, this.symbolRange.maximum));
            }
        }

        return  result;
    }
}