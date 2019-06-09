import { AuthGuardService } from './services/authguard.service';
import { FooterComponent } from './components/footer/footer.component';
// default
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// angular
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

// libs
import { ToastrModule } from 'ngx-toastr';
import { NgxTwitterTimelineModule } from 'ngx-twitter-timeline';
import { NgStringPipesModule } from 'angular-pipes';

// project
import { AppComponent } from './components/app/app.component';
// import { AppRoutingModule } from './configuration/app-routing.module';

import { AuthInterceptor } from './interceptors/auth-interceptor';

import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { ProfileComponent } from './components/profile/profile.component';
import { MonstersComponent } from './components/monsters/monsters.component';
import { MonsterDetailsComponent } from './components/monster-details/monster-details.component';
import { TradingComponent } from './components/trading/trading.component';
import { MarketplaceComponent } from './components/marketplace/marketplace.component';
import { MapComponent } from './components/map/map.component';
import { MessagesComponent } from './components/messages/messages.component';
import { UserComponent } from './components/user/user.component';
import { AdminComponent } from './components/admin/admin.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { RouterModule, Routes } from '@angular/router';
import { NavbarComponent } from './components/navbar/navbar.component';
import { NewsDetailsComponent } from './components/news-details/news-details.component';

const appRoutes: Routes = [
  { path: '',               component: HomeComponent },
  { path: 'news/:name',     component: NewsDetailsComponent },
  { path: 'login',          component: LoginComponent },
  { path: 'register',       component: RegisterComponent },
  { path: 'profile',        component: ProfileComponent },
  { path: 'monsters',       component: MonstersComponent },
  { path: 'monsters/:id',   component: MonsterDetailsComponent },
  { path: 'trading',        component: TradingComponent },
  { path: 'marketplace',    component: MarketplaceComponent },
  { path: 'map',            component: MapComponent },
  { path: 'messages',       component: MessagesComponent },
  { path: 'user/:id',       component: UserComponent },
  { path: 'admin',          component: AdminComponent, canActivate: [AuthGuardService], },
  { path: '**',             component: NotFoundComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    ProfileComponent,
    MonstersComponent,
    MonsterDetailsComponent,
    TradingComponent,
    MarketplaceComponent,
    MapComponent,
    MessagesComponent,
    UserComponent,
    AdminComponent,
    NotFoundComponent,
    NewsDetailsComponent
  ],
  imports: [
    // angular
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot(appRoutes),
    // libs
    ToastrModule.forRoot({
      timeOut: 2500,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    NgxTwitterTimelineModule,
    NgStringPipesModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
