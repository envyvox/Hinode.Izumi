import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import {
  AlcoholPropertyService,
  AlcoholPropertyWebModel,
  IAlcoholPropertyWebModel
} from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AlcoholPropertyResolverService implements Resolve<IAlcoholPropertyWebModel>{

  constructor(private _alcoholPropertyService: AlcoholPropertyService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IAlcoholPropertyWebModel> | Promise<IAlcoholPropertyWebModel> | IAlcoholPropertyWebModel {
    const id = route.paramMap.get('id')
    return this._alcoholPropertyService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new AlcoholPropertyWebModel()))
        );
  }
}
