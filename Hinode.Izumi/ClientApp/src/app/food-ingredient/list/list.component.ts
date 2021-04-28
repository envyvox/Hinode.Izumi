import { Component, OnInit } from '@angular/core';
import { FoodIngredientService, FoodIngredientWebModel, FoodService, FoodWebModel, IngredientCategory } from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { EnumEx } from '../../shared/enum.extensions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  ingredientCategory = IngredientCategory;
  ingredientCategories: SelectItem[];
  foods: FoodWebModel[];
  data: FoodIngredientWebModel[];
  selectedItem: FoodIngredientWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _foodIngredientService: FoodIngredientService,
              private _router: Router,
              private _foodService: FoodService) {
    this.ingredientCategories = EnumEx.getNamesAndValues(IngredientCategory);
    this._foodService
        .list()
        .subscribe(x => {
          this.foods = x;
        });
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
    this._foodIngredientService
        .listAll()
        .subscribe(x => {
          this.data = x;
        });
  }

  remove( selectedItem: FoodIngredientWebModel ) {
    this._foodIngredientService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Food ingredient deleted!'});
  }

  edit( selectedItem: FoodIngredientWebModel ) {
    this._router.navigateByUrl(`/food-ingredient/edit/${selectedItem.id}`);
  }
}
