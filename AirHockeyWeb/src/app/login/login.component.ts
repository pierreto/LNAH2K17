import {Router} from '@angular/router';
import { User } from './../user';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { LoginService } from './login.service';
import { AppService } from '../app.service';


const SIGNUP_URL = '/signup';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent implements OnInit {
  formErrors = {
    'username': '',
    'password': ''
  };

  validationMessages = {
    'username': {
      'required':      'Nom d\'usager requis.'
    },
    'password': {
      'required':      'Mot de passe requis.'
    }
  };

  private title = 'LNAH';
  private year = '2K17';
  private loginForm: FormGroup;
  private user;
  private validUser: boolean;
  constructor(private fb: FormBuilder, private router: Router, private loginService: LoginService, private appService: AppService) { }

  ngOnInit() {
    this.user = new User();
    this.validUser = true;
    this.buildForm();
  }

  buildForm(): void {
    this.loginForm = this.fb.group({
      username: [this.user.Username, [
          Validators.required]],
      password: [this.user.Password, [
            Validators.required]]
    });
    this.loginForm.valueChanges
      .subscribe(data => this.onValueChanged(data));

    this.onValueChanged();
  }

  onValueChanged(data?: any) {
    if (!this.loginForm) { return; }
    const form = this.loginForm;
    for (const field in this.formErrors) {
      const control = form.get(field);
      this.formErrors[field] = '';
      if (control && control.dirty && !control.valid) {
        const messages = this.validationMessages[field];
        for (const key in control.errors) {
          this.formErrors[field] += messages[key] + ' ';
        }
      }
    }
  }

  login() {
    this.appService.loading = true;
    this.loginService.login(this.user).subscribe(
      (res) => {
          this.appService.loading = false;
          this.validUser = true;
          this.router.navigate(['GO TO PROFILE']);
      },
      (err) => {
        this.appService.loading = false;
        this.validUser = false;
      }
    );
  }

  signup() {
    this.router.navigate([SIGNUP_URL]);
  }
}
