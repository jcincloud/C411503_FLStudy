declare module server {
    interface BaseEntityTable {
        edit_type: number;
        check_del: boolean;
        expland_sub: boolean;
    }
    interface i_Code {
        code: string;
        langCode: string;
        value: string;
    }
    interface CUYUnit {
        sign: string;
        code: string;
    }
    interface i_Lang extends BaseEntityTable {
        lang: string;
        area: string;
        memo: string;
        isuse: boolean;
        sort: any;
    }
    interface loginField {
        lang: string;
        account: string;
        password: string;
        img_vildate: string;
        rememberme: boolean;

    }
    interface AspNetRoles extends BaseEntityTable {
        Id: string;
        name: string;
        aspNetUsers: any[];
    }
    interface AspNetUsers extends BaseEntityTable {
        Id: string;
        email: string;
        emailConfirmed: boolean;
        passwordHash: string;
        securityStamp: string;
        phoneNumber: string;
        phoneNumberConfirmed: boolean;
        twoFactorEnabled: boolean;
        lockoutEndDateUtc: Date;
        lockoutEnabled: boolean;
        accessFailedCount: number;
        userName: string;
        department_id: number;
        aspNetRoles: server.AspNetRoles[];
        role_array: any;
    }
    interface JoinUsMail {
        name: string;
        tel: string;
        addr: string;
        email: string;
        interest: string;
    }
} 