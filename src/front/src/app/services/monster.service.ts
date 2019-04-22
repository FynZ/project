import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Monster } from '../models/monster';

@Injectable({
    providedIn: 'root'
})
export class MonsterService {

    private options: any;

    constructor(private http: HttpClient)
    {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': 'http://localhost:4200/'
        });
        this.options = {
            headers
        };
    }

    async getMonsters(): Promise<Monster[]>
    {
        try
        {
            const response = await this.http.get<Monster[]>('http://localhost:82/monsters/', this.options).toPromise();
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
}
