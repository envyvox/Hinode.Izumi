import { Component, OnInit } from '@angular/core';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { LocalizationCategory, LocalizationService, LocalizationWebModel } from '../../shared/web.api.service';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  localizationCategories: SelectItem[];
  localizationCategory = LocalizationCategory;
  data: LocalizationWebModel[];
  selectedItem: LocalizationWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _localizationService: LocalizationService) {
    this.localizationCategories = EnumEx.getNamesAndValues(LocalizationCategory);
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
    this._localizationService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  upload() {
    this._localizationService
        .upload()
        .subscribe(() => {
          this.refresh();
          this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Localizations uploaded!'});
        });
  }

  edit( selectedItem: LocalizationWebModel ) {
    this._router.navigateByUrl(`/localization/edit/${selectedItem.id}`);
  }

}
