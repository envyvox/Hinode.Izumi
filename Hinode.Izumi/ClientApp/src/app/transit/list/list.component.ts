import { Component, OnInit } from '@angular/core';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { TransitService, TransitWebModel, Location } from '../../shared/web.api.service';
import { EnumEx } from '../../shared/enum.extensions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  location = Location;
  locations: SelectItem[];
  data: TransitWebModel[];
  selectedItem: TransitWebModel;
  menuItems: MenuItem[];

  constructor(private _transitService: TransitService,
              private _messageService: MessageService,
              private _router: Router) {
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
    this._transitService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  remove( selectedItem: TransitWebModel ) {
    this._transitService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Transit deleted!'});
  }

  edit( selectedItem: TransitWebModel ) {
    this._router.navigateByUrl(`/transit/edit/${selectedItem.id}`);
  }

}
