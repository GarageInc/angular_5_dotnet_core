import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { Observable } from 'rxjs/Observable';
import { User } from './../models/user';

@Injectable()
export class AuthenticationService {
  public token: string | null;

  public get bearerToken(): string {
    return 'Bearer ' + this.token;
  }

  constructor(private http: Http) {
    // set token if saved in local storage
    const mock = localStorage.getItem('currentUser');
    if (mock) {
      const currentUser = JSON.parse(mock);
      this.token = currentUser && currentUser.token;
    }
  }

  login(username: string, password: string): Observable<boolean> {
    return this.http
      .post('/api/v1/account/authenticate', {
        Username: username,
        Password: password
      })
      .map((response: Response) => {
        // login successful if there's a jwt token in the response
        const token = response.json() && response.json().access_token;
        if (token) {
          // set token property
          this.token = token;

          // store username and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem(
            'currentUser',
            JSON.stringify({ username: username, token: token })
          );

          // return true to indicate successful login
          return true;
        } else {
          // return false to indicate failed login
          return false;
        }
      });
  }

  logout(): void {
    // clear token remove user from local storage to log user out
    this.token = null;
    localStorage.removeItem('currentUser');
  }

  public getUserName(): string {
    const user: any = JSON.parse(localStorage.getItem('currentUser') || '{}');
    return user ? user.username : 'аноним';
  }

  public isAuthorized(): boolean {
    return !!localStorage.getItem('currentUser');
  }
}
