import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { ContextMenuModule } from 'primeng/contextmenu';
import { TableModule } from 'primeng/table';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { Route, RouterModule } from '@angular/router';
import { GatheringResolverService } from './gathering-resolver/gathering-resolver.service';
import { FieldsetModule } from 'primeng/fieldset';
import { MultiSelectModule } from "primeng/multiselect";

const routes: Array<Route> = [
    {
        path: 'gathering',
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
                    model: GatheringResolverService
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
        DropdownModule,
        FormsModule,
        SharedModule,
        ReactiveFormsModule,
        FieldsetModule,
        MultiSelectModule
    ]
})
export class GatheringModule { }
