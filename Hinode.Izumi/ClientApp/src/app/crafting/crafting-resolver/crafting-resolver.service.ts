import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { CraftingService, CraftingWebModel, ICraftingWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CraftingResolverService implements Resolve<ICraftingWebModel>{

  constructor(private _craftingService: CraftingService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<ICraftingWebModel> | Promise<ICraftingWebModel> | ICraftingWebModel {
    const id = route.paramMap.get('id');
    return this._craftingService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new CraftingWebModel()))
        );
  }
}
