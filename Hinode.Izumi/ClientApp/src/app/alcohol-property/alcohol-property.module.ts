import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListComponent } from './list/list.component';
import { EditComponent } from './edit/edit.component';
import { Route, RouterModule } from '@angular/router';
import { AlcoholPropertyResolverService } from './alcohol-property-resolver/alcohol-property-resolver.service';
import { SharedModule } from '../shared/shared.module';
import { TableModule } from 'primeng/table';
import { ContextMenuModule } from 'primeng/contextmenu';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { FieldsetModule } from 'primeng/fieldset';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ToasterModule } from 'angular2-toaster';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { PaginatorModule } from 'primeng/paginator';
import { InputNumberModule } from 'primeng/inputnumber';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { ListboxModule } from 'primeng/listbox';
import { SliderModule } from 'primeng/slider';
import { MessagesModule } from 'primeng/messages';
import { DropdownModule } from 'primeng/dropdown';

const routes: Array<Route> = [
  {
    path: 'alcohol-property',
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
          model: AlcoholPropertyResolverService
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
    ReactiveFormsModule,
    FieldsetModule,
    DropdownModule,
    InputTextModule,
    ButtonModule,
    RippleModule,
    ContextMenuModule,
    TableModule,
    SharedModule,
  ]
})
export class AlcoholPropertyModule { }
