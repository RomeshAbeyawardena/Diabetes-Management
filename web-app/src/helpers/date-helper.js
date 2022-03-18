export default {
    getDate(date, secondaryDate) {
        return new Date(date.getFullYear(), 
        date.getMonth(), 
        date.getDate(),
        secondaryDate.getHours(),
        secondaryDate.getMinutes(),
        secondaryDate.getSeconds())
    }
}