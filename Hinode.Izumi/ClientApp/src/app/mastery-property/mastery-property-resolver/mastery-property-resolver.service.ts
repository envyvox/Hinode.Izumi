import { Injectable } from '@angular/core';
import { IMasteryPropertyWebModel, MasteryPropertyWebModel, MasteryPropertyService } from '../../shared/web.api.service';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
    providedIn:'root'
})
export class MasteryPropertyResolverService implements Resolve<IMasteryPropertyWebModel> {
    constructor( private _masteryPropertyService: MasteryPropertyService ) {
    }

    resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
        : Observable<IMasteryPropertyWebModel> | Promise<IMasteryPropertyWebModel> | IMasteryPropertyWebModel {
        const id = route.paramMap.get('id');
        return this._masteryPropertyService
            .get(id ? parseInt(id) : 0)
            .pipe(
                take(1),
                mergeMap(x => of(x)),
                catchError(e => of(new MasteryPropertyWebModel()))
            );
    }
}
