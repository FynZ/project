import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.sass']
})
export class NavbarComponent implements OnInit
{
    constructor(public authService: AuthService, private router: Router) { }

    ngOnInit() {
    }

    public isLoggedIn(): boolean
    {
        return this.authService.isAuthenticated();
    }

    public logout()
    {
        this.authService.logout();
        this.router.navigateByUrl('/');
    }
}
