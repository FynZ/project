import { UserInformation } from './../../models/user-information';
import { UserService } from './../../services/user-service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.sass']
})
export class ProfileComponent implements OnInit
{
    public userInformation: UserInformation;

    constructor(private userService: UserService)
    {
    }

    async ngOnInit()
    {
        this.userInformation = await this.userService.getUserInformation();
    }

}
