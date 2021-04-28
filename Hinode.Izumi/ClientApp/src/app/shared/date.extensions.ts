export class DateExtensions {
    static createDateAsUTC(date) {
        return new Date(Date.UTC(
            date.getFullYear(),
            date.getMonth(),
            date.getDate(),
            date.getHours(),
            date.getMinutes(),
            date.getSeconds()));
    }

    static convertDateToUTC(date) {
        return new Date(
            date.getUTCFullYear(),
            date.getUTCMonth(),
            date.getUTCDate(),
            date.getUTCHours(),
            date.getUTCMinutes(),
            date.getUTCSeconds());
    }
}
