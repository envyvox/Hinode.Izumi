import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Season, SeedService, SeedWebModel, TransitWebModel } from '../../shared/web.api.service';
import { MessageService, SelectItem } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
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

  season = Season;
  seasons: SelectItem[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _seedService: SeedService) {
    this._destroyed = new Subject();
    this.seasons = EnumEx.getNamesAndValues(Season);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      name: [null, Validators.required],
      season: [0, Validators.required],
      growth: [0, Validators.required],
      reGrowth: [0, Validators.required],
      price: [0, Validators.required],
      multiply: [false, Validators.required],
      cropName: [null, Validators.required],
      cropPrice: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: SeedWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new SeedWebModel();
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

    const model = new SeedWebModel(this.form.value);

    if (model.growth <= 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Growth must be greater then 0!'});
      return;
    }
    else if (model.reGrowth < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Re-Growth must be greater or equal to 0!'});
      return;
    }
    else if (model.price < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Price must be greater then 0!'});
      return;
    }
    else if (model.cropPrice < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Price must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._seedService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/seed/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Seed added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._seedService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Seed updated!'});
          });
    }
  }

}
