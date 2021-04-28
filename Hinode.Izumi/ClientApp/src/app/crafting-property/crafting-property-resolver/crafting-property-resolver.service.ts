import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { CraftingPropertyService, CraftingPropertyWebModel, ICraftingPropertyWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CraftingPropertyResolverService implements Resolve<ICraftingPropertyWebModel>{

  constructor(private _craftingPropertyService: CraftingPropertyService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<ICraftingPropertyWebModel> | Promise<ICraftingPropertyWebModel> | ICraftingPropertyWebModel {
    const id = route.paramMap.get('id')
    return this._craftingPropertyService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new CraftingPropertyWebModel()))
        );
  }
}
