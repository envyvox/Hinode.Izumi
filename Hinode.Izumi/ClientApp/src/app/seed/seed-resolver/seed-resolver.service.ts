import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { ISeedWebModel, SeedService, SeedWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SeedResolverService implements Resolve<ISeedWebModel>{

  constructor(private _seedService: SeedService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      :Observable<ISeedWebModel> | Promise<ISeedWebModel> | ISeedWebModel {
    const id = route.paramMap.get('id');
    return this._seedService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new SeedWebModel()))
        );
  }
}
