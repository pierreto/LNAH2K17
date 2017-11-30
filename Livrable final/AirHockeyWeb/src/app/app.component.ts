import { Component } from '@angular/core';
import { AppService } from './app.service';
import { User } from './user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'LNAH';
  year = '2K17';

  constructor(private appService: AppService) {
  }
}
