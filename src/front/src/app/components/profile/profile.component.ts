import { MonsterService } from 'src/app/services/monster.service';
import { MonsterSummary } from './../../models/monster-summary';
import { UserInformation } from './../../models/user-information';
import { UserService } from './../../services/user-service';
import { Component, OnInit } from '@angular/core';
import { UpdateProfileWithPassword } from 'src/app/models/update-profile-with-password';
import { ToastrService } from 'ngx-toastr';
import { UpdateProfile } from 'src/app/models/update-profile';
import { Monster } from 'src/app/models/monster';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit
{
    public userInformation: UserInformation;
    public monsterSummary: MonsterSummary;

    public searchedMonsters: Monster[] = [];
    public proposedMonsters: Monster[] = [];

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
        const userInformation = this.userService.getUserInformation();
        const monsterSummary = this.monsterService.getSummary();

        const searchedMonsters = this.monsterService.getSearchedMonsters();
        const proposedMonsters = this.monsterService.getProposedMonsters();

        await Promise.all<UserInformation, MonsterSummary, Monster[], Monster[]>
        ([userInformation, monsterSummary, searchedMonsters, proposedMonsters])
            .then((result: any[]) =>
        {
            this.userInformation = result[0];
            this.monsterSummary = result[1];
            this.searchedMonsters = result[2];
            this.proposedMonsters = result[3];
        });
    }

    async updateProfile()
    {
        let result = false;

        if (this.password.length === 0 && this.passwordConfirm.length === 0)
        {
            const profile = new UpdateProfile();
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

        this.password = '';
        this.passwordConfirm = '';

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
