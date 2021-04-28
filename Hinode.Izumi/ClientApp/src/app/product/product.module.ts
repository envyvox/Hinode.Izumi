import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { ProductResolverService } from './product-resolver/product-resolver.service';
import { TableModule } from 'primeng/table';
import { ContextMenuModule } from 'primeng/contextmenu';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { FieldsetModule } from 'primeng/fieldset';
import { ReactiveFormsModule } from '@angular/forms';

const routes: Array<Route> = [
  {
    path: 'product',
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
          model: ProductResolverService
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
    ContextMenuModule,
    RippleModule,
    ButtonModule,
    InputTextModule,
    FieldsetModule,
    ReactiveFormsModule,
  ]
})
export class ProductModule { }
