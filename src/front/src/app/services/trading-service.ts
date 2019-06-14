import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Trading } from './../models/trading';
import { TradingDetails } from '../models/trading-details';
import { HttpServiceBase } from '../utils/http-service-base';

@Injectable({
    providedIn: 'root'
})
export class TradingService extends HttpServiceBase
{
    constructor(private http: HttpClient)
    {
        super();
    }

    public async getTradableUsers(): Promise<Trading[]>
    {
        try
        {
            return await this.http.get<Trading[]>(
                'http://localhost:80/trading/',
                {headers: this.jsonHeaders}
            ).toPromise();
        }
        catch (ex)
        {
            return null;
        }
    }

    public async getTradingDetails(userId: number): Promise<TradingDetails>
    {
        try
        {
            return await this.http.get<TradingDetails>(
                `http://localhost:80/trading/${userId}`,
                {headers: this.jsonHeaders}
            ).toPromise();
        }
        catch (ex)
        {
            return null;
        }
    }
}
