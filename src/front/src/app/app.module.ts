import { AuthInterceptor } from './interceptors/auth-interceptor';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ToastrModule } from 'ngx-toastr';
import { NgxTwitterTimelineModule } from 'ngx-twitter-timeline';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { MonstersComponent } from './components/monsters/monsters.component';
import { MonsterDetailsComponent } from './components/monster-details/monster-details.component';
import { FooterComponent } from './components/footer/footer.component';
import { UserComponent } from './components/user/user.component';
import { ProfileComponent } from './components/profile/profile.component';
import { AdminComponent } from './components/admin/admin.component';
import { TradingComponent } from './components/trading/trading.component';
import { MarketplaceComponent } from './components/marketplace/marketplace.component';
import { MapComponent } from './components/map/map.component';
import { MessagesComponent } from './components/messages/messages.component';

const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  // { path: 'monsters', component: AppComponent },
  { path: 'login',          component: LoginComponent },
  { path: 'register',       component: RegisterComponent },
  { path: 'profile',        component: ProfileComponent},
  { path: 'monsters',       component: MonstersComponent},
  { path: 'monsters/:id',   component: MonsterDetailsComponent },
  { path: 'trading',        component: TradingComponent},
  { path: 'marketplace',    component: MarketplaceComponent},
  { path: 'map',            component: MapComponent},
  { path: 'messages',       component: MessagesComponent},
  { path: 'user/;id',       component: UserComponent},
  { path: 'admin',          component: AdminComponent},
  { path: '**',             component: NotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    NotFoundComponent,
    MonsterDetailsComponent,
    MonstersComponent,
    MonsterDetailsComponent,
    FooterComponent,
    UserComponent,
    ProfileComponent,
    AdminComponent,
    TradingComponent,
    MarketplaceComponent,
    MapComponent,
    MessagesComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    CommonModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 2500,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    NgxTwitterTimelineModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
