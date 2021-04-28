import { Component, OnInit } from '@angular/core';
import { FoodService, FoodWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  masteryBrackets: SelectItem[];
  data: FoodWebModel[];
  selectedItem: FoodWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _foodService: FoodService,
              private _router: Router) {
    this.masteryBrackets = [
      { label: '0', value: '0' },
      { label: '50', value: '50' },
      { label: '100', value: '100' },
      { label: '150', value: '150' },
      { label: '200', value: '200' },
      { label: '250', value: '250' }
    ];
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
    this._foodService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  remove( selectedItem: FoodWebModel ) {
    this._foodService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Food deleted!'});
  }

  edit( selectedItem: FoodWebModel ) {
    this._router.navigateByUrl(`/food/edit/${selectedItem.id}`);
  }
}
