import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { GatheringService, GatheringWebModel, IGatheringWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GatheringResolverService implements Resolve<IGatheringWebModel>{

  constructor(private _gatheringService: GatheringService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IGatheringWebModel> | Promise<IGatheringWebModel> | IGatheringWebModel {
    const id = route.paramMap.get('id')
    return this._gatheringService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new GatheringWebModel()))
        );
  }
}
