import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Login } from '../models/login';
import { Register } from '../models/register';
import { LoginResult } from '../models/login-result';
import { RegisterResult } from '../models/register-result';
import { Token } from '../models/token';
import { HttpServiceBase } from '../utils/http-service-base';

@Injectable({
    providedIn: 'root'
})
export class AuthService extends HttpServiceBase
{
    private authenticated: boolean;

    private token: Token;
    private tokenString: string;
    private expires: number;

    constructor(private http: HttpClient)
    {
        super();

        const token = localStorage.getItem('token');

        if (token && token.length !== 0)
        {
            // check if token is still valid
            console.log(token);
            this.token = JSON.parse(atob(token.split('.')[1])) as Token;
            console.log(this.token);

            this.tokenString = token;
            this.authenticated = true;
        }
        else
        {
            this.authenticated = false;
        }
    }

    public async login(login: Login): Promise<boolean>
    {
        try
        {
            const response = await this.http.post<LoginResult>(
                'http://localhost:80/auth/login/',
                login,
                {headers : this.jsonHeaders}
            ).toPromise();

            if (response)
            {
                localStorage.setItem('token', response.token);

                this.token = JSON.parse(atob(response.token.split('.')[1])) as Token;
                console.log(this.token);

                this.tokenString = response.token;

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
                {headers: this.jsonHeaders}
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
        localStorage.setItem('token', '');

        this.token = null;
        this.tokenString = '';
        this.expires = -1;
        this.authenticated = false;
    }

    public isAuthenticated(): boolean
    {
        return this.authenticated;
    }

    public hasRole(role: string): boolean
    {
        if (this.isAuthenticated)
        {
            if (this.token.roles.includes(role))
            {
                return true;
            }
        }

        return false;
    }

    public getToken(): string
    {
        if (this.token)
        {
            return this.tokenString;
        }

        return '';
    }
}
