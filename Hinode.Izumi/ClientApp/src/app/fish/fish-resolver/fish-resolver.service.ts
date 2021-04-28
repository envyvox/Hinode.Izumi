import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { FishService, FishWebModel, IFishWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FishResolverService implements Resolve<IFishWebModel>{

  constructor(private _fishService: FishService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IFishWebModel> | Promise<IFishWebModel> | IFishWebModel {
    const id = route.paramMap.get('id');
    return this._fishService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new FishWebModel()))
        );
  }
}
