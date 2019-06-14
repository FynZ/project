import { MonsterSummary } from './../models/monster-summary';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Monster } from '../models/monster';
import { HttpServiceBase } from '../utils/http-service-base';

@Injectable({
    providedIn: 'root'
})
export class MonsterService extends HttpServiceBase
{
    constructor(private http: HttpClient)
    {
        super();
    }

    async getSummary(): Promise<MonsterSummary>
    {
        try
        {
            return await this.http.get<MonsterSummary>(
                "http://localhost:80/monsters/summary", 
                {headers: this.jsonHeaders})
                .toPromise();
        }
        catch (e)
        {
            return null;
        }
    }

    async getMonsters(): Promise<Monster[]>
    {
        try
        {
            return await this.http.get<Monster[]>(
                'http://localhost:80/monsters/',
                {headers: this.jsonHeaders})
            .toPromise();
        }
        catch (e)
        {
            return [];
        }
    }

    async incrementMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:80/monsters/increment/${monsterId}`);
    }

    async decrementMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:80/monsters/decrement/${monsterId}`);
    }

    async proposeMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:80/monsters/propose/${monsterId}`);
    }

    async unproposeMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:80/monsters/unpropose/${monsterId}`);
    }

    async searchMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:80/monsters/search/${monsterId}`);
    }

    async unsearchMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:80/monsters/unsearch/${monsterId}`);
    }

    private async executeMonsterRequest(url: string): Promise<boolean>
    {
        try
        {
            await this.http.post(
                url,
                {headers: this.jsonHeaders})
            .toPromise();

            return true;
        }
        catch(e)
        {
            return false;
        }
    }
}
