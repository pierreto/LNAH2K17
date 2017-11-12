import { SignupComponent } from './signup/signup.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { NgModule } from '@angular/core';
import { RankingComponent } from './ranking/ranking.component';
import { ProfileComponent } from './profile/profile.component';
import { CanActivateGuard } from './canActivateGuard';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';


// TODO : Put the right paths
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'rankings', component: RankingComponent, canActivate: [CanActivateGuard]},
  { path: 'profile/:id', component: ProfileComponent, canActivate: [CanActivateGuard] },
  { path: 'forbidden', component: UnauthorizedComponent},
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }
];
@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
