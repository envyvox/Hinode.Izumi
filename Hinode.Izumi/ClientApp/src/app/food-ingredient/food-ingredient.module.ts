import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { FoodIngredientResolverService } from './food-ingredient-resolver/food-ingredient-resolver.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { SharedModule } from '../shared/shared.module';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ContextMenuModule } from 'primeng/contextmenu';
import { ToastModule } from 'primeng/toast';
import { FieldsetModule } from 'primeng/fieldset';

const routes: Array<Route> = [
  {
    path: 'food-ingredient',
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
          model: FoodIngredientResolverService
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
    InputTextModule,
    FormsModule,
    DropdownModule,
    SharedModule,
    ReactiveFormsModule,
    ToggleButtonModule,
    ContextMenuModule,
    ToastModule,
    FieldsetModule
  ]
})
export class FoodIngredientModule { }
