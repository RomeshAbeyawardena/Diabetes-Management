import dayjs from "dayjs";

export interface IDateRange {
    fromDate: Date;
    toDate: Date;
    useStartAndEndOfTime: boolean;
    add(value:number, unit:string) : IDateRange;
    subtract(value:number, unit:string) : IDateRange;
    display(format: string, showFromDate: boolean, showToDate: boolean) : string;
}

export class DateRange implements IDateRange {
    fromDate: Date;
    toDate: Date;
    useStartAndEndOfTime: boolean;

    constructor(fromDate: Date, toDate: Date, useStartAndEndOfTime: boolean) {
        this.fromDate = fromDate;
        this.toDate = toDate;
        this.useStartAndEndOfTime = useStartAndEndOfTime;

        if(useStartAndEndOfTime) {
            let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);

            this.fromDate = fromDate.startOf("date").toDate();
            this.toDate = toDate.endOf("date").toDate();
        }
    }

    add(value: number, unit: string): IDateRange {
        let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);
            
            let newFromDate = fromDate.add(value, unit).toDate();
            let newToDate = toDate.add(value, unit).toDate();

            let dateRange = new DateRange(
                newFromDate,
                newToDate,
                this.useStartAndEndOfTime);
            
            return dateRange;
    }

    display(format: string, showFromDate: boolean, showToDate: boolean): string {
        let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);
            let display = "";

            if(showFromDate) {
                display += fromDate.format(format);
            }

            if(showFromDate && showToDate) {
                display += " - "
            }
            
            if(showToDate) {
                display += toDate.format(format);
            }

            return display;
    }

    subtract(value: number, unit: string): IDateRange {
        let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);
            
            let newFromDate = fromDate.subtract(value, unit).toDate();
            let newToDate = toDate.subtract(value, unit).toDate();

            let dateRange = new DateRange(
                newFromDate,
                newToDate,
                this.useStartAndEndOfTime);
            
            return dateRange;
    }
}