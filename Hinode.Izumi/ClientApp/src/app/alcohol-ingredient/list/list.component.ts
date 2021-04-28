import { Component, OnInit } from '@angular/core';
import {
  AlcoholIngredientService,
  AlcoholIngredientWebModel,
  AlcoholService,
  AlcoholWebModel,
  IngredientCategory
} from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  ingredientCategory = IngredientCategory;
  ingredientCategories: SelectItem[];
  alcohols: AlcoholWebModel[];
  data: AlcoholIngredientWebModel[];
  selectedItem: AlcoholIngredientWebModel;
  menuItems: MenuItem[];

  constructor(private _router: Router,
              private _messageService: MessageService,
              private _alcoholIngredientService: AlcoholIngredientService,
              private _alcoholService: AlcoholService) {
    this.ingredientCategories = EnumEx.getNamesAndValues(IngredientCategory);
    this._alcoholService
        .list()
        .subscribe(x => {
          this.alcohols = x;
        })
  }

  ngOnInit(): void {
    this.refresh();

    this.menuItems = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => this.edit(this.selectedItem)
      },
      {
        label: 'Remove',
        icon: 'pi pi-trash',
        command: () => this.remove(this.selectedItem)
      }
    ];
  }

  refresh() {
    this._alcoholIngredientService
        .listAll()
        .subscribe(x => {
          this.data = x;
        });
  }

  edit( selectedItem: AlcoholIngredientWebModel ) {
    this._router.navigateByUrl(`/alcohol-ingredient/edit/${selectedItem.id}`);
  }

  remove( selectedItem: AlcoholIngredientWebModel ) {
    this._alcoholIngredientService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Crafting ingredient deleted!'});
  }
}
