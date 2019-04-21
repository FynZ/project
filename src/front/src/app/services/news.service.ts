import { Injectable } from '@angular/core';
import { News } from '../models/news';

import * as moment from 'moment';

@Injectable({
    providedIn: 'root'
})
export class NewsService
{
    constructor()
    {
    }

    public getNews(): News[]
    {
        const news: News[] = [
            new News('News 1', 'Content 1', moment().toDate()),
            new News('News 2', 'Content 2', moment().toDate()),
            new News('News 3', 'Content 3', moment().toDate())
        ];

        return news;
    }
}
