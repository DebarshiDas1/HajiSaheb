import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfigService } from '../app-config.service';

@Injectable({
    providedIn: 'root'
})
export class LayoutService {
    constructor(private http: HttpClient) { }

    private get route() {
        const baseUrl = AppConfigService.appConfig ? AppConfigService.appConfig.api.url : '';
        return `${baseUrl}/api/meta-data`;
    }

    public getLayout(entityName: string, layoutType: string): Observable<any> {
        return this.http.get<any>(`${this.route}/${entityName}/layout?layoutType=${layoutType}`);
    }
}
