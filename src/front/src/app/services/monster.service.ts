import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Monster } from '../models/monster';

@Injectable({
    providedIn: 'root'
})
export class MonsterService {

    private jsonOptions: any;
    private regularOptions: any;

    constructor(private http: HttpClient)
    {
        const jsonheaders = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': 'http://localhost:4200/'
        });
        this.jsonOptions = {
            jsonheaders
        };
    }

    async getMonsters(): Promise<Monster[]>
    {
        try
        {
            const response = await this.http.get<Monster[]>('http://localhost:82/monsters/', this.jsonOptions).toPromise();
            const content = response as unknown as Monster[];

            console.log('success');
            console.log(content);

            return content;
        }
        catch (e)
        {
            const content: Monster[] = [];

            console.log('error');
            console.log(content);

            return content;
        }
    }

    async incrementMonster(monsterId : number) : Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/increment/${monsterId}`);
    }

    async decrementMonster(monsterId : number) : Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/decrement/${monsterId}`);
    }

    async proposeMonster(monsterId : number) : Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/propose/${monsterId}`);
    }

    async unproposeMonster(monsterId : number) : Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/unpropose/${monsterId}`);
    }

    async searchMonster(monsterId : number) : Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/search/${monsterId}`);
    }

    async unsearchMonster(monsterId : number) : Promise<boolean>
    {
        return await this.executeMonsterRequest(`http://localhost:82/monsters/unsearch/${monsterId}`);
    }

    private async executeMonsterRequest(url: string) : Promise<boolean>
    {
        try
        {
            console.log(url);

            await this.http.get(url, this.jsonOptions).toPromise();

            return true;
        }
        catch(e)
        {
            return false;
        }
    }
}
