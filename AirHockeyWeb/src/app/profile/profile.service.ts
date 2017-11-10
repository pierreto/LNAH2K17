import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Profile } from './profile';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class ProfileService {
    constructor(private http: Http) {}

    getProfiles(): Observable<Profile[]> {
        return this.http.get('/api/profile').map(res => res.json());
    }
}
