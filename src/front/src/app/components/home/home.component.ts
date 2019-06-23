import { Component, OnInit } from '@angular/core';
import { NewsService } from 'src/app/services/news.service';
import { News } from 'src/app/models/news';
import { SlugifyPipe } from 'angular-pipes';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.sass']
})
export class HomeComponent implements OnInit {

    public news: News[];

    private currentLoadedPage: number;

    constructor(private newsService: NewsService, private toastr: ToastrService)
    {
    }

    async ngOnInit()
    {
        const news = await this.newsService.getNews();

        if (news)
        {
            this.news = news;
            this.currentLoadedPage = 1;
        }
        else
        {
            this.toastr.error('An error occured', 'Error');
        }
    }

    async loadNextNews()
    {
        const news = await this.newsService.getNewsForPage(this.currentLoadedPage + 1);

        if (news)
        {
            this.news = this.news.concat(news);
            this.currentLoadedPage++;

            console.log(this.news);
        }
        else
        {
            this.toastr.error('An error occured', 'Error');
        }
    }
}
