import { UpdateProfileWithPassword } from './../models/update-profile-with-password';
import { UpdateProfile } from './../models/update-profile';
import { UserInformation } from '../models/user-information';
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { UserProfile } from '../models/user-profile';
import { HttpServiceBase } from '../utils/http-service-base';

@Injectable({
    providedIn: 'root'
})
export class UserService extends HttpServiceBase
{
    constructor(private http: HttpClient)
    {
        super();
    }

    public async getUserInformation(): Promise<UserInformation>
    {
        try
        {
            return await this.http.get<UserInformation>(
                'http://localhost:80/auth/profile/',
                {headers : this.jsonHeaders}
            ).toPromise();
        }
        catch (ex)
        {
            return null;
        }
    }

    public async getUserProfile(userId: number): Promise<UserProfile>
    {
        try
        {
            return await this.http.get<UserProfile>(
                `http://localhost:80/auth/profile/${userId}`,
                {headers : this.jsonHeaders}
            ).toPromise();
        }
        catch (ex)
        {
            return null;
        }
    }

    public async updateUserProfile(userProfile: UpdateProfile): Promise<boolean>
    {
        try
        {
            await this.http.post(
                `http://localhost:80/auth/profile/update`,
                userProfile,
                {headers : this.jsonHeaders}
            ).toPromise();

            return true;
        }
        catch (ex)
        {
            return false;
        }
    }

    public async updateUserProfileWithPassword(userProfile: UpdateProfileWithPassword): Promise<boolean>
    {
        try
        {
            await this.http.post(
                `http://localhost:80/auth/profile/complete-update`,
                userProfile,
                {headers : this.jsonHeaders},
            ).toPromise();

            return true;
        }
        catch (ex)
        {
            return false;
        }
    }
}
