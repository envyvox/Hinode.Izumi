import { Injectable } from '@angular/core';
import { IUserWebModel, UserWebModel, UserService } from '../../shared/web.api.service';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
    providedIn:'root'
})
export class UserResolverService implements Resolve<IUserWebModel> {
    constructor( private _userService: UserService ) {
    }

    resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
        : Observable<IUserWebModel> | Promise<IUserWebModel> | IUserWebModel {
        const id = route.paramMap.get('id');
        return this._userService
            .get(id ? id : '0')
            .pipe(
                take(1),
                mergeMap(x => of(x)),
                catchError(e => of(new UserWebModel()))
            );
    }
}
