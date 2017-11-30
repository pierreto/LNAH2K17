import { DomSanitizer } from '@angular/platform-browser';
import { Component, OnInit, ViewEncapsulation, AfterViewInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { AppService } from '../app.service';
import { Router } from '@angular/router';
import { MapsService } from './maps.service';
import { MapService } from '../map/map.service';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class MapsComponent implements OnInit, AfterViewInit {
  // displayedColumns = ['No', 'Username', 'GamesWon', 'TournamentsWon', 'Points'];
  displayedColumns = ['Preview', 'MapName', 'Creator', 'CreationDate', 'LastBackup', 'Private'];
  dataSource = new MatTableDataSource();
  showPagination: boolean;
  length: number;
  pageSize: number;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  // tslint:disable-next-line:max-line-length
  constructor(private appService: AppService, private router: Router, private mapsService: MapsService, private mapService: MapService, private sanitizer: DomSanitizer) {
    
    this.showPagination = false;
    this.length = 0;
    this.pageSize = 25;
  }

  ngOnInit() {

  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);

    setTimeout(() => {
      this.appService.loading = true;
    });
    this.mapsService.getMaps().subscribe(
      res => {
        this.length = res.length;
        for (let map of res){
            map.Icon = this.sanitizer.bypassSecurityTrustResourceUrl('data:image/jpg;base64,' + map.Icon );
        }
        this.dataSource.data = res;
        this.appService.loading = false;
      },
      err => {
        this.appService.loading = false;
      }
    );
  }

  goToMap(id: number) {
    this.appService.loading = true;
    this.mapService.getMap(id).subscribe(
      (res) => {
        this.appService.loading = false;
        this.router.navigate(['/maps', id]);
      },
      (err) => {
        this.appService.loading = false;
      }
    );
  }
}
