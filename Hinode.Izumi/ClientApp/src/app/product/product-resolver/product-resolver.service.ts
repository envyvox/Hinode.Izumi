import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { IProductWebModel, ProductService, ProductWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProductResolverService implements Resolve<IProductWebModel>{

  constructor(private _productService: ProductService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IProductWebModel> | Promise<IProductWebModel> | IProductWebModel {
    const id = route.paramMap.get('id')
    return this._productService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new ProductWebModel()))
        );
  }
}
