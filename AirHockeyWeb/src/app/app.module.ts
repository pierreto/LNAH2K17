import { SignupService } from './signup/signup.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { LoadingModule } from 'ngx-loading';

import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';

import { LoginComponent } from './login/login.component';
import { ProfileComponent } from './profile/profile.component';
import { RankingComponent } from './ranking/ranking.component';
import { SignupComponent } from './signup/signup.component';
import { MapComponent } from './map/map.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { HeaderComponent } from './header/header.component';
import { LoginService } from './login/login.service';
import { AppService } from './app.service';

@NgModule({
    imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    ReactiveFormsModule,
    LoadingModule
  ],
  declarations: [
    AppComponent,
    LoginComponent,
    ProfileComponent,
    RankingComponent,
    SignupComponent,
    MapComponent,
    PageNotFoundComponent,
    HeaderComponent
  ],
  providers: [SignupService, LoginService, AppService],
  bootstrap: [AppComponent]
})
export class AppModule { }
