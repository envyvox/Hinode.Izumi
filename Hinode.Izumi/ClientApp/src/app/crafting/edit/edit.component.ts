import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {MenuItem, MessageService, SelectItem} from 'primeng/api';
import {
  CraftingIngredientService,
  CraftingIngredientWebModel,
  CraftingService,
  CraftingWebModel,
  IngredientCategory,
  Location
} from '../../shared/web.api.service';
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
  ingredients: CraftingIngredientWebModel[];
  selectedItem: CraftingIngredientWebModel;
  menuItems: MenuItem[];
  ingredientCategory = IngredientCategory;
  ingredientCategories: SelectItem[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _messageService: MessageService,
              private _craftingService: CraftingService,
              private _craftingIngredientService: CraftingIngredientService) {
    this._destroyed = new Subject();
    this.locations = EnumEx.getNamesAndValues(Location);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      name: [null, Validators.required],
      time: [0, Validators.required],
      location: [0, Validators.required],
      costPrice: [0],
      craftingPrice: [0],
      npcPrice: [0],
      profit: [0],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: CraftingWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new CraftingWebModel();
        model.init({id: 0});
      }

      this.form.patchValue(model);
    });

    this._craftingIngredientService
        .list(model.id)
        .subscribe(x => {
          this.ingredients = x;
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

    const model = new CraftingWebModel(this.form.value);

    if (model.time < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Time must be greater then 0!'});
      return;
    }

    this.isSaving = true;
    if (model.id === 0) {
      model.createdAt = new Date();
      model.updatedAt = new Date();
      this._craftingService
          .add(model)
          .subscribe(x => {
            this.isSaving = false;
            this._router.navigateByUrl(`/crafting/edit/${x.id}`);
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Crafting added!'});
          });
    }
    else {
      model.updatedAt = new Date();
      this._craftingService
          .edit(model.id, model)
          .subscribe(x => {
            this.form.patchValue(x);
            this.isSaving = false;
            this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Crafting updated!'});
          });
    }
  }

}
