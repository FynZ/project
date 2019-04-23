import { Component, OnInit } from '@angular/core';
import { MonsterService } from 'src/app/services/monster.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Monster } from 'src/app/models/monster';

@Component({
    selector: 'app-monster-details',
    templateUrl: './monster-details.component.html',
    styleUrls: ['./monster-details.component.sass']
})
export class MonsterDetailsComponent implements OnInit
{
    public monster: Monster;
    
    constructor(
        private route: ActivatedRoute,
        private monsterService: MonsterService,
        private location: Location) 
    {
            
    }
        
    ngOnInit()
    {
        this.getMonster();
    }

    public goBack()
    {
        this.location.back();
    }
    
    private getMonster()
    {
        const id = +this.route.snapshot.paramMap.get('id');

        const monster: Monster = new Monster();

        monster.id = id;
        monster.ankamaId = 1;
        monster.count = 1;
        monster.minLevel = 1;
        monster.maxLevel = 999;
        monster.name = "Toto [PH]";
        monster.propose = true;
        monster.search = false;
        monster.slug = "Slugus [PH]";

        this.monster = monster;
    }
}
    