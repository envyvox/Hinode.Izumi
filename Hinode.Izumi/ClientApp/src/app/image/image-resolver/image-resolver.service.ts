import { Injectable } from '@angular/core';
import { IImageWebModel, ImageWebModel, ImageService } from '../../shared/web.api.service';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
    providedIn:'root'
})
export class ImageResolverService implements Resolve<IImageWebModel> {
    constructor( private _imageService: ImageService ) {
    }

    resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
        : Observable<IImageWebModel> | Promise<IImageWebModel> | IImageWebModel {
        const id = route.paramMap.get('id');
        return this._imageService
            .get(id ? parseInt(id) : 0)
            .pipe(
                take(1),
                mergeMap(x => of(x)),
                catchError(e => of(new ImageWebModel()))
            );
    }
}
