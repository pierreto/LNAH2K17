import { Injectable } from '@angular/core';

@Injectable()
export class AppService {
    private _loading: boolean;

    public get loading() {
        return this._loading;
    }

    public set loading(loading: boolean) {
        this._loading = loading;
    }
}
