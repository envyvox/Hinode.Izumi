import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService, SelectItem } from 'primeng/api';
import { FishRarity, FishService, FishWebModel, Season, TimesDay, Weather } from '../../shared/web.api.service';
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

  seasons: SelectItem[];
  rarities: SelectItem[];
  weathers: SelectItem[];
  timesDays: SelectItem[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _fishService: FishService) {
    this._destroyed = new Subject();
    this.seasons = EnumEx.getNamesAndValues(Season);
    this.rarities = EnumEx.getNamesAndValues(FishRarity);
    this.weathers = EnumEx.getNamesAndValues(Weather);
    this.timesDays = EnumEx.getNamesAndValues(TimesDay);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      name: [null, Validators.required],
      rarity: [1, Validators.required],
      seasons: [[0], Validators.required],
      weather: [0, Validators.required],
      timesDay: [0, Validators.required],
      price: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: FishWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new FishWebModel();
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

    const model = new FishWebModel(this.form.value);

    if (model.price < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Price must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._fishService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/fish/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Fish added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._fishService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Fish updated!'});
          });
    }
  }

}
