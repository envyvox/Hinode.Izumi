import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService, SelectItem } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import { ContractService, ContractWebModel, Location } from '../../shared/web.api.service';
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
              private _contractService: ContractService) {
    this._destroyed = new Subject();
    this.locations = EnumEx.getNamesAndValues(Location);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      name: [null, Validators.required],
      location: [0, Validators.required],
      description: [null, Validators.required],
      time: [0, Validators.required],
      currency: [0, Validators.required],
      reputation: [0, Validators.required],
      energy: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: ContractWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new ContractWebModel();
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

    const model = new ContractWebModel(this.form.value);

    if (model.time < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Time must be greater then 0!'});
      return;
    }
    else if (model.currency < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Currency must be greater then 0!'});
      return;
    }
    else if (model.reputation < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Reputation must be greater then 0!'});
      return;
    }
    else if (model.energy < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Energy must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._contractService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/contract/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Contract added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._contractService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Contract updated!'});
          });
    }
  }

}
