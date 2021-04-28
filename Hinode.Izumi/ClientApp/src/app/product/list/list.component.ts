import { Component, OnInit } from '@angular/core';
import { ProductService, ProductWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  data: ProductWebModel[];
  selectedItem: ProductWebModel;
  menuItems: MenuItem[];

  constructor(private _router: Router,
              private _messageService: MessageService,
              private _productService: ProductService) { }

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
    this._productService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  edit( selectedItem: ProductWebModel ) {
    this._router.navigateByUrl(`/product/edit/${selectedItem.id}`);
  }

  remove( selectedItem: ProductWebModel ) {
    this._productService
        .remove(selectedItem.id)
        .subscribe(() => {
          this.refresh();
        });
    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Product deleted!'});
  }

}
