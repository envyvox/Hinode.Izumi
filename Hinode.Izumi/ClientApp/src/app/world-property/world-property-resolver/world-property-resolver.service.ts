import { Injectable } from '@angular/core';
import { IWorldPropertyWebModel, WorldPropertyWebModel, WorldPropertyService } from '../../shared/web.api.service';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
    providedIn:'root'
})
export class WorldPropertyResolverService implements Resolve<IWorldPropertyWebModel> {
    constructor( private _worldPropertyService: WorldPropertyService ) {
    }

    resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
        : Observable<IWorldPropertyWebModel> | Promise<IWorldPropertyWebModel> | IWorldPropertyWebModel {
        const id = route.paramMap.get('id');
        return this._worldPropertyService
            .get(id ? parseInt(id) : 0)
            .pipe(
                take(1),
                mergeMap(x => of(x)),
                catchError(e => of(new WorldPropertyWebModel()))
            );
    }
}
