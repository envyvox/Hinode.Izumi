import { Component, OnInit } from '@angular/core';
import {
  Property,
  PropertyCategory,
  WorldPropertyService,
  WorldPropertyWebModel
} from '../../shared/web.api.service';
import {MenuItem, MessageService, SelectItem} from 'primeng/api';
import { EnumEx } from '../../shared/enum.extensions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  propertyCategories: SelectItem[];
  propertyCategory = PropertyCategory;
  property = Property;
  data: WorldPropertyWebModel[];
  selectedItem: WorldPropertyWebModel;
  menuItems: MenuItem[];

  constructor(private _worldPropertyService: WorldPropertyService,
              private _messageService: MessageService,
              private _router: Router) {
    this.propertyCategories = EnumEx.getNamesAndValues(PropertyCategory);
  }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this._worldPropertyService
        .list()
        .subscribe(x => {
          this.data = x;
        });

    this.menuItems = [
      {
        label: 'Edit',
        icon: 'pi pi-pencil',
        command: () => this.edit(this.selectedItem)
      }];
  }

  upload() {
    this._worldPropertyService
        .upload()
        .subscribe(() => {
          this.refresh();
          this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Properties uploaded!'});
        });
  }

  private edit(selectedItem: WorldPropertyWebModel) {
    this._router.navigateByUrl(`/world-property/edit/${selectedItem.id}`);
  }
}
