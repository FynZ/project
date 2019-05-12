import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Trading } from './../models/trading';
import { TradingDetails } from '../models/trading-details';

@Injectable({
    providedIn: 'root'
})
export class TradingService
{
    private httpHeaders: HttpHeaders;

    constructor(private http: HttpClient)
    {
        this.httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': 'http://localhost:4200/'
        });
    }

    public async getTradableUsers(): Promise<Trading[]>
    {
        try
        {
            return await this.http.get<Trading[]>(
                'http://localhost:83/trading/',
                {headers: this.httpHeaders}
            ).toPromise();
        }
        catch (ex)
        {
            // non 401 error
            return null;
        }
    }

    public async getTradingDetails(userId: number): Promise<TradingDetails>
    {
        try
        {
            return await this.http.get<TradingDetails>(
                `http://localhost:83/trading/${userId}`,
                {headers: this.httpHeaders}
            ).toPromise();
        }
        catch (ex)
        {
            // non 401 error
            return null;
        }
    }
}
