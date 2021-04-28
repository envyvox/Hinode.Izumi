import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import {
  GatheringPropertyService,
  GatheringPropertyWebModel,
  IGatheringPropertyWebModel
} from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GatheringPropertyResolverService implements Resolve<IGatheringPropertyWebModel>{

  constructor(private _gatheringPropertyService: GatheringPropertyService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IGatheringPropertyWebModel> | Promise<IGatheringPropertyWebModel> | IGatheringPropertyWebModel {
    const id = route.paramMap.get('id')
    return this._gatheringPropertyService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new GatheringPropertyWebModel()))
        );
  }
}
