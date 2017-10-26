import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { User } from '../user';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class LoginService {
    constructor(private http: Http) {}


    login(user: User): any {
        const body = {
            Username: user.Username,
            Password: user.Password,
            LoginFromWebApp: true
        };

        return this.http.post('/api/login', body)
        .map(res => res.json());
    }
}
