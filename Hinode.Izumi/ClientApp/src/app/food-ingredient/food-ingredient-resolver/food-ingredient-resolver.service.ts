import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { FoodIngredientService, FoodIngredientWebModel, IFoodIngredientWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FoodIngredientResolverService implements Resolve<IFoodIngredientWebModel>{

  constructor(private _foodIngredientService: FoodIngredientService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IFoodIngredientWebModel> | Promise<IFoodIngredientWebModel> | IFoodIngredientWebModel {
    const id = route.paramMap.get('id');
    return this._foodIngredientService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new FoodIngredientWebModel()))
        );
  }
}
