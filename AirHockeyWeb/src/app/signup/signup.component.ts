import { SignupService } from './signup.service';
import { passwordMatcher } from './passwordMatcher';
import { User } from './../user';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AppService } from '../app.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  formErrors = {
    'username': '',
    'passwords': {
      'password': '',
      'passwordConfirm': ''
    }
  };

  validationMessages = {
    'username': {
      'required':      'Nom d\'usager requis.',
      'minlength':     'Nom d\'usager doit contenir au moins 2 caractères.',
      'maxlength':     'Nom d\'usager doit contenir au plus 15 caractères.',
      'pattern':       'Nom d\'usager invalide.'
    },
    'password': {
      'required':      'Mot de passe requis.',
      'minlength':     'Mot de passe doit contenir au moins 8 caractères.',
      'pattern':       'Mot de passe doit contenir au moins une lettre minuscule, une lettre majuscule et un caractère spécial.'
    },
    'passwordConfirm': {
      'required': 'Confirmation du mot de passe requise.',
      'noMatch':  'Les mots de passe doivent être identiques.'
    }
  };

  private signupForm: FormGroup;
  private user: User;
  private passwordConfirm: string;
  constructor(private fb: FormBuilder, private router: Router, private signupService: SignupService, private appService: AppService) { }

  ngOnInit() {
    this.user = new User();
    console.log(this.user);
    this.passwordConfirm = '';
    this.buildForm();
  }

buildForm(): void {
    this.signupForm = this.fb.group({
      username: [this.user.Username, [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(15),
          Validators.pattern(/^[a-zA-Z0-9_.-]*$/)]],
      passwords: this.fb.group({
        password: [this.user.Password, [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(/(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]/)]],
        passwordConfirm: [this.passwordConfirm, [
            Validators.required
            ]]},
          { validator : passwordMatcher})
    });
    this.signupForm.valueChanges
      .subscribe(data => this.onValueChanged(data));

    this.onValueChanged();
  }

onValueChanged(data?: any) {
    if (!this.signupForm) { return; }
    const form = this.signupForm;
    for (const field in this.formErrors) {
      // check if the field corresponds a formgroup (controls is present)
      if ((form.get(field) as any).controls ) {
        // if yes, iterate the inner formfields
        for (const subfield in (form.get(field) as FormGroup).controls) {
          // in this example corresponds = "child", reset the error messages
          this.formErrors[field][subfield] = '';
          // now access the actual formfield
          const control = (form.get(field) as FormGroup).controls[subfield];
          // validate and show appropriate error message
          if (control && control.dirty && !control.valid) {
            const messages = this.validationMessages[subfield];
            for (const key in control.errors) {
              this.formErrors[field][subfield] += messages[key] + ' ';
            }
          }
        }
      } else { // does not contain a nested formgroup, so just iterate like before making changes to this method
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
  }


  signup(): void {
    this.appService.loading = true;
    this.signupService.signup(this.user).subscribe(
      () => {
        this.appService.loading = false;
      },
      () => {
        this.appService.loading = false;
      }
    );
  }
}
