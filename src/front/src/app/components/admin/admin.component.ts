import { UserService } from './../../services/user-service';
import { UserManagement } from './../../models/user-management';
import { TradingService } from './../../services/trading-service';
import { MetricsService } from './../../services/metrics.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NewsCreation } from 'src/app/models/news-creation';
import { NewsService } from 'src/app/services/news.service';

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit, OnDestroy
{
    // #region News
    public title = '';
    public content = '';
    // #endregion News

    // #region Monitoring
    private timerInterval: any;

    public gatewayStatus: boolean;
    public serviceDiscoveryStatus: boolean;
    public newsServiceStatus: boolean;
    public ressourcesServiceStatus: boolean;
    public authServiceStatus: boolean;
    public monstersServiceStatus: boolean;
    public tradingServiceStatus: boolean;
    // #endregion Monitoring

    // #region Users
    public users: UserManagement[];
    // #endregion Users

    constructor(
        private newsService: NewsService,
        private metricsService: MetricsService,
        private userService: UserService,
        private toastr: ToastrService)
    {
    }

    async ngOnInit()
    {
        await this.poll();
        this.users = await this.userService.getUsers();

        this.timerInterval = setInterval(async () =>
        {
            console.log('polling...');

            await this.poll();
            this.toastr.success(`Fetched application statuses`, 'Success');
        }, 10000);
    }

    ngOnDestroy(): void
    {
        clearInterval(this.timerInterval);
    }

    public async createNews()
    {
        const news = new NewsCreation(this.title, this.content);

        const result = await this.newsService.createNews(news);

        if (result === true)
        {
            this.toastr.success(`News created`, 'Success');
            this.title = '';
            this.content = '';
        }
        else
        {
            this.toastr.error('Error creating news', 'Error');
        }
    }

    private async poll()
    {
        const gateway = this.metricsService.getGatewayStatus();
        const clientDiscovery = this.metricsService.getClientDiscoveryStatus();
        const news = this.metricsService.getNewsServiceStatus();
        const ressourcesService = this.metricsService.getRessourcesServiceStatus();
        const authService = this.metricsService.getAuthServiceStatus();
        const monstersService = this.metricsService.getMonsterServiceStatus();
        const tradingService = this.metricsService.getTradingServiceStatus();

        await Promise.all<boolean, boolean, boolean, boolean, boolean, boolean, boolean>
        ([gateway, clientDiscovery, news, ressourcesService, authService, monstersService, tradingService])
            .then((result: any[]) =>
        {
            this.gatewayStatus = result[0];
            this.serviceDiscoveryStatus = result[1];
            this.newsServiceStatus = result[2];
            this.ressourcesServiceStatus = result[3];
            this.authServiceStatus = result[4];
            this.monstersServiceStatus = result[5];
            this.tradingServiceStatus = result[6];
        });
    }
}
