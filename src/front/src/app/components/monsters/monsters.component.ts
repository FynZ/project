import { Component, OnInit } from '@angular/core';
import { Monster } from 'src/app/models/monster';
import { MonsterService } from 'src/app/services/monster.service';
import { AuthService } from 'src/app/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { delay } from 'q';
import { filterQueryId } from '@angular/core/src/view/util';

@Component({
    selector: 'app-monsters',
    templateUrl: './monsters.component.html',
    styleUrls: ['./monsters.component.scss']
})
export class MonstersComponent implements OnInit
{
    public monsters: Monster[];
    public displayedMonsters: Monster[] = [];

    public filterHave: string = '/';
    public filterNeed: string = '/';
    public filterPropose: string = '/';

    private displaying: boolean = false;

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
            
            await this.filter();
        }
    }

    async filter()
    {
        let tmp = this.monsters;

        if (this.filterHave != '/')
        {
            if (this.filterHave == '+')
            {
                tmp = tmp.filter(x => x.count > 0)
            }
            else if (this.filterHave == '-') 
            {
                tmp = tmp.filter(x => x.count == 0);
            }
        }

        if (this.filterNeed != '/')
        {
            if (this.filterNeed == '+')
            {
                tmp = tmp.filter(x => x.search == true)
            }
            else if (this.filterNeed == '-') 
            {
                tmp = tmp.filter(x => x.search == false);
            }
        }

        if (this.filterPropose != '/')
        {
            if (this.filterPropose == '+')
            {
                tmp = tmp.filter(x => x.propose == true)
            }
            else if (this.filterPropose == '-') 
            {
                tmp = tmp.filter(x => x.propose == false);
            }
        }

        this.displayedMonsters = tmp;
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

    async searchMonster(monsterId: number)
    {
        if (await this.monsterService.searchMonster(monsterId))
        {
            const monster = this.monsters.find(x => x.id == monsterId);
            monster.search = true;

            this.toastr.success(`Now looking for ${monster.name}`, 'Success');
        }
        else
        {
            this.toastr.error('An unexpected error occured', 'Error');
        }
    }

    async unsearchMonster(monsterId: number)
    {
        if (await this.monsterService.unsearchMonster(monsterId))
        {
            const monster = this.monsters.find(x => x.id == monsterId);
            monster.search = false;

            this.toastr.success(`No longer looking for ${monster.name}`, 'Success');
        }
        else
        {
            this.toastr.error('An unexpected error occured', 'Error');
        }
    }

    async proposeMonster(monsterId: number)
    {
        if (await this.monsterService.proposeMonster(monsterId))
        {
            const monster = this.monsters.find(x => x.id == monsterId);
            monster.propose = true;

            this.toastr.success(`Now proposing ${monster.name}`, 'Success');
        }
        else
        {
            this.toastr.error('An unexpected error occured', 'Error');
        }
    }

    async unproposeMonster(monsterId: number)
    {
        if (await this.monsterService.unproposeMonster(monsterId))
        {
            const monster = this.monsters.find(x => x.id == monsterId);
            monster.propose = false;

            this.toastr.success(`No longer proposing ${monster.name}`, 'Success');
        }
        else
        {
            this.toastr.error('An unexpected error occured', 'Error');
        }
    }
}
