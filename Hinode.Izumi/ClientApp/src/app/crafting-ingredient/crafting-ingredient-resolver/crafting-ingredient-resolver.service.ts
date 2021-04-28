import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import {
  CraftingIngredientService,
  CraftingIngredientWebModel,
  ICraftingIngredientWebModel
} from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CraftingIngredientResolverService implements Resolve<ICraftingIngredientWebModel>{

  constructor(private _craftingIngredientService: CraftingIngredientService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<ICraftingIngredientWebModel> | Promise<ICraftingIngredientWebModel> | ICraftingIngredientWebModel {
    const id = route.paramMap.get('id');
    return this._craftingIngredientService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new CraftingIngredientWebModel()))
        );
  }
}
