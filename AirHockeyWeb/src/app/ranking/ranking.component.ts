import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource, MatSort, MatPaginator } from '@angular/material';
import { Ranking } from './ranking';
import { RankingService } from './ranking.service';
import { AppService } from '../app.service';

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

  constructor(private rankingService: RankingService, private appService: AppService) {
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
    this.rankingService.getProfiles().subscribe(
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

  goToProfileOf(i: string) {
    console.log(i);
  }
}
