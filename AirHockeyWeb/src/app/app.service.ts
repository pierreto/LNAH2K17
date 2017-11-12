import { Injectable } from '@angular/core';

@Injectable()
export class AppService {
    private _loading: boolean;
    private _loggedIn: boolean;
    private _loginPage: boolean;
    private _id: number;

    public get id() {
        return Number(localStorage.getItem('id'));
    }

    public set id(id: number) {
        this._id = id;
    }

    public get loading() {
        return this._loading;
    }

    public set loading(loading: boolean) {
        this._loading = loading;
    }

    public get loginPage() {
        return this._loginPage;
    }

    public set loginPage(loginPage: boolean) {
        this._loginPage = loginPage;
    }

    public get loggedIn() {
        return localStorage.getItem('loggedIn') === 'true';
    }

    public set loggedIn(loggedIn: boolean) {
        this._loggedIn = loggedIn;
    }
}
