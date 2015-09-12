declare enum InfoType {
    Info,
    Warnning,
    Error
}

interface IResultBase {
    result: boolean;
    message: string;
    hasData: boolean;
    data: any;
    infoType: InfoType
}

interface IResultData<T> extends IResultBase {
    data: T;
}