import { Component, OnInit } from '@angular/core';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { CraftingService, CraftingWebModel, Location } from '../../shared/web.api.service';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  data: CraftingWebModel[];
  location = Location;
  locations: SelectItem[];
  selectedItem: CraftingWebModel;
  menuItems: MenuItem[];

  constructor(private _router: Router,
              private _messageService: MessageService,
              private _craftingService: CraftingService) {
    this.locations = EnumEx.getNamesAndValues(Location);
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
    this._craftingService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  remove( selectedItem: CraftingWebModel ) {
    this._craftingService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Crafting deleted!'});
  }

  edit( selectedItem: CraftingWebModel ) {
    this._router.navigateByUrl(`/crafting/edit/${selectedItem.id}`);
  }

}
