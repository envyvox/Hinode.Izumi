import { Component, OnInit } from '@angular/core';
import { EmoteService, EmoteWebModel } from '../../shared/web.api.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  data: EmoteWebModel[];
  cols: any[];

  constructor(private _emoteService: EmoteService) { }

  ngOnInit(): void {
    this.refresh();

    this.cols = [
      { field: 'id', header: 'ID' },
      { field: 'name', header: 'Name' },
      { field: 'code', header: 'Code' },
      { field: 'createdAt', header: 'Created At' },
      { field: 'updatedAt', header: 'Updated At' }
    ];
  }

  refresh() {
    this._emoteService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  upload() {
    this._emoteService
        .upload()
        .subscribe(() => {
          this.refresh();
        });
  }

}
