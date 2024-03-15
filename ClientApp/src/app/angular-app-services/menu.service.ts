import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppConfigService } from '../app-config.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  constructor(private http: HttpClient) { }

  private get route() {
    const baseUrl = AppConfigService.appConfig ? AppConfigService.appConfig.api.url : '';
    return `${baseUrl}/api/meta-data/menu`;
  }

  public getMenu(): Observable<any> {
    return this.http.get<any>(this.route);
  }
}
