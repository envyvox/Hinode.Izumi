import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { ContractResolverService } from './contract-resolver/contract-resolver.service';
import { ContextMenuModule } from 'primeng/contextmenu';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { SharedModule } from '../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FieldsetModule } from 'primeng/fieldset';

const routes: Array<Route> = [
  {
    path: 'contract',
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'list'
      },
      {
        path: 'list',
        component: ListComponent
      },
      {
        path: 'edit',
        component: EditComponent
      },
      {
        path: 'edit/:id',
        component: EditComponent,
        resolve: {
          model: ContractResolverService
        }
      }
    ]
  }
];

@NgModule({
  declarations: [ListComponent, EditComponent],
  imports:[
    CommonModule,
    RouterModule.forChild(routes),
    ContextMenuModule,
    TableModule,
    ButtonModule,
    RippleModule,
    InputTextModule,
    DropdownModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    FieldsetModule,
  ]
})
export class ContractModule { }
