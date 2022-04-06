import dayjs from "dayjs";
import customParseFormat from "dayjs/plugin/customParseFormat";
dayjs.extend(customParseFormat);

dayjs.extend(customParseFormat)
export interface IDateHelper {
    dateRange(fromDate: Date, toDate: Date, useStartAndEndOfTime: boolean) : IDateRange;
    appendTime(date: Date, time: string, format: string) : Date;
    appendTimeFromDate(date: Date, time: Date) : Date;
}

export class DateHelper implements IDateHelper {

    dateRange(fromDate: Date, toDate: Date, useStartAndEndOfTime: boolean): IDateRange {
        return new DateRange(this, fromDate, toDate, useStartAndEndOfTime);
    }

    appendTimeFromDate(date: Date, time: Date): Date {
        let _date = dayjs(date);

        if(!_date.isValid()) {
            return date;
        }

        let timeSpan = dayjs(time);
        
        if(!timeSpan.isValid()) {
            return date;
        }

        let newHour = timeSpan.hour();
        let newMinute = timeSpan.minute();
        let newSecond = timeSpan.second();

        return _date
            .set("hour", newHour)
            .set("minute", newMinute)
            .set("second", newSecond).toDate();
    }

    appendTime(date: Date, time: string, format: string) : Date {
        let timeSpan = dayjs(time, format);
        
        if(!timeSpan.isValid()) {
            return date;
        }

        return this.appendTimeFromDate(date, timeSpan.toDate());
    }
}

export interface IDateRange {
    fromDate: Date;
    toDate: Date;
    useStartAndEndOfTime: boolean;
    add(value:number, unit:string) : IDateRange;
    subtract(value:number, unit:string) : IDateRange;
    display(format: string, showFromDate: boolean, showToDate: boolean) : string;
    getDateWithCurrentTime() : Date;
}

export class DateRange implements IDateRange {
    fromDate: Date;
    toDate: Date;
    useStartAndEndOfTime: boolean;
    dateHelper: IDateHelper;

    constructor(dateHelper: IDateHelper, fromDate: Date, toDate: Date, useStartAndEndOfTime: boolean) {
        this.fromDate = fromDate;
        this.toDate = toDate;
        this.useStartAndEndOfTime = useStartAndEndOfTime;
        this.dateHelper = dateHelper;

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

            let dateRange = new DateRange(this.dateHelper,
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

    getDateWithCurrentTime(): Date {
        return this.dateHelper.appendTimeFromDate(this.fromDate, new Date());
    }

    subtract(value: number, unit: string): IDateRange {
        let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);
            
            let newFromDate = fromDate.subtract(value, unit).toDate();
            let newToDate = toDate.subtract(value, unit).toDate();

            let dateRange = new DateRange(this.dateHelper,
                newFromDate,
                newToDate,
                this.useStartAndEndOfTime);
            
            return dateRange;
    }
}