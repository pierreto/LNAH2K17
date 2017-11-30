import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AppService } from '../app.service';
import { ProfileService } from '../profile/profile.service';

const LOGIN_URL = '/login';
const SIGNUP_URL = '/signup';
const RANKING_URL = '/rankings';
const PROFILE_URL = '/profile';
const MAPS_URL = '/maps';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})

export class HeaderComponent implements OnInit {

  constructor(private router: Router, private appService: AppService, private profileService: ProfileService) {
    this.appService.loginPage = true;
  }

  ngOnInit() {
  }

  login() {
    this.appService.loginPage = true;
    this.router.navigate([LOGIN_URL]);
  }

  maps() {
    this.router.navigate([MAPS_URL]);
  }

  signup() {
    this.appService.loginPage = false;
    this.router.navigate([SIGNUP_URL]);
  }

  ranking() {
    this.router.navigate([RANKING_URL]);
  }

  profile() {
    this.router.navigate(['/profile', this.appService.id]);
  }
}
