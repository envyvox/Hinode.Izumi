import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SelectItem, MessageService } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { TransitService, TransitWebModel, Location } from '../../shared/web.api.service';
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

  locations: SelectItem[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _transitService: TransitService) {
    this._destroyed = new Subject();
    this.locations = EnumEx.getNamesAndValues(Location);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      departure: [0, Validators.required],
      destination: [0, Validators.required],
      time: [0, Validators.required],
      price: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: TransitWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new TransitWebModel();
        model.init({id: 0});
      }

      this.form.patchValue(model);
    });
  }

  ngOnDestroy() {
    this._destroyed.next();
    this._destroyed.complete();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const model = new TransitWebModel(this.form.value);

    if (model.time < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Time must be greater then 0!'});
      return;
    }
    else if (model.price < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Price must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._transitService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/transit/edit/${x.id}`);
          });
    }
    else {
      model.updatedAt = new Date();
      this._transitService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
          });
    }

    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Transit database updated!'});
  }

}
