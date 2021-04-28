import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { AchievementService, AchievementWebModel, IAchievementWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AchievementResolverService implements Resolve<IAchievementWebModel>{

  constructor(private _achievementService: AchievementService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IAchievementWebModel> | Promise<IAchievementWebModel> | IAchievementWebModel {
    const id = route.paramMap.get('id');
    return this._achievementService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(e => of(new AchievementWebModel()))
        );
  }
}
