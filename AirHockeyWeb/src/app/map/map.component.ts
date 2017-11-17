import { Component, OnInit , AfterViewInit} from '@angular/core';
import { MapService } from './map.service';
import { AppService } from '../app.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, AfterViewInit {
  map: any;
  constructor(private mapService: MapService, private route: ActivatedRoute, private appService: AppService) {
  }

  ngOnInit() {
    this.route.params.subscribe(param => {
      setTimeout(() => {
        this.map = null;
        this.appService.loading = true;
        this.getProfile();
      });
    });
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.appService.loading = true;
      this.getProfile();
    });
  }

  private getProfile() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.mapService.getMap(id).subscribe(
      (res) => {
        console.log(res);
        this.map = res;
        setTimeout(() => {
          this.appService.loading = false;
        });
      },
      (err) => {
        setTimeout(() => {
          this.appService.loading = false;
        });
        console.log(err);
      }
    );
  }
}
