import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Monster } from '../models/monster';

@Injectable({
    providedIn: 'root'
})
export class MonsterService
{
    private httpHeaders: HttpHeaders;

    constructor(private http: HttpClient)
    {
        this.httpHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': 'http://localhost:4200/'
        });
    }

    async getMonsters(): Promise<Monster[]>
    {
        try
        {
            return await this.http.get<Monster[]>('http://localhost:82/monsters/', {headers: this.httpHeaders}).toPromise();
        }
        catch (e)
        {
            return [];
        }
    }

    async incrementMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/increment/${monsterId}`);
    }

    async decrementMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/decrement/${monsterId}`);
    }

    async proposeMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/propose/${monsterId}`);
    }

    async unproposeMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/unpropose/${monsterId}`);
    }

    async searchMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/search/${monsterId}`);
    }

    async unsearchMonster(monsterId: number): Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/unsearch/${monsterId}`);
    }

    private async executeMonsterRequest(url: string): Promise<boolean>
    {
        try
        {
            await this.http.get(url, {headers: this.httpHeaders}).toPromise();

            return true;
        }
        catch(e)
        {
            return false;
        }
    }
}
