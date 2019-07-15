import { AuthService } from './../services/auth.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor
{
    constructor(private authService: AuthService)
    {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
    {
        if (this.authService.isAuthenticated())
        {
            const token = this.authService.getToken();

            // console.log('token:');
            // console.log(token);

            const cloned = req.clone({
                headers: req.headers.set('Authorization', 'Bearer ' + token)
            });

            return next.handle(cloned);
        }
        else
        {
            return next.handle(req);
        }
    }
}
