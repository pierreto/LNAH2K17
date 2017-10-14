import { User } from './../user';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SignupService {

    constructor(private http: Http) {}

    signup(user: User): Observable<boolean> {
        const body = {
            Username: user.Username,
            Password: user.Password
        };

        return this.http.post('/api/signup', body).map(
            (res) => res.json()
        );
    }
}
