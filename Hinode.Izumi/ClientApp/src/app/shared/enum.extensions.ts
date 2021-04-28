export class EnumEx {
    private constructor() {
    }

    static getNamesAndValues(e: any) {
        return EnumEx.getNames(e).map(n => ({ name: n, value: e[n] as string | number }));
    }

    static getNames(e: any) {
        return Object.keys(e).filter(k =>
            typeof e[k] === "number"
            || e[k] === k
            || e[e[k]]?.toString() !== k
        );
    }

    static getValues(e: any) {
        return EnumEx.getNames(e).map(n => e[n] as string | number);
    }
}
