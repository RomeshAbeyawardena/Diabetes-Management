import { isDate } from "@vue/shared";
import dayjs from "dayjs";

export class DateRange {
    constructor(fromDate, toDate, useStartAndEndOfTime) {
        if (!isDate(fromDate)) {
            fromDate = Date(fromDate);
        }

        if (!isDate(toDate)) {
            toDate = Date(toDate);
        }

        this.fromDate = fromDate;
        this.toDate = toDate;

        if(useStartAndEndOfTime) {
            let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);

            this.fromDate = fromDate.startOf("date").toDate();
            this.toDate = toDate.endOf("date").toDate();
        }

        this.display = (format, showFromDate, showToDate) => {
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

        this.add = (value, unit) => {
            let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);
            
            let newFromDate = fromDate.add(value, unit).toDate();
            let newToDate = toDate.add(value, unit).toDate();

            let dateRange = new DateRange(
                newFromDate,
                newToDate,
                this.useStartAndEndOfTime);
            
            return dateRange;
        };
        
        this.subtract = (value, unit) => {
            let fromDate = dayjs(this.fromDate);
            let toDate = dayjs(this.toDate);
            
            let newFromDate = fromDate.subtract(value, unit).toDate();
            let newToDate = toDate.subtract(value, unit).toDate();

            let dateRange = new DateRange(
                newFromDate,
                newToDate,
                this.useStartAndEndOfTime);
            
            return dateRange;
        };
    }
}