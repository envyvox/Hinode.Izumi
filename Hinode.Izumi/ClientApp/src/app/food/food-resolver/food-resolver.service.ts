import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { FoodService, FoodWebModel, IFoodWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FoodResolverService implements Resolve<IFoodWebModel>{

  constructor(private _foodService: FoodService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IFoodWebModel> | Promise<IFoodWebModel> | IFoodWebModel {
    const id = route.paramMap.get('id');
    return this._foodService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new FoodWebModel()))
        );
  }
}
