import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

/** external imports */
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgProgressModule } from 'ngx-progressbar';
import { NgProgressHttpModule } from 'ngx-progressbar/http';
import { ToastrModule } from 'ngx-toastr';
import { TranslateModule } from '@ngx-translate/core';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ConfigurationModule } from 'zencode-configuration-manager';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { AppRoutingModule } from './app-routing.module';
import { TokenComponent } from './token/token.component';
import {
  API_BASE_URL_PROVIDER, CONFIGURATION_PATH_PROVIDER, ERROR_HALDER_PROVDER,
  HTTP_AUTH_INTERCEPTORS_PROVIDER, HTTP_SERVER_SIDE_VALIDATION_INTERCEPTORS_PROVIDER,
  HTTP_SYS_LANG_INTERCEPTORS_PROVIDER,
  NG_PROGRESS_HTTP_CONFIG_PROVIDER, TRANSLATE_PROVIDER
} from './app.config';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    WelcomeComponent,
    TokenComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FontAwesomeModule,
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ModalModule.forRoot(),
    NgProgressModule,
    NgProgressHttpModule,
    ToastrModule.forRoot(),
    TranslateModule.forRoot(TRANSLATE_PROVIDER),
    ConfigurationModule.forRoot(CONFIGURATION_PATH_PROVIDER),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    API_BASE_URL_PROVIDER,
    ERROR_HALDER_PROVDER,
    HTTP_AUTH_INTERCEPTORS_PROVIDER,
    HTTP_SYS_LANG_INTERCEPTORS_PROVIDER,
    NG_PROGRESS_HTTP_CONFIG_PROVIDER,
    HTTP_SERVER_SIDE_VALIDATION_INTERCEPTORS_PROVIDER
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
