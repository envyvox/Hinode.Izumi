import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { FoodIngredientService, FoodIngredientWebModel, FoodService, FoodWebModel, IngredientCategory } from '../../shared/web.api.service';
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
  ingredients: FoodIngredientWebModel[];
  selectedItem: FoodIngredientWebModel;
  menuItems: MenuItem[];
  ingredientCategory = IngredientCategory;
  ingredientCategories: SelectItem[];
  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _foodService: FoodService,
              private _foodIngredientService: FoodIngredientService) {
    this._destroyed = new Subject();
    this.ingredientCategories = EnumEx.getNamesAndValues(IngredientCategory);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      name: [null, Validators.required],
      mastery: [0, Validators.required],
      time: [0, Validators.required],
      recipeSellable: [false],
      event: [false],
      energy: [0, Validators.required],
      costPrice: [0],
      cookingPrice: [0],
      npcPrice: [0],
      profit: [0],
      recipePrice: [0],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: FoodWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new FoodWebModel();
        model.init({id: 0});
      }

      this.form.patchValue(model);
    });

    this._foodIngredientService
        .list(model.id)
        .subscribe(x => {
          this.ingredients = x;
        })
  }

  ngOnDestroy(): void {
    this._destroyed.next();
    this._destroyed.complete();
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const model = new FoodWebModel(this.form.value);

    if (model.mastery < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Mastery must be greater then 0!'});
      return;
    }
    else if (model.time < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Time must be greater then 0!'});
      return;
    }
    else if (model.energy < 0 || model.energy > 100) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Energy must be between 0 and 100!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._foodService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/food/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Food added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._foodService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Food updated!'});
          });
    }
  }

}
