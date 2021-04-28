import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { FoodResolverService } from './food-resolver/food-resolver.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ContextMenuModule } from 'primeng/contextmenu';
import { FieldsetModule } from 'primeng/fieldset';
import { SharedModule } from '../shared/shared.module';
import { BadgeModule } from 'primeng/badge';

const routes: Array<Route> = [
  {
    path: 'food',
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
          model: FoodResolverService
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
        TableModule,
        ButtonModule,
        RippleModule,
        RouterModule,
        DropdownModule,
        FormsModule,
        InputTextModule,
        ReactiveFormsModule,
        ContextMenuModule,
        FieldsetModule,
        SharedModule,
        BadgeModule
    ]
})
export class FoodModule { }
