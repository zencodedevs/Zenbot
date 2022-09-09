/** core imports */
import { HttpClient, HTTP_INTERCEPTORS } from "@angular/common/http";
import { ErrorHandler, Provider } from "@angular/core";
import { TranslateLoader } from "@ngx-translate/core";
import { TranslateHttpLoader } from "@ngx-translate/http-loader";

/** external imports */
import { Guid } from "guid-typescript";
import { NgProgressHttpConfig, NG_PROGRESS_HTTP_CONFIG } from "ngx-progressbar/http";

/** internal imports */
import { environment } from "src/environments/environment";
import { CONFIGURATION } from "zencode-configuration-manager";
import { AppErrorHandler } from "./app.error.handler";
import { AuthInterceptor } from "./core/auth.interceptor";
import { ServerSideValidationInterceptor } from "./core/server-side-val.interceptor";
import { SysLanguageInterceptor } from "./core/sys-language.interceptor";
import { API_BASE_URL } from "./shared/web-api-client";

/** start configuration path provider */
export const CONFIGURATION_PATH_PROVIDER: any = `assets/config/${environment.name}.json`;
/** end configuration provider */

/** start translate configuration */
declare var require: any;
export function createTranslateLoader(http: HttpClient) {
  const pack = require("package.json");
  return new TranslateHttpLoader(
    http,
    "assets/locales/",
    `.json?v=${pack && pack.version ? pack.version : Guid.create()}`
  );
}
export const TRANSLATE_PROVIDER: any = {
  loader: {
    provide: TranslateLoader,
    useFactory: createTranslateLoader,
    deps: [HttpClient],
  },
};
/** end translate configuration */



/** atart api base url provider */
export function getBaseUrl(): string {
  return CONFIGURATION.get<string>('api_base_url');
}
export const API_BASE_URL_PROVIDER: Provider = {
  provide: API_BASE_URL,
  useFactory: getBaseUrl
};
/** end api base url provider */



/** start ng progress http config providing */
export function GetNgProgressHttpConfig(): NgProgressHttpConfig {
  const config: NgProgressHttpConfig = {} as NgProgressHttpConfig;

  const silentApis = CONFIGURATION.get<string[]>("silentApiKeys") || [];

  silentApis.forEach((part, index) => {
    silentApis[index] = silentApis[index].toLocaleLowerCase();
  });

  config.silentApis = [...silentApis];

  return config;
}
export const NG_PROGRESS_HTTP_CONFIG_PROVIDER: Provider = {
  provide: NG_PROGRESS_HTTP_CONFIG,
  multi: true,
  useFactory: GetNgProgressHttpConfig,
};
/** end ng progress http config providing */

/** start sys language interceptor */
export const HTTP_SYS_LANG_INTERCEPTORS_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: SysLanguageInterceptor,
  multi: true,
};
/** end sys language interceptor */


/** start auth  interceptor */
export const HTTP_AUTH_INTERCEPTORS_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: AuthInterceptor,
  multi: true,
};
/** end auth  interceptor */



/** start sys language interceptor */
export const HTTP_SERVER_SIDE_VALIDATION_INTERCEPTORS_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  useClass: ServerSideValidationInterceptor,
  multi: true,
};
/** end sys language interceptor */


/** start error handler provider */
export const ERROR_HALDER_PROVDER: Provider = {
  provide: ErrorHandler,
  useClass: AppErrorHandler
};
/** end error handler provider */

