import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Route, RouterModule } from '@angular/router';
import { ToasterModule } from 'angular2-toaster';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { PaginatorModule } from 'primeng/paginator';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ListboxModule } from 'primeng/listbox';
import { SliderModule } from 'primeng/slider';
import { ListComponent } from './list/list.component';
import { MessagesModule } from 'primeng/messages';

const routes: Array<Route> = [
    {
        path: 'emote',
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'list'
            },
            {
                path: 'list',
                component: ListComponent
            }
        ]
    }];


@NgModule({
    declarations: [ListComponent],
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
        MessagesModule
    ]
})
export class EmoteModule {
}
