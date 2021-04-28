import { Component, OnInit } from '@angular/core';
import { AlcoholService, AlcoholWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  data: AlcoholWebModel[];
  selectedItem: AlcoholWebModel;
  menuItems: MenuItem[];

  constructor(private _messageService: MessageService,
              private _router: Router,
              private _alcoholService: AlcoholService) { }

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
    this._alcoholService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  edit( selectedItem: AlcoholWebModel ) {
    this._router.navigateByUrl(`/alcohol/edit/${selectedItem.id}`);
  }

  remove( selectedItem: AlcoholWebModel ) {
    this._alcoholService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Alcohol deleted!'});
  }
}
