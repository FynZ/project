import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class AuthGuardService implements CanActivate
{
    constructor(private authService: AuthService, private router: Router)
    {

    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean
    {
        console.log('trying to access route ' + state.url);
        if (this.authService.isAuthenticated)
        {
            if (state.url.indexOf('admin') > 0)
            {
                console.log('trying to access admin route');

                if (this.authService.hasRole('Admin'))
                {
                    console.log('user has role Admin');
                    return true;
                }

                console.log('User does not have role Admin');
                return false;
            }
        }

        return true;
    }

    checkLogin(url: string): boolean
    {
        if (this.authService.isAuthenticated)
        {
            return true;
        }

        // Store the attempted URL for redirecting
        // this.authService.redirectUrl = url;

        // Navigate to the login page with extras
        this.router.navigate(['/login']);

        return false;
    }
}
