import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import {
  CraftingProperty,
  CraftingPropertyService,
  CraftingPropertyWebModel,
  CraftingService, CraftingWebModel
} from '../../shared/web.api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService, SelectItem } from 'primeng/api';
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

  craftingProperties: SelectItem[];
  craftings: CraftingWebModel[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _craftingPropertyService: CraftingPropertyService,
              private _craftingService: CraftingService) {
    this._destroyed = new Subject();
    this.craftingProperties = EnumEx.getNamesAndValues(CraftingProperty);

    this._craftingService
        .list()
        .subscribe(x => {
          this.craftings = x;
        });
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      craftingId: [0, Validators.required],
      craftingName: [null],
      property: [1, Validators.required],
      mastery0: [0, Validators.required],
      mastery50: [0, Validators.required],
      mastery100: [0, Validators.required],
      mastery150: [0, Validators.required],
      mastery200: [0, Validators.required],
      mastery250: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: CraftingPropertyWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new CraftingPropertyWebModel();
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

    const model = new CraftingPropertyWebModel(this.form.value);

    if (model.mastery0 < 0 || model.mastery0 > 100 ||
        model.mastery50 < 0 || model.mastery50 > 100 ||
        model.mastery100 < 0 || model.mastery100 > 100 ||
        model.mastery150 < 0 || model.mastery150 > 100 ||
        model.mastery200 < 0 || model.mastery200 > 100 ||
        model.mastery250 < 0 || model.mastery250 > 100) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Value must be between 0 and 100!'});
      return;
    }

    this.isSaving = true;
    model.updatedAt = new Date();

    this._craftingPropertyService.edit(model.id, model)
        .subscribe(x => {
          this.form.patchValue(x);
          this.isSaving = false;
        });

    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Property updated!'});
  }

}
