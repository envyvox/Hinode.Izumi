import { Component, OnInit } from '@angular/core';
import { GatheringService, GatheringWebModel, Location } from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  location = Location;
  locations: SelectItem[];
  data: GatheringWebModel[];
  selectedItem: GatheringWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _gatheringService: GatheringService) {
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
    this._gatheringService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  edit( selectedItem: GatheringWebModel ) {
    this._router.navigateByUrl(`/gathering/edit/${selectedItem.id}`);
  }

  remove( selectedItem: GatheringWebModel ) {
    this._gatheringService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Gathering deleted!'});
  }
}
