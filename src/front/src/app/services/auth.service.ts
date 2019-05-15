import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Login } from '../models/login';
import { Register } from '../models/register';
import { LoginResult } from './../models/login-result';
import { RegisterResult } from '../models/register-result';

@Injectable({
    providedIn: 'root'
})
export class AuthService
{
    private httpHeaders: HttpHeaders;

    private authenticated: boolean;
    private roles: string[];

    private token: string;
    private expires: number;

    constructor(private http: HttpClient)
    {
        this.authenticated = false;

        this.httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json'
        });
    }

    public async login(login: Login): Promise<boolean>
    {
        try
        {
            const response = await this.http.post<LoginResult>(
                'http://localhost:80/auth/login/',
                login,
                {headers : this.httpHeaders}
                ).toPromise();

            if (response)
            {
                this.token = response.token;
                this.expires = response.expires;

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

    public async register(register: Register): Promise<RegisterResult>
    {
        try
        {
            return await this.http.post<RegisterResult>(
                'http://localhost:80/auth/register/',
                register,
                {headers: this.httpHeaders}
            ).toPromise();
        }
        catch (ex)
        {
            // Conflict error, we have the data to see what went wrong
            if (ex.code === 401)
            {
                return ex.error as unknown as RegisterResult;
            }

            // non 401 error
            return null;
        }
    }

    public logout()
    {
        this.authenticated = false;
        this.roles = [];
        this.token = '';
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
