import { Trading } from './../../models/trading';
import { Component, OnInit } from '@angular/core';
import { TradingService } from 'src/app/services/trading-service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';

@Component({
    selector: 'app-trading',
    templateUrl: './trading.component.html',
    styleUrls: ['./trading.component.scss']
})
export class TradingComponent implements OnInit
{
    public trading: Trading[] = [];

    constructor(
        private monsterService: TradingService,
        private toastr: ToastrService,
        private authService: AuthService)
        {
        }

        async ngOnInit()
        {
            if (this.authService.isAuthenticated())
            {
                this.trading = await this.monsterService.getTradableUsers();

                console.log(this.trading);
            }
        }
    }
