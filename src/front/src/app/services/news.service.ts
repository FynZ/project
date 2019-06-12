import { Injectable } from '@angular/core';
import { News } from '../models/news';

import * as moment from 'moment';
import { HttpHeaders, HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class NewsService
{
    private httpHeaders: HttpHeaders;

    constructor(private http: HttpClient)
    {
        this.httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': 'http://localhost:4200/'
        });
    }

    async getNews(): Promise<News[]>
    {
        try
        {
            return await this.http.get<News[]>('http://localhost:80/news/news', {headers: this.httpHeaders}).toPromise();
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
            return await this.http.get<News[]>(`http://localhost:80/news/news/${page}`, {headers: this.httpHeaders}).toPromise();
        }
        catch (e)
        {
            return null;
        }
    }
}
