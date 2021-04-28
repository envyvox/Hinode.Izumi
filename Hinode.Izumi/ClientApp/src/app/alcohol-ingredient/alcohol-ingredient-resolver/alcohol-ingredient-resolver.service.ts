import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import {
  AlcoholIngredientService,
  AlcoholIngredientWebModel,
  IAlcoholIngredientWebModel
} from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AlcoholIngredientResolverService implements Resolve<IAlcoholIngredientWebModel>{

  constructor(private _alcoholIngredientService: AlcoholIngredientService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IAlcoholIngredientWebModel> | Promise<IAlcoholIngredientWebModel> | IAlcoholIngredientWebModel {
    const id = route.paramMap.get('id');
    return this._alcoholIngredientService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new AlcoholIngredientWebModel()))
        );
  }
}
