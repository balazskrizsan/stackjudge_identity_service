import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { catchError, map } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  title = 'identity-sandbox-ui';
  private readonly BACKEND_URL = 'https://localhost:7090/weatherforecast';
  public data: any;

  constructor(
    readonly oidcSecurityService: OidcSecurityService,
    readonly http: HttpClient
  ) {}

  ngOnInit() {
    this.oidcSecurityService
      .checkAuth()
      .subscribe(({ isAuthenticated, userData, accessToken, idToken }) => {
        console.log({ isAuthenticated, userData, accessToken, idToken });
      });
  }

  login() {
    this.oidcSecurityService.authorize();
  }

  logout() {
    this.oidcSecurityService
      .logoff()
      .subscribe((result) => console.log(result));
  }

  fetchData() {
    const renderData = (data: any) =>
      (this.data = JSON.stringify(data, null, 2));

    this.http
      .get(this.BACKEND_URL)
      .subscribe(renderData);
  }
}
