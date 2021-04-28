import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { Route, RouterModule } from '@angular/router';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { FishResolverService } from './fish-resolver/fish-resolver.service';
import { ContextMenuModule } from 'primeng/contextmenu';
import { FieldsetModule } from 'primeng/fieldset';

const routes: Array<Route> = [
    {
        path: 'fish',
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
                    model: FishResolverService
                }
            }
        ]
    }
];

@NgModule({
  declarations: [
          ListComponent,
          EditComponent
      ],
    imports:[
        CommonModule,
        RouterModule.forChild(routes),
        TableModule,
        ButtonModule,
        RippleModule,
        RouterModule,
        InputTextModule,
        DropdownModule,
        FormsModule,
        SharedModule,
        ReactiveFormsModule,
        MultiSelectModule,
        ContextMenuModule,
        FieldsetModule
    ]
})
export class FishModule { }
