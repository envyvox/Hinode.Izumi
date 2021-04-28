import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService, SelectItem, Message } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { Property, PropertyCategory, WorldPropertyService, WorldPropertyWebModel } from '../../shared/web.api.service';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit, OnDestroy {
  private _destroyed: Subject<unknown>;

  form: FormGroup;
  isSaving: boolean;

  propertyCategories: SelectItem[];
  properties: SelectItem[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _worldPropertyService: WorldPropertyService,
              private _messageService: MessageService) {
    this._destroyed = new Subject();
    this.propertyCategories = EnumEx.getNamesAndValues(PropertyCategory);
    this.properties = EnumEx.getNamesAndValues(Property);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      propertyCategory: [1, Validators.required],
      property: [1, Validators.required],
      value: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: WorldPropertyWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new WorldPropertyWebModel();
        model.init({id: 0});
      }

      this.form.patchValue(model);
    });
  }

  ngOnDestroy(): void {
    this._destroyed.next();
    this._destroyed.complete();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const model = new WorldPropertyWebModel(this.form.value);

    if (model.value < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Value must be greater than zero!'});
      return;
    }

    this.isSaving = true;
    model.updatedAt = new Date();

    this._worldPropertyService.edit(model.id, model)
        .subscribe(x => {
          this.form.patchValue(x);
          this.isSaving = false;
        });

    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Property updated!'});
  }

}
