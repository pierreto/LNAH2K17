import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Profile } from './profile';
import { ProfileService } from './profile.service';
import { AppService } from '../app.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, AfterViewInit {
  displayedColumns = ['Username', 'GamesWon', 'TournamentsWon', 'Points'];
  dataSource = new MatTableDataSource();
  showPagination: boolean;
  length: number;
  pageSize: number;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private profileService: ProfileService, private appService: AppService) {
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

    this.appService.loading = true;
    this.profileService.getProfiles().subscribe(
      res => {
        console.log(res);
        this.length = res.length;
        this.dataSource.data = res;
        this.appService.loading = false;
      },
      err => {
        this.appService.loading = false;
        console.log(err);
      }
    );
  }
}
