import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { ContractService, ContractWebModel, IContractWebModel } from '../../shared/web.api.service';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ContractResolverService implements Resolve<IContractWebModel>{

  constructor(private _contractService: ContractService) { }

  resolve( route: ActivatedRouteSnapshot, state: RouterStateSnapshot )
      : Observable<IContractWebModel> | Promise<IContractWebModel> | IContractWebModel {
    const id = route.paramMap.get('id')
    return this._contractService
        .get(id ? parseInt(id) : 0)
        .pipe(
            take(1),
            mergeMap(x => of(x)),
            catchError(err => of(new ContractWebModel()))
        );
  }
}
