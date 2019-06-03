import { UpdateProfileWithPassword } from './../models/update-profile-with-password';
import { UpdateProfile } from './../models/update-profile';
import { UserInformation } from '../models/user-information';
import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { UserProfile } from '../models/user-profile';

@Injectable({
    providedIn: 'root'
})
export class UserService {

    private httpHeaders: HttpHeaders;

    constructor(private http: HttpClient)
    {
        this.httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json'
        });
    }

    public async getUserInformation(): Promise<UserInformation>
    {
        try
        {
            return await this.http.get<UserInformation>(
                'http://localhost:80/auth/profile/',
                {headers : this.httpHeaders}
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
                {headers : this.httpHeaders}
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
                {headers : this.httpHeaders}
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
                {headers : this.httpHeaders},
            ).toPromise();

            return true;
        }
        catch (ex)
        {
            return false;
        }
    }
}
