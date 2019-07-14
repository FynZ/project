import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class MetricsService
{
    private readonly gatewayUrl              = 'http://localhost:80/actuator/ping';
    private readonly clientDiscoveryUrl      = 'http://localhost:90/actuator/ping';
    private readonly newsServiceUrl          = 'http://localhost:88/news/actuator/health';
    private readonly ressourcesServiceUrl    = 'http://localhost:89/ressources/actuator/health';
    private readonly authServiceUrl          = 'http://localhost:81/ping';
    private readonly monstersServiceUrl      = 'http://localhost:82/ping';
    private readonly tradingServiceUrl       = 'http://localhost:83/ping';

    constructor(private http: HttpClient)
    {

    }

    public async getGatewayStatus(): Promise<boolean>
    {
        return await this.doRequest(this.gatewayUrl);
    }

    public async getClientDiscoveryStatus(): Promise<boolean>
    {
        return await this.doRequest(this.clientDiscoveryUrl);
    }

    public async getNewsServiceStatus(): Promise<boolean>
    {
        return await this.doRequest(this.newsServiceUrl);
    }

    public async getRessourcesServiceStatus(): Promise<boolean>
    {
        return await this.doRequest(this.ressourcesServiceUrl);
    }

    public async getAuthServiceStatus(): Promise<boolean>
    {
        return await this.doRequest(this.authServiceUrl);
    }

    public async getMonsterServiceStatus(): Promise<boolean>
    {
        return await this.doRequest(this.monstersServiceUrl);
    }

    public async getTradingServiceStatus(): Promise<boolean>
    {
        return await this.doRequest(this.tradingServiceUrl);
    }

    private async doRequest(url: string): Promise<boolean>
    {
        try
        {
            await this.http.get(url).toPromise();

            return true;
        }
        catch (e)
        {
            return false;
        }
    }
}
