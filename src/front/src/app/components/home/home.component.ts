import { Component, OnInit } from '@angular/core';
import { NewsService } from 'src/app/services/news.service';
import { News } from 'src/app/models/news';
import { SlugifyPipe } from 'angular-pipes';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {

    public news: News[];

    constructor(private newsService: NewsService)
    {
    }

    async ngOnInit()
    {
        this.news = this.newsService.getNews();
    }
}
