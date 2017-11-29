import { Component, OnInit , AfterViewInit} from '@angular/core';
import { MapService } from './map.service';
import { AppService } from '../app.service';
import { ActivatedRoute } from '@angular/router';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { MapModel } from './map.model';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css']
})
export class MapComponent implements OnInit, AfterViewInit {
  map: MapModel;
  imageSafe: SafeUrl;
  constructor(private mapService: MapService, private route: ActivatedRoute, private appService: AppService,
    private sanitizer: DomSanitizer) {
  }

  ngOnInit() {
    this.route.params.subscribe(param => {
      setTimeout(() => {
                // tslint:disable-next-line:max-line-length

        this.map = null;
        this.appService.loading = true;
        this.getMap();
      });
    });
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.appService.loading = true;
      this.getMap();
    });
  }

  private getMap() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.mapService.getMap(id).subscribe(
      (res) => {
        this.map = res;
        console.log(this.map.Icon);
        this.imageSafe = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + this.map.Icon );
        setTimeout(() => {
          this.appService.loading = false;
        });
      },
      (err) => {
        setTimeout(() => {
          this.appService.loading = false;
        });
      }
    );
  }
}
