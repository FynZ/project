import { Injectable } from '@angular/core';
import { News } from '../models/news';

import * as moment from 'moment';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { HttpServiceBase } from '../utils/http-service-base';

@Injectable({
    providedIn: 'root'
})
export class NewsService extends HttpServiceBase
{
    constructor(private http: HttpClient)
    {
        super();
    }

    async getNews(): Promise<News[]>
    {
        try
        {
            return await this.http.get<News[]>(
                'http://localhost:80/news/news',
                {headers: this.jsonHeaders})
            .toPromise();
        }
        catch (e)
        {
            return null;
        }
    }

    async getNewsForPage(page: number)
    {
        try
        {
            return await this.http.get<News[]>(
                `http://localhost:80/news/news/${page}`, 
                {headers: this.jsonHeaders})
            .toPromise();
        }
        catch (e)
        {
            return null;
        }
    }
}
