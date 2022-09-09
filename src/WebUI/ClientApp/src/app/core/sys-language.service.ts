/** core imports */
import { Injectable } from '@angular/core';

/** external imports */
import { TranslateService } from '@ngx-translate/core';

/** internal imports */
import { EN_US, X_SYSTEM_LANGUAGE } from '../shared/constant';

@Injectable({
    providedIn: 'root',
})
export class LanguageService {

    /**
     * @param translateService
     */
    constructor(private translateService: TranslateService) { }

    /**
     * Sets default
     */
    setDefault(): void {
        const lang = localStorage.getItem(X_SYSTEM_LANGUAGE);

        if (!lang) {
            this.translateService.use(EN_US);

            localStorage.setItem(X_SYSTEM_LANGUAGE, EN_US);
        } else {
            this.translateService.use(lang);
        }
    }

    /**
     * Changes language
     * @param culture
     */
    changeLanguage(culture: string): void {
        this.translateService.use(culture);

        localStorage.setItem(X_SYSTEM_LANGUAGE, culture);

        setTimeout(() => {
            location.reload();
        }, 1);
    }

    /**
     * Gets language
     */
    get language(): string {
        return localStorage.getItem(X_SYSTEM_LANGUAGE) || EN_US;
    }


}
