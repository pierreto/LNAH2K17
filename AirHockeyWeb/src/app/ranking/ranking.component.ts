import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Ranking } from './ranking';
import { RankingService } from './ranking.service';
import { AppService } from '../app.service';
import { Router } from '@angular/router';
import { ProfileService } from '../profile/profile.service';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.css']
})
export class RankingComponent implements OnInit, AfterViewInit {
  // displayedColumns = ['No', 'Username', 'GamesWon', 'TournamentsWon', 'Points'];
  displayedColumns = ['Username', 'GamesWon', 'TournamentsWon', 'Points'];
  dataSource = new MatTableDataSource();
  showPagination: boolean;
  length: number;
  pageSize: number;

  @ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  // tslint:disable-next-line:max-line-length
  constructor(private rankingService: RankingService, private appService: AppService, private router: Router, private profileService: ProfileService) {
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
    this.rankingService.getRankings().subscribe(
      res => {
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

  goToProfileOf(id: number) {
    this.appService.loading = true;
    this.profileService.getProfile(id).subscribe(
      (res) => {
        this.appService.loading = false;
        this.router.navigate(['/profile', id]);
      },
      (err) => {
        this.appService.loading = false;
        console.log(err);
      }
    );
  }
}
