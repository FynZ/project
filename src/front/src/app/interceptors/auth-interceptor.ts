import { AuthService } from './../services/auth.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthInterceptor implements HttpInterceptor
{
    // for an unknown reason response body are duplicated when adding token
    // for an endpoint that does not need them
    // and i'm unable to find out why
    // so i'm hard coding skipping those endpoints
    private exceptions = [
        'http://localhost:80/health',
        'http://localhost:80/service-discovery/health',
        'http://localhost:80/news/actuator/health',
        'http://localhost:80/ressources/health',
        'http://localhost:80/auth/health',
        'http://localhost:80/monsters/health',
        'http://localhost:80/trading/health',
        'http://localhost:80/news/news/1',
        'http://localhost:80/news/news/4',
        'http://localhost:80/news/news/5',
        'http://localhost:80/news/news/6',
        'http://localhost:80/news/news/7',
    ];

    constructor(private authService: AuthService)
    {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
    {
        if (this.authService.isAuthenticated())
        {
            // if (req.method == "GET" && this.exceptions.includes(req.url))
            // {
            //     console.log(`Intercepted and no token exception request, skipping for ${req.url}`);
            //     return next.handle(req);
            // }

            const token = this.authService.getToken();

            // console.log('token:');
            // console.log(token);

            const cloned = req.clone({
                headers: req.headers.set('Authorization', 'Bearer ' + token)
            });

            console.log(`Added auth token for ${req.url}`);

            return next.handle(cloned);
        }
        else
        {
            console.log(`No token to add for ${req.url}`);

            return next.handle(req);
        }
    }
}
