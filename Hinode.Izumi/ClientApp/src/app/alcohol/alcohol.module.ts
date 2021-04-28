import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { AlcoholResolverService } from './alcohol-resolver/alcohol-resolver.service';
import { ContextMenuModule } from 'primeng/contextmenu';
import { TableModule } from 'primeng/table';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FieldsetModule } from 'primeng/fieldset';
import {SharedModule} from "../shared/shared.module";

const routes: Array<Route> = [
  {
    path: 'alcohol',
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
          model: AlcoholResolverService
        }
      }
    ]
  }
];

@NgModule({
  declarations: [ListComponent, EditComponent],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        ContextMenuModule,
        TableModule,
        RippleModule,
        ButtonModule,
        InputTextModule,
        FormsModule,
        ReactiveFormsModule,
        FieldsetModule,
        SharedModule
    ]
})
export class AlcoholModule { }
