import { Component, OnInit, ViewEncapsulation, AfterViewInit } from '@angular/core';
import { ProfileService } from './profile.service';
import { ActivatedRoute } from '@angular/router';
import { AppService } from '../app.service';
import { Observable } from 'rxjs/Observable';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class ProfileComponent implements OnInit, AfterViewInit {
  profile: any;
  constructor(private profileService: ProfileService, private route: ActivatedRoute, private appService: AppService) {
  }

  ngOnInit() {
    this.route.params.subscribe(param => {
      setTimeout(() => {
        this.profile = null;
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
    this.profileService.getProfile(id).subscribe(
      (res) => {
        // tslint:disable-next-line:forin
        for (const achivement of res.AchievementEntities) {
          (achivement as any).DisabledImageUrl = ((achivement as any).DisabledImageUrl as any).replace(/\\/g, '/');
          (achivement as any).EnabledImageUrl = ((achivement as any).EnabledImageUrl as any).replace(/\\/g, '/');
        }
        console.log(res);
        this.profile = res;
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
