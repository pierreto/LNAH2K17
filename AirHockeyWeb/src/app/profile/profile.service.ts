import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import 'rxjs/add/operator/map';

@Injectable()
export class ProfileService {
    constructor(private http: Http, private route: ActivatedRoute) {

    }

    public getProfile(id: number): Observable<any> {
        return this.http.get('/api/profile/' + id).map(res => res.json());
    }
}
