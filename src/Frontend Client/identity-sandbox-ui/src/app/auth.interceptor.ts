import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpInterceptor,
} from '@angular/common/http';
import { from, lastValueFrom } from 'rxjs';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private readonly oidcSecurityService: OidcSecurityService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    return from(this.handle(req, next));
  }

  async handle(request: HttpRequest<any>, next: HttpHandler) {
    const accessToken = await lastValueFrom(
      this.oidcSecurityService.getAccessToken()
    );

    request = request.clone({
      headers: request.headers.set(
        'Authorization',
        `Bearer ${accessToken}`
      ),
    });

    return await lastValueFrom(next.handle(request));
  }
}
