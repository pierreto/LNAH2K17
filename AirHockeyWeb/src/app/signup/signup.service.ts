import { User } from './../user';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class SignupService {

    constructor(private http: Http) {}

    signup(user: User): Observable<number> {
        const body = {
            Username: user.Username,
            Name: user.Name,
            Email: user.Email,
            Password: user.Password
        };

        return this.http.post('/api/signup', body).map(
            (res) => res.json()
        );
    }
}
