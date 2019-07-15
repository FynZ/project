import { TradingService } from './../../services/trading-service';
import { MetricsService } from './../../services/metrics.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.sass']
})
export class AdminComponent implements OnInit, OnDestroy
{
    private timerInterval: any;

    public gatewayStatus: boolean;
    public serviceDiscoveryStatus: boolean;
    public newsServiceStatus: boolean;
    public ressourcesServiceStatus: boolean;
    public authServiceStatus: boolean;
    public monstersServiceStatus: boolean;
    public tradingServiceStatus: boolean;

    constructor(private metricsService: MetricsService, private toastr: ToastrService) { }

    async ngOnInit()
    {
        await this.poll();

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
            console.log(result[0]);
            this.gatewayStatus = result[0];
            console.log(result[1]);
            this.serviceDiscoveryStatus = result[1];
            console.log(result[2]);
            this.newsServiceStatus = result[2];
            console.log(result[3]);
            this.ressourcesServiceStatus = result[3];
            console.log(result[4]);
            this.authServiceStatus = result[4];
            console.log(result[5]);
            this.monstersServiceStatus = result[5];
            console.log(result[6]);
            this.tradingServiceStatus = result[6];
        });
    }
}
