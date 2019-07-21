import { Injectable } from '@angular/core';
import { News } from '../models/news';

import * as moment from 'moment';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { HttpServiceBase } from '../utils/http-service-base';
import { NewsDetails } from '../models/news-details';
import { NewsCreation } from '../models/news-creation';

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
            const news = await this.http.get<News[]>(
                // 'http://localhost:88/news/1',
                'http://localhost:80/news/news/1',
                {headers: this.jsonHeaders})
            .toPromise();

            console.log('success');
            console.log(news);
            return news;
        }
        catch (e)
        {
            console.log('error');
            console.log(e);
            return null;
        }
    }

    async getNewsForPage(page: number): Promise<News[]>
    {
        try
        {
            return await this.http.get<News[]>(
                `http://localhost:80/news/news/${page}`,
                {headers: this.jsonHeaders}
            ).toPromise();
        }
        catch (e)
        {
            return null;
        }
    }

    async getDetailedNews(slug: string): Promise<NewsDetails>
    {
        try
        {
            return await this.http.get<NewsDetails>(
                `http://localhost:80/news/news/details/${slug}`,
                {headers: this.jsonHeaders}
            ).toPromise();
        }
        catch (e)
        {
            return null;
        }
    }

    async createNews(newsCreation: NewsCreation): Promise<boolean>
    {
        try
        {
            await this.http.post<any>(
                `http://localhost:80/news/news/create`,
                newsCreation,
                {headers: this.jsonHeaders}
            ).toPromise();

            return true;
        }
        catch (e)
        {
            return false;
        }
    }
}
