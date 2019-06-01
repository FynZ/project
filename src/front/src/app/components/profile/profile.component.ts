import { MonsterService } from 'src/app/services/monster.service';
import { MonsterSummary } from './../../models/monster-summary';
import { UserInformation } from './../../models/user-information';
import { UserService } from './../../services/user-service';
import { Component, OnInit } from '@angular/core';
import { UpdateProfileWithPassword } from 'src/app/models/update-profile-with-password';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.sass']
})
export class ProfileComponent implements OnInit
{
    public userInformation: UserInformation;
    public monsterSummary: MonsterSummary;

    public password: string = '';
    public passwordConfirm: string = '';

    constructor(
        private userService: UserService,
        private monsterService: MonsterService,
        private toastr: ToastrService)
    {
    }

    async ngOnInit()
    {
        this.userInformation = await this.userService.getUserInformation();
        this.monsterSummary = await this.monsterService.getSummary();
    }

    async update()
    {
        let result = false;

        if (this.password.length !== 0 || this.passwordConfirm.length !== 0)
        {
            const profile = new UpdateProfileWithPassword();
            profile.email = this.userInformation.email;
            profile.server = this.userInformation.server;
            profile.inGameName = this.userInformation.inGameName;
            profile.subscribed = this.userInformation.subscribed;

            result = await this.userService.updateUserProfile(profile);
        }
        else
        {
            const profile = new UpdateProfileWithPassword();
            profile.email = this.userInformation.email;
            profile.server = this.userInformation.server;
            profile.inGameName = this.userInformation.inGameName;
            profile.password = this.password;
            profile.passwordConfirm = this.passwordConfirm;
            profile.subscribed = this.userInformation.subscribed;

            result = await this.userService.updateUserProfileWithPassword(profile);
        }

        if (result)
        {
            this.toastr.success('Profile successfully updated', 'Success');
        }
        else
        {
            this.toastr.error('An error occured when updating your profile', 'Error');
        }
    }
}
