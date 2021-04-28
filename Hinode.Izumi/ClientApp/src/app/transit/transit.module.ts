import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { TableModule } from 'primeng/table';
import { RippleModule } from 'primeng/ripple';
import { ButtonModule } from 'primeng/button';
import { Route, RouterModule } from '@angular/router';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { MessagesModule } from 'primeng/messages';
import { TransitResolverService } from './transit-resolver/transit-resolver.service';
import { InputTextModule } from 'primeng/inputtext';
import { ContextMenuModule } from 'primeng/contextmenu';
import { FieldsetModule } from 'primeng/fieldset';

const routes: Array<Route> = [
    {
        path: 'transit',
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
                    model: TransitResolverService
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
        RippleModule,
        ButtonModule,
        RouterModule,
        DropdownModule,
        FormsModule,
        SharedModule,
        MessagesModule,
        ReactiveFormsModule,
        InputTextModule,
        ContextMenuModule,
        FieldsetModule
    ]
})
export class TransitModule { }
