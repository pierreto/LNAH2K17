import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

const LOGIN_URL = '/login';
const SIGNUP_URL = '/signup';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  login() {
    this.router.navigate([LOGIN_URL]);
  }

  signup() {
    this.router.navigate([SIGNUP_URL]);
  }

}
