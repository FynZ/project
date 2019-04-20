import { AuthService } from './../services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.sass']
})
export class NavbarComponent implements OnInit
{
    constructor(private authService: AuthService) { }

    ngOnInit() {
    }

    public isLoggedIn(): boolean
    {
        return this.authService.isAuthenticated();
    }
}
