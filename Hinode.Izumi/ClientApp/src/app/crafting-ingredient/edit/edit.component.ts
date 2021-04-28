import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageService, SelectItem } from 'primeng/api';
import { EnumEx } from '../../shared/enum.extensions';
import {
  AlcoholService,
  AlcoholWebModel, CraftingIngredientService,
  CraftingIngredientWebModel, CraftingService,
  CraftingWebModel, CropService, CropWebModel, DrinkService, DrinkWebModel, FoodService, FoodWebModel, GatheringService,
  GatheringWebModel,
  IngredientCategory, ProductService,
  ProductWebModel
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

  gatherings: GatheringWebModel[];
  products: ProductWebModel[];
  craftings: CraftingWebModel[];
  alcohols: AlcoholWebModel[];
  drinks: DrinkWebModel[];
  crops: CropWebModel[];
  foods: FoodWebModel[];

  gatheringsDisabled = true;
  productsDisabled = true;
  craftingsDisabled = true;
  alcoholsDisabled = true;
  drinksDisabled = true;
  cropsDisabled = true;
  foodsDisabled = true;

  ingredientCategories: SelectItem[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _gatheringService: GatheringService,
              private _productService: ProductService,
              private _craftingService: CraftingService,
              private _alcoholService: AlcoholService,
              private _drinkService: DrinkService,
              private _cropService: CropService,
              private _foodService: FoodService,
              private _craftingIngredientService: CraftingIngredientService) {
    this._destroyed = new Subject();
    this.ingredientCategories = EnumEx.getNamesAndValues(IngredientCategory);

    this._gatheringService
        .list()
        .subscribe(x => {
          this.gatherings = x;
        });
    this._productService
        .list()
        .subscribe(x => {
          this.products = x;
        });
    this._craftingService
        .list()
        .subscribe(x => {
          this.craftings = x;
        });
    this._alcoholService
        .list()
        .subscribe(x => {
          this.alcohols = x;
        });
    this._drinkService
        .list()
        .subscribe(x => {
          this.drinks = x;
        });
    this._cropService
        .list()
        .subscribe(x => {
          this.crops = x;
        });
    this._foodService
        .list()
        .subscribe(x => {
          this.foods = x;
        });
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      craftingId: [0, Validators.required],
      category: [0, Validators.required],
      ingredientId: [0, Validators.required],
      amount: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: CraftingIngredientWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;

        switch (model.category) {
          case IngredientCategory.Gathering:
            this.gatheringsDisabled = false;
            break;
          case IngredientCategory.Product:
            this.productsDisabled = false;
            break;
          case IngredientCategory.Crafting:
            this.craftingsDisabled = false;
            break;
          case IngredientCategory.Alcohol:
            this.alcoholsDisabled = false;
            break;
          case IngredientCategory.Drink:
            this.drinksDisabled = false;
            break;
          case IngredientCategory.Crop:
            this.cropsDisabled = false;
            break;
          case IngredientCategory.Food:
            this.foodsDisabled = false;
            break;
        }
      }
      else {
        model = new CraftingIngredientWebModel();
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

    const model = new CraftingIngredientWebModel(this.form.value);

    if (model.amount < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Amount must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._craftingIngredientService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/crafting-ingredient/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Crafting ingredient added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._craftingIngredientService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Crafting ingredient updated!'});
          });
    }
  }

  selectCategory ( value: IngredientCategory ) {
    switch (value) {
      case IngredientCategory.Gathering:
        this.gatheringsDisabled = false;
        this.productsDisabled = true;
        this.craftingsDisabled = true;
        this.alcoholsDisabled = true;
        this.drinksDisabled = true;
        this.cropsDisabled = true;
        this.foodsDisabled = true;
        break;
      case IngredientCategory.Product:
        this.gatheringsDisabled = true;
        this.productsDisabled = false;
        this.craftingsDisabled = true;
        this.alcoholsDisabled = true;
        this.drinksDisabled = true;
        this.cropsDisabled = true;
        this.foodsDisabled = true;
        break;
      case IngredientCategory.Crafting:
        this.gatheringsDisabled = true;
        this.productsDisabled = true;
        this.craftingsDisabled = false;
        this.alcoholsDisabled = true;
        this.drinksDisabled = true;
        this.cropsDisabled = true;
        this.foodsDisabled = true;
        break;
      case IngredientCategory.Alcohol:
        this.gatheringsDisabled = true;
        this.productsDisabled = true;
        this.craftingsDisabled = true;
        this.alcoholsDisabled = false;
        this.drinksDisabled = true;
        this.cropsDisabled = true;
        this.foodsDisabled = true;
        break;
      case IngredientCategory.Drink:
        this.gatheringsDisabled = true;
        this.productsDisabled = true;
        this.craftingsDisabled = true;
        this.alcoholsDisabled = true;
        this.drinksDisabled = false;
        this.cropsDisabled = true;
        this.foodsDisabled = true;
        break;
      case IngredientCategory.Crop:
        this.gatheringsDisabled = true;
        this.productsDisabled = true;
        this.craftingsDisabled = true;
        this.alcoholsDisabled = true;
        this.drinksDisabled = true;
        this.cropsDisabled = false;
        this.foodsDisabled = true;
        break;
      case IngredientCategory.Food:
        this.gatheringsDisabled = true;
        this.productsDisabled = true;
        this.craftingsDisabled = true;
        this.alcoholsDisabled = true;
        this.drinksDisabled = true;
        this.cropsDisabled = true;
        this.foodsDisabled = false;
        break;
    }
  }

}
