import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { MapModel } from '../map/map.model';

@Injectable()
export class MapsService {
    constructor(private http: Http) {}

    getMaps(): Observable<MapModel[]> {
        return this.http.get('/api/maps').map(res => res.json());
    }
}
