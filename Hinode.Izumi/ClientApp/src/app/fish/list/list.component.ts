import { Component, OnInit } from '@angular/core';
import { FishRarity, FishService, FishWebModel, Season, TimesDay, Weather } from '../../shared/web.api.service';
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
  rarity = FishRarity;
  rarities: SelectItem[];
  weather = Weather;
  weathers: SelectItem[];
  timesDay = TimesDay;
  timesDays: SelectItem[];
  data: FishWebModel[];
  selectedItem: FishWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService, private _fishService: FishService, private _router: Router) {
    this.seasons = EnumEx.getNamesAndValues(Season);
    this.rarities = EnumEx.getNamesAndValues(FishRarity);
    this.weathers = EnumEx.getNamesAndValues(Weather);
    this.timesDays = EnumEx.getNamesAndValues(TimesDay);
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
    this._fishService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  remove( selectedItem: FishWebModel ) {
    this._fishService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Fish deleted!'});
  }

  edit( selectedItem: FishWebModel ) {
    this._router.navigateByUrl(`/fish/edit/${selectedItem.id}`);
  }
}
