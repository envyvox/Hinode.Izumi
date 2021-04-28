import { Component, OnInit } from '@angular/core';
import { DrinkService, DrinkWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  data: DrinkWebModel[];
  selectedItem: DrinkWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _drinkService: DrinkService) { }

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
    this._drinkService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  edit( selectedItem: DrinkWebModel ) {
    this._router.navigateByUrl(`/drink/edit/${selectedItem.id}`);
  }

  remove( selectedItem: DrinkWebModel ) {
    this._drinkService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Drink deleted!'});
  }
}
