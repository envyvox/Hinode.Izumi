import { Component, OnInit } from '@angular/core';
import {
  MasteryProperty,
  MasteryPropertyCategory,
  MasteryPropertyService,
  MasteryPropertyWebModel,
} from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { EnumEx } from '../../shared/enum.extensions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  propertyCategories: SelectItem[];
  masteryPropertyCategory = MasteryPropertyCategory;
  masteryProperty = MasteryProperty;
  data: MasteryPropertyWebModel[];
  selectedItem: MasteryPropertyWebModel;
  menuItems: MenuItem[];

  constructor(private _masteryPropertyService: MasteryPropertyService,
              private _messageService: MessageService,
              private _router: Router) {
    this.propertyCategories = EnumEx.getNamesAndValues(MasteryPropertyCategory);
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
    this._masteryPropertyService
        .list()
        .subscribe(x => {
            this.data = x;
        });
  }

  upload() {
    this._masteryPropertyService
        .upload()
        .subscribe(() => {
          this.refresh();
          this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Properties uploaded!'});
        });
  }

  edit( selectedItem: MasteryPropertyWebModel) {
    this._router.navigateByUrl(`/mastery-property/edit/${selectedItem.id}`);
  }

}
