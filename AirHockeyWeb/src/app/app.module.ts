import { SignupService } from './signup/signup.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoadingModule } from 'ngx-loading';

import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { MapComponent } from './map/map.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { HeaderComponent } from './header/header.component';
import { LoginService } from './login/login.service';
import { AppService } from './app.service';
import { MatTableModule, MatPaginatorModule, MatSortModule } from '@angular/material';
import { HttpModule } from '@angular/http';
import { RankingService } from './ranking/ranking.service';
import { RankingComponent } from './ranking/ranking.component';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
    imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    HttpModule,
    AppRoutingModule,
    ReactiveFormsModule,
    LoadingModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
  ],
  declarations: [
    AppComponent,
    LoginComponent,
    RankingComponent,
    SignupComponent,
    MapComponent,
    PageNotFoundComponent,
    HeaderComponent,
    ProfileComponent
  ],
  providers: [SignupService, LoginService, AppService, RankingService],
  bootstrap: [AppComponent]
})
export class AppModule { }
