import { MonsterTrading } from './../../models/monster-trading';
import { UserService } from './../../services/user-service';
import { UserProfile } from './../../models/user-profile';
import { TradingDetails } from './../../models/trading-details';
import { TradingService } from 'src/app/services/trading-service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit
{
    public profile: UserProfile;
    public monsters: TradingDetails;

    constructor(private userService: UserService, private tradingService: TradingService, private route: ActivatedRoute)
    {
    }

    async ngOnInit()
    {
        const userId = +this.route.snapshot.paramMap.get('id');

        this.profile = await this.userService.getUserProfile(userId);
        this.monsters = await this.tradingService.getTradingDetails(userId);
    }
}
