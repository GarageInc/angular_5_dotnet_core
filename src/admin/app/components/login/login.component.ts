import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from './../../../../common/services/authentication.service';
import { HeaderEnums } from '../../enums/header.enum';
import { EventService } from '../../../../common/services/event-service/event.service';
import { AdminNavigationService } from '../../services/navigation/admin-navigation.service';

@Component({
  templateUrl: 'login.component.html',
  styleUrls: ['login.component.scss']
})
export class LoginComponent implements OnInit {
  model: any = {};
  loading = false;
  error = '';

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    private eventsService: EventService,
    private navigation: AdminNavigationService
  ) {}

  ngOnInit() {
    // reset login status
    this.authenticationService.logout();
  }

  login() {
    this.loading = true;
    this.authenticationService
      .login(this.model.username, this.model.password)
      .subscribe(
        result => {
          if (result === true) {
            this.navigation.goToMain();
            this.eventsService.broadcast(HeaderEnums.reloadCounter);
          } else {
            this.catchError();
          }
        },
        () => {
          this.catchError();
        }
      );
  }

  private catchError(): void {
    this.error = 'Упс, что-то введено не верно :(';
    this.loading = false;
  }
}
