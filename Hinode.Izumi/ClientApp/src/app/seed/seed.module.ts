import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { SeedResolverService } from './seed-resolver/seed-resolver.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { InputTextModule } from 'primeng/inputtext';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { ContextMenuModule } from 'primeng/contextmenu';
import { MultiSelectModule } from 'primeng/multiselect';
import { FieldsetModule } from 'primeng/fieldset';

const routes: Array<Route> = [
  {
    path: 'seed',
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
          model: SeedResolverService
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
        DropdownModule,
        FormsModule,
        SharedModule,
        InputTextModule,
        ReactiveFormsModule,
        ToggleButtonModule,
        ContextMenuModule,
        MultiSelectModule,
        FieldsetModule
    ]
})
export class SeedModule { }
