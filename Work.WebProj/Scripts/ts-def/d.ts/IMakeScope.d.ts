interface IMakeScopeBase extends ng.IScope {

    InitData: any;
    
    Grid_Items: any;
    Detail_Items: any;

    firstpage: number;
    lastpage: number;
    nextpage: number;
    prevpage: number;

    TotalPage: number;
    NowPage: number;
    RecordCount: number;
    StartCount: number;
    EndCount: number;

    fd: any; //編輯欄位
    sd: any; //查詢欄位

    show_master_edit: boolean;
    edit_type: IEditType;
    grid_new_show: boolean;
    grid_del_show: boolean;
    grid_nav_show: boolean;
    check_del_value: boolean;

    JumpPage(page: number): void;
    JumpPageKey(): void;
    Init_Query(): void;
    ExpandSub($index: number): void;
    Master_Grid_Delete(): void;
    SelectAllCheckDel($event: any): void;
    Master_Submit(): void;
    Master_Edit_Close(): void;
    Master_Open_Modify($index: number): void;
    Master_Open_New(): void;
    Detail_Init(): void;
    typeHead_addressTW(val: string): any;
    typeHead_addressTW_Select(item: any, zip: string): void;
}  
interface INGScope extends ng.IScope {

    InitData: any;

    firstpage: number;
    lastpage: number;
    nextpage: number;
    prevpage: number;

    TotalPage: number;
    NowPage: number;
    RecordCount: number;
    StartCount: number;
    EndCount: number;
    show_master_edit: boolean;
    edit_type: IEditType;
    grid_new_show: boolean;
    grid_del_show: boolean;
    grid_nav_show: boolean;
    check_del_value: boolean;

    JumpPage(page: number): void;
    JumpPageKey(): void;
    Init_Query(): void;
    ExpandSub($index: number): void;
    Master_Grid_Delete(): void;
    SelectAllCheckDel($event: any): void;
    Master_Submit(): void;
    Master_Edit_Close(): void;
    Master_Open_Modify($index: number): void;
    Master_Open_New(): void;
    Detail_Init(): void;
}
interface IScopeM<T> extends INGScope {
    fd: T; //編輯欄位
    sd: any; //查詢欄位
    Grid_Items: T[];
}
interface IScopeMD<T, Ts> extends INGScope {
    fd: T; //編輯欄位
    fds: Ts;
    sd: any; //查詢欄位
    Grid_Items: T[];
    Detail_Items: Ts[];
}  