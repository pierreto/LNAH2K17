import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { MapModel } from '../map/map.model';
import { ActivatedRoute } from '@angular/router';

@Injectable()

export class MapService {
    constructor(private http: Http, private route: ActivatedRoute) {}

    public getMap(id: number): Observable<MapModel> {
        return this.http.get('/api/maps/get/' + id).map(res => res.json());
    }
}
