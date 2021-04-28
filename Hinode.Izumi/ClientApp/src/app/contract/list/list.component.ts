import { Component, OnInit } from '@angular/core';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { ContractService, ContractWebModel, Location } from '../../shared/web.api.service';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  locations: SelectItem[];
  location = Location;
  data: ContractWebModel[];
  selectedItem: ContractWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _contractService: ContractService) {
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
    this._contractService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  edit( selectedItem: ContractWebModel ) {
    this._router.navigateByUrl(`/contract/edit/${selectedItem.id}`);
  }

  remove( selectedItem: ContractWebModel ) {
    this._contractService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Contract deleted!'});
  }
}
