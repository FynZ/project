import { Component, OnInit } from '@angular/core';
import { NewsService } from 'src/app/services/news.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-news-details',
    templateUrl: './news-details.component.html',
    styleUrls: ['./news-details.component.sass']
})
export class NewsDetailsComponent implements OnInit
{
    private slug: string;

    constructor(private route: ActivatedRoute, private newsService: NewsService, private toastr: ToastrService)
    {
        this.slug = this.route.snapshot.params.slug;
    }

    async ngOnInit()
    {
        const detailedNews = await this.newsService.getDetailedNews(this.slug);

        console.log(detailedNews);
    }
}
