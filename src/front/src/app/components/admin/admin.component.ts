import { TradingService } from './../../services/trading-service';
import { MetricsService } from './../../services/metrics.service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-admin',
    templateUrl: './admin.component.html',
    styleUrls: ['./admin.component.sass']
})
export class AdminComponent implements OnInit
{
    public gatewayStatus: boolean;
    public serviceDiscoveryStatus: boolean;
    public newsServiceStatus: boolean;
    public ressourcesServiceStatus: boolean;
    public authServiceStatus: boolean;
    public monstersServicesStatus: boolean;
    public tradingServiceStatus: boolean;

    constructor(private metricsService: MetricsService) { }

    async ngOnInit()
    {
        await this.poll();

        setTimeout(async () =>
        {
           await this.poll();
        }, 10000);
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
            this.monstersServicesStatus = result[5];
            this.tradingServiceStatus = result[6];
        });
    }
}
