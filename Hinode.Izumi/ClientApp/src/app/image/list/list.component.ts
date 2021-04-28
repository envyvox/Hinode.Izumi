import { Component, OnInit } from '@angular/core';
import { Image, ImageService, ImageWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  data: ImageWebModel[];
  selectedItem: ImageWebModel;
  menuItems: MenuItem[];
  image = Image;

  constructor( private _imageService: ImageService,
               private _messageService: MessageService,
               public _router: Router) { }

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
    this._imageService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  edit( selectedItem: ImageWebModel ) {
    this._router.navigateByUrl(`/image/edit/${selectedItem.id}`);
  }

  upload() {
    this._imageService
        .upload()
        .subscribe(() => {
          this.refresh();
        });
  }
}
