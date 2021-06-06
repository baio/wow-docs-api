import { Component } from '@angular/core';
import { AuthService } from 'src/auth';

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss'],
  providers: [AuthService],
})
export class Tab1Page {
  constructor(private readonly auth: AuthService) {}

  onYaLogin() {
    console.log('+++');
    this.auth.login();
  }
}
