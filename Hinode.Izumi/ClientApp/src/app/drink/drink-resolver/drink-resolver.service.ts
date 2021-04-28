import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { DrinkService, DrinkWebModel, IDrinkWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DrinkResolverService implements Resolve<IDrinkWebModel>{

  constructor(private _drinkService: DrinkService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IDrinkWebModel> | Promise<IDrinkWebModel> | IDrinkWebModel {
    const id = route.paramMap.get('id')
    return this._drinkService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new DrinkWebModel()))
        );
  }
}
