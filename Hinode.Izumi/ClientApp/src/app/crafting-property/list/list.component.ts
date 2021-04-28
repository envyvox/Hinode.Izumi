import { Component, OnInit } from '@angular/core';
import {
  CraftingProperty,
  CraftingPropertyService,
  CraftingPropertyWebModel, CraftingService, CraftingWebModel
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

  craftingProperty = CraftingProperty;
  craftingPropertyCategories: SelectItem[];
  craftings: CraftingWebModel[];
  data: CraftingPropertyWebModel[];
  selectedItem: CraftingPropertyWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _craftingPropertyService: CraftingPropertyService,
              private _craftingService: CraftingService) {
    this._craftingService
        .list()
        .subscribe(x => {
          this.craftings = x;
        });
  }

  ngOnInit(): void {
    this.refresh();

    this.menuItems = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => this.edit(this.selectedItem)
      }
    ];
  }

  refresh() {
    this._craftingPropertyService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  upload() {
    this._craftingPropertyService
        .upload()
        .subscribe(() => {
          this.refresh();
          this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Properties uploaded!'});
        });
  }

  edit( selectedItem: CraftingPropertyWebModel ) {
    this._router.navigateByUrl(`/crafting-property/edit/${selectedItem.id}`);
  }
}
