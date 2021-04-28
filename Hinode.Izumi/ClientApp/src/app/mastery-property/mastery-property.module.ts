import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { ToasterModule } from 'angular2-toaster';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ListComponent } from './list/list.component';
import { SharedModule } from '../shared/shared.module';
import { EditComponent } from './edit/edit.component';
import { MasteryPropertyResolverService } from './mastery-property-resolver/mastery-property-resolver.service';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { PaginatorModule } from 'primeng/paginator';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ListboxModule } from 'primeng/listbox';
import { SliderModule } from 'primeng/slider';
import { MessagesModule } from 'primeng/messages';
import { ContextMenuModule } from 'primeng/contextmenu';
import { FieldsetModule } from 'primeng/fieldset';

const routes: Array<Route> = [
    {
        path: 'mastery-property',
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
                    model: MasteryPropertyResolverService
                }
            }
        ]
    }];


@NgModule({
    declarations: [ListComponent, EditComponent],
    imports:[
        CommonModule,
        RouterModule.forChild(routes),
        ToasterModule,
        NgbModule,
        FormsModule,
        SharedModule,
        ReactiveFormsModule,
        TableModule,
        ButtonModule,
        RippleModule,
        PaginatorModule,
        InputTextModule,
        InputNumberModule,
        InputTextareaModule,
        ListboxModule,
        SliderModule,
        MessagesModule,
        ContextMenuModule,
        FieldsetModule
    ]
})
export class MasteryPropertyModule {
}
