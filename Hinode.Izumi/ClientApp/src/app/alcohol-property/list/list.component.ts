import { Component, OnInit } from '@angular/core';
import { AlcoholProperty, AlcoholPropertyService, AlcoholPropertyWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  properties: SelectItem[];
  property = AlcoholProperty;
  data: AlcoholPropertyWebModel[];
  selectedItem: AlcoholPropertyWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _alcoholPropertyService: AlcoholPropertyService) {
    this.properties = EnumEx.getNamesAndValues(AlcoholProperty);
  }

  ngOnInit() {
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
    this._alcoholPropertyService
        .listAll()
        .subscribe(x => {
          this.data = x;
        });
  }

  upload() {
    this._alcoholPropertyService
        .upload()
        .subscribe(() => {
          this.refresh();
          this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Properties uploaded!'});
        });
  }

  edit( selectedItem: AlcoholPropertyWebModel ) {
    this._router.navigateByUrl(`/alcohol-property/edit/${selectedItem.id}`);
  }
}
