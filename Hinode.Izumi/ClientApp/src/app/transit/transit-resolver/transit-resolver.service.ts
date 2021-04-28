import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';
import { ITransitWebModel, TransitService, TransitWebModel } from '../../shared/web.api.service';

@Injectable({
  providedIn: 'root'
})
export class TransitResolverService implements Resolve<ITransitWebModel>{

  constructor(private _transitService: TransitService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<ITransitWebModel> | Promise<ITransitWebModel> | ITransitWebModel {
    const id = route.paramMap.get('id');
    return this._transitService
        .get(id? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new TransitWebModel()))
        );
  }
}
