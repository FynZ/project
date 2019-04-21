import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

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
