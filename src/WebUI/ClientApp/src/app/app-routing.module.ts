/** core imports */
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

/** internal imports */
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { DevEnvGuard } from './nav-menu/dev-env.guard';
import { WelcomeComponent } from './welcome/welcome.component';
import { TokenComponent } from './token/token.component';

export const routes: Routes = [

  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'welcome', component: WelcomeComponent },
  { path: 'token', component: TokenComponent, canActivate: [AuthorizeGuard, DevEnvGuard] },
  { path: '**', pathMatch: 'full', redirectTo: 'welcome' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule],
})
export class AppRoutingModule { }
