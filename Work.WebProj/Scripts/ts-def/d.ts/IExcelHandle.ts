interface IExcelHandleBase extends IMakeScopeBase {
    ifrmeSrcPath: string;
    getFileInfo: {};
    uploadProgress: {};
    fileUpload: any;
    isSelect: boolean;
    isUpload: boolean;

    lastUploadTime: Date;
    countUpload: number;
}  
