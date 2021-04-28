import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {MenuItem, MessageService, SelectItem} from 'primeng/api';
import {
  AlcoholIngredientService,
  AlcoholIngredientWebModel, AlcoholProperty, AlcoholPropertyService, AlcoholPropertyWebModel,
  AlcoholService,
  AlcoholWebModel,
  IngredientCategory
} from '../../shared/web.api.service';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit, OnDestroy {
  private _destroyed: Subject<unknown>;

  form: FormGroup;
  isSaving: boolean;
  ingredients: AlcoholIngredientWebModel[];
  selectedItem: AlcoholIngredientWebModel;
  properties: AlcoholPropertyWebModel[];
  property = AlcoholProperty;
  selectedProperty: AlcoholPropertyWebModel;
  menuItems: MenuItem[];
  ingredientCategory = IngredientCategory;
  ingredientCategories: SelectItem[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _alcoholService: AlcoholService,
              private _alcoholIngredientService: AlcoholIngredientService,
              private _alcoholPropertyService: AlcoholPropertyService) {
    this._destroyed = new Subject();
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      name: [null, Validators.required],
      time: [0, Validators.required],
      costPrice: [0],
      craftingPrice: [0],
      npcPrice: [0],
      profit: [0],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: AlcoholWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new AlcoholWebModel();
        model.init({id: 0});
      }

      this.form.patchValue(model);
    });

    this._alcoholIngredientService
        .list(model.id)
        .subscribe(x => this.ingredients = x);

    this._alcoholPropertyService
        .list(model.id)
        .subscribe(x => this.properties = x);
  }

  ngOnDestroy(): void {
    this._destroyed.next();
    this._destroyed.complete();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const model = new AlcoholWebModel(this.form.value);

    if (model.time < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Time must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._alcoholService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/alcohol/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Alcohol added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._alcoholService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Alcohol updated!'});
          });
    }
  }

}
