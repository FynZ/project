import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpServiceBase } from '../utils/http-service-base';
import { MetricsResponse } from '../models/metrics-response';

@Injectable({
    providedIn: 'root'
})
export class MetricsService extends HttpServiceBase
{
    private readonly gatewayUrl              = 'http://localhost:80/health';
    private readonly clientDiscoveryUrl      = 'http://localhost:80/service-discovery/health';
    private readonly newsServiceUrl          = 'http://localhost:80/news/actuator/health';
    private readonly ressourcesServiceUrl    = 'http://localhost:80/ressources/health';
    private readonly authServiceUrl          = 'http://localhost:80/auth/health';
    private readonly monstersServiceUrl      = 'http://localhost:80/monsters/health';
    private readonly tradingServiceUrl       = 'http://localhost:80/trading/health';

    constructor(private http: HttpClient)
    {
        super();
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
            // await this.http.get(url, {responseType: 'text'}).toPromise();
            const result = await this.http.get<MetricsResponse>(
                url, 
                {headers: this.jsonHeaders})
                .toPromise();

            console.log('success');

            return true;
        }
        catch (e)
        {
            console.log(`error while calling ${url} with error`);
            console.log(e);

            return false;
        }
    }
}
