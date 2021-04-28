import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Gender, Title, Location, UserService, UserWebModel } from '../../shared/web.api.service';
import { MessageService, SelectItem, Message } from 'primeng/api';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit, OnDestroy {
  private _destroyed: Subject<unknown>;

  form: FormGroup;
  isSaving: boolean;

  titles: SelectItem[];
  genders: SelectItem[];
  locations: SelectItem[];

  msgs: Message[];

  constructor(private _route: ActivatedRoute,
              private _router: Router,
              private _builder: FormBuilder,
              private _userService: UserService,
              private _messageService: MessageService) {
    this._destroyed = new Subject();
    this.titles = EnumEx.getNamesAndValues(Title);
    this.genders = EnumEx.getNamesAndValues(Gender);
    this.locations = EnumEx.getNamesAndValues(Location);
  }

  ngOnInit(): void {
    const template = {
      id: [null, Validators.required],
      name: [null, [Validators.required]],
      about: [null],
      title: [1, [Validators.required]],
      gender: [0, [Validators.required]],
      location: [1, [Validators.required]],
      energy: [100, [Validators.required]],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this
        ._builder
        .group(template);

    let model: UserWebModel;

    this._route.data.subscribe(x => {
      if (x.user) {
        model = x.user;
      }
      else {
        model = new UserWebModel();
        model.init();
      }

      this.form.patchValue(model);
    });
  }

  ngOnDestroy(): void {
    this._destroyed.next();
    this._destroyed.complete();
  }

  save() {
    if (this.form.invalid) {
      return;
    }
    this.isSaving = true;
    const model = new UserWebModel(this.form.value);

    model.createdAt = new Date();
    model.updatedAt = new Date();

    this._userService.add(model).subscribe(x => {
      this.form.patchValue(x);
      this.isSaving = false;
    });

    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Users database updated!'});
  }
}
