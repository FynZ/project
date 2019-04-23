import { Component, OnInit } from '@angular/core';
import { Monster } from 'src/app/models/monster';
import { MonsterService } from 'src/app/services/monster.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-monsters',
    templateUrl: './monsters.component.html',
    styleUrls: ['./monsters.component.scss']
})
export class MonstersComponent implements OnInit
{
    public monsters: Monster[];

    constructor(private monsterService: MonsterService, private authService: AuthService)
    {
    }

    async ngOnInit()
    {
        if (this.authService.isAuthenticated())
        {
            this.monsters = await this.monsterService.getMonsters();
        }
    }
}
