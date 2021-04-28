import { Component, OnInit } from '@angular/core';
import {
  GatheringProperty,
  GatheringPropertyService,
  GatheringPropertyWebModel, GatheringService,
  GatheringWebModel
} from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  gatheringProperty = GatheringProperty;
  gatheringPropertyCategories: SelectItem[];
  gatherings: GatheringWebModel[];
  data: GatheringPropertyWebModel[];
  selectedItem: GatheringPropertyWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _gatheringPropertyService: GatheringPropertyService,
              private _gatheringService: GatheringService) {
    this._gatheringService
        .list()
        .subscribe(x => {
          this.gatherings = x;
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
    this._gatheringPropertyService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  upload() {
    this._gatheringPropertyService
        .upload()
        .subscribe(() => {
          this.refresh();
          this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Properties uploaded!'});
        });
  }

  edit( selectedItem: GatheringPropertyWebModel ) {
    this._router.navigateByUrl(`/gathering-property/edit/${selectedItem.id}`);
  }

}
