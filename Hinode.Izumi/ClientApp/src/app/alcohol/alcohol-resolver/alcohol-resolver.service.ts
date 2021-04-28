import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { AlcoholService, AlcoholWebModel, IAlcoholWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AlcoholResolverService implements Resolve<IAlcoholWebModel>{

  constructor(private _alcoholService: AlcoholService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IAlcoholWebModel> | Promise<IAlcoholWebModel> | IAlcoholWebModel {
    const id = route.paramMap.get('id')
    return this._alcoholService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new AlcoholWebModel()))
        );
  }
}
