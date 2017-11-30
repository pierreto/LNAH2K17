import { AbstractControl } from '@angular/forms';

export const passwordMatcher = (control: AbstractControl): { [key: string]: boolean } => {
  const password = control.get('password');
  const confirmPassword = control.get('passwordConfirm');
  if (!password || !confirmPassword) {
    return null;
  }
  return password.value === confirmPassword.value ? null : { 'noMatch': true };
};