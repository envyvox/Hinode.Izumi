import { Component, OnInit } from '@angular/core';
import { Gender, Title, Location, UserService, UserWebModel } from '../../shared/web.api.service';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
    selector:'app-list',
    templateUrl:'./list.component.html',
    styleUrls:['./list.component.scss']
})
export class ListComponent implements OnInit {

    title = Title;
    gender = Gender;
    location = Location;
    titles: SelectItem[];
    genders: SelectItem[];
    locations: SelectItem[];
    data: UserWebModel[];
    selectedItem: UserWebModel;
    menuItems: MenuItem[];

    constructor(private _userService: UserService,
                private _messageService: MessageService,
                private _router: Router) {
        this.titles = EnumEx.getNamesAndValues(Title);
        this.genders = EnumEx.getNamesAndValues(Gender);
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

    remove( selectedItem: UserWebModel ) {
        this._userService
            .remove(selectedItem.id.toString())
            .subscribe(() => {
                this.refresh();
            });
        this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'User deleted from database!'});
    }

    refresh() {
        this._userService
            .list()
            .subscribe(x => {
                this.data = x;
            });
    }

    private edit( selectedItem: UserWebModel ) {
        this._router.navigateByUrl(`/user/edit/${selectedItem.id}`);
    }
}
