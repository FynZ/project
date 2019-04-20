import { AuthService } from './../services/auth.service';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

export class AuthInterceptor implements HttpInterceptor {

    constructor(private authService: AuthService)
    {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
    {
        if (this.authService.isAuthenticated())
        {
            const token = this.authService.getToken();

            const cloned = req.clone({
                headers: req.headers.set('Authorization',
                    'Bearer ' + token)
            });

            return next.handle(cloned);
        }
        else
        {
            return next.handle(req);
        }
    }
}
