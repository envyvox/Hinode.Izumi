import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { ILocalizationWebModel, LocalizationService, LocalizationWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LocalizationResolverService implements Resolve<ILocalizationWebModel>{

  constructor(private _localizationService: LocalizationService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<ILocalizationWebModel> | Promise<ILocalizationWebModel> | ILocalizationWebModel {
    const id = route.paramMap.get('id')
    return this._localizationService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new LocalizationWebModel()))
        );
  }
}
