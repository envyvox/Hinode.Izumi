import { Component, OnInit } from '@angular/core';
import { Season, SeedService, SeedWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { EnumEx } from '../../shared/enum.extensions';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  season = Season;
  seasons: SelectItem[];
  data: SeedWebModel[];
  selectedItem: SeedWebModel;
  menuItems: MenuItem[];

  constructor(private _seedService: SeedService,
              private _messageService: MessageService,
              private _router: Router) {
    this.seasons = EnumEx.getNamesAndValues(Season);
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
    this._seedService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  remove( selectedItem: SeedWebModel ) {
    this._seedService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Seed deleted!'});
  }

  edit( selectedItem: SeedWebModel ) {
    this._router.navigateByUrl(`/seed/edit/${selectedItem.id}`);
  }
}
