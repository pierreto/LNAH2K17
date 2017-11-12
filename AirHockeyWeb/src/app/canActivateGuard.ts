import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { AppService } from './app.service';

@Injectable()
export class CanActivateGuard implements CanActivate {
  constructor(private appService: AppService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean>|Promise<boolean>|boolean {
    if (this.appService.loggedIn) {
      return true;
    } else {
      this.router.navigate(['/forbidden']);
      return false;
    }
  }
}
