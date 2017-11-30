import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Ranking } from './ranking';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class RankingService {
    constructor(private http: Http) {}

    getRankings(): Observable<Ranking[]> {
        return this.http.get('/api/rankings').map(res => res.json());
    }
}
