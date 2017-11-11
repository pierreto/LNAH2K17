import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Ranking } from './ranking';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class RankingService {
    constructor(private http: Http) {}

    getProfiles(): Observable<Ranking[]> {
        return this.http.get('/api/profile').map(res => res.json());
    }
}
