import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { GatheringPropertyResolverService } from './gathering-property-resolver/gathering-property-resolver.service';
import { ContextMenuModule } from 'primeng/contextmenu';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextModule } from 'primeng/inputtext';

const routes: Array<Route> = [
  {
    path: 'gathering-property',
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
          model: GatheringPropertyResolverService
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
    DropdownModule,
    FormsModule,
    SharedModule,
    FieldsetModule,
    ReactiveFormsModule,
    InputTextModule,
  ]
})
export class GatheringPropertyModule { }
