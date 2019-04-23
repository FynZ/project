import { LoginResult } from './../models/login-result';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { tokenKey } from '@angular/core/src/view';

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    private options: any;

    private authenticated: boolean;
    private roles: string[];

    private token: string;
    private expires: number;

    constructor(private http: HttpClient)
    {
        this.authenticated = false;

        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': 'http://localhost:4200/'
         });
        this.options = {
            headers
         };
     }

    public async login(email: string, password: string, rememberMe: boolean = false): Promise<boolean>
    {
        try
        {
            const response = await this.http.post<LoginResult>('http://localhost:81/auth/login/', {
                Email: email,
                Password: password,
                RememberMe: rememberMe
            }, this.options).toPromise();

            const result = response as unknown as LoginResult;

            if (result)
            {
                this.token = result.token;
                this.expires = result.expires;

                this.authenticated = true;

                return true;
            }

            return false;
        }
        catch (ex)
        {
            return false;
        }
    }

    public logout()
    {
        this.authenticated = false;
        this.roles = [];
        this.token = "";
        this.expires = 0;
    }

    public isAuthenticated(): boolean
    {
        return this.authenticated;
    }

    public getToken(): string
    {
        if (this.token)
        {
            return this.token;
        }

        return '';
    }

    private decodeJwt(loginResult: LoginResult)
    {

    }
}
