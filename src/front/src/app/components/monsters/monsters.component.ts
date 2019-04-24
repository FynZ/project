import { Component, OnInit } from '@angular/core';
import { Monster } from 'src/app/models/monster';
import { MonsterService } from 'src/app/services/monster.service';
import { AuthService } from 'src/app/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-monsters',
    templateUrl: './monsters.component.html',
    styleUrls: ['./monsters.component.scss']
})
export class MonstersComponent implements OnInit
{
    public monsters: Monster[];

    constructor(
        private monsterService: MonsterService, 
        private toastr: ToastrService,
        private authService: AuthService)
    {
    }

    async ngOnInit()
    {
        if (this.authService.isAuthenticated())
        {
            this.monsters = await this.monsterService.getMonsters();
        }
    }
    
    async incrementMonster(monsterId: number)
    {
        if (await this.monsterService.incrementMonster(monsterId))
        {
            const monster = this.monsters.find(x => x.id == monsterId);
            monster.count++;

            this.toastr.success(`Successfully increased count for ${monster.name}`, 'Success');
        }
        else
        {
            this.toastr.error('An unexpected error occured', 'Error');
        }
    }

    async decrementMonster(monsterId: number)
    {
        if (await this.monsterService.decrementMonster(monsterId))
        {
            const monster = this.monsters.find(x => x.id == monsterId);
            monster.count--;

            this.toastr.success(`Successfully decreased count for ${monster.name}`, 'Success');
        }
        else
        {
            this.toastr.error('An unexpected error occured', 'Error');
        }
    }

    // async incrementMonster(monsterId: number)
    // {
        
    // }

    // async incrementMonster(monsterId: number)
    // {
        
    // }

    // async incrementMonster(monsterId: number)
    // {
        
    // }
}
