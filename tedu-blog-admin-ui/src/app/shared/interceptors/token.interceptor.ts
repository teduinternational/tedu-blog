import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpInterceptor,
} from '@angular/common/http';
import {
  catchError,
  Observable,
  switchMap,
  throwError,
  tap,
  Subject
} from 'rxjs';
import { TokenStorageService } from '../services/token-storage.service';
import {
  AdminApiTokenApiClient,
  AuthenticatedResult,
  TokenRequest,
} from '../../api/admin-api.service.generated';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/shared/services/alert.service';
import { BroadcastService } from 'src/app/shared/services/boardcast.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  refreshTokenInProgress = false;
  tokenRefreshedSource = new Subject();
  tokenRefreshed$ = this.tokenRefreshedSource.asObservable();

  constructor(
    private router: Router,
    private tokenService: TokenStorageService,
    private tokenApiClient: AdminApiTokenApiClient,
    private alertService: AlertService,
    private boardCastService: BroadcastService) { }

  addAuthHeader(request) {
    const authHeader = this.tokenService.getToken();
    if (authHeader) {
      return request.clone({
        setHeaders: {
          'Authorization': `Bearer ` + authHeader
        }
      });
    }
    return request;
  }

  refreshToken(): Observable<any> {
    if (this.refreshTokenInProgress) {
      return new Observable(observer => {
        this.tokenRefreshed$.subscribe(() => {
          observer.next();
          observer.complete();
        });
      });
    } else {
      this.refreshTokenInProgress = true;
      const token = this.tokenService.getToken();
      const refreshToken = this.tokenService.getRefreshToken();
      var tokenRequest = new TokenRequest({
        accessToken: token!,
        refreshToken: refreshToken!,
      });
      return this.tokenApiClient.refresh(tokenRequest).pipe(
        tap((response: AuthenticatedResult) => {
          this.refreshTokenInProgress = false;
          this.tokenService.saveToken(response.token!);
          this.tokenService.saveRefreshToken(response.refreshToken!);
          this.tokenRefreshedSource.next(response.token);
        }),
        catchError((err) => {
          this.refreshTokenInProgress = false;
          this.logout();
          return throwError(err);
        }));
    }
  }

  logout() {
    this.tokenService.signOut();
    this.router.navigate(["login"]);
  }

  async handleResponseError(error, request?, next?) {
    // Business error
    if (error.status === 400) {
      const errMessage = await (new Response(error.error)).text();
      this.alertService.showError(errMessage);
      this.boardCastService.httpError.next(true);
    }

    // Invalid token error
    else if (error.status === 401) {
      return this.refreshToken().pipe(
        switchMap(() => {
          request = this.addAuthHeader(request);
          return next.handle(request);
        }),
        catchError(e => {
          if (e.status !== 401) {
            return this.handleResponseError(e);
          } else {
            this.logout();
          }
        }));
    }

    // Access denied error
    else if (error.status === 403) {
      // Logout
      this.logout();
      this.boardCastService.httpError.next(true);

    }
    // Maintenance error
    else if (error.status === 500) {
      this.alertService.showError('Hệ thống có lỗi xảy ra. Vui lòng liên hệ admin');
      this.boardCastService.httpError.next(true);
    }

    return throwError(error);
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<any> {
    // Handle request
    request = this.addAuthHeader(request);

    // Handle response
    return next.handle(request).pipe(catchError(error => {
      return this.handleResponseError(error, request, next);
    }));
  }
}
