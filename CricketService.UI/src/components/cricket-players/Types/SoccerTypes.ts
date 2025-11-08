export type StringModifiers = (str: string) => void;
export type StringArrayModifiers = (strArr: string[]) => void;
export type BooleanToggler = (is: boolean) => void;
export type GenericArrayModifiers<T> = (gen: T[]) => void;
export type GenericModifiers<T> = (gen: T) => void;