import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService, SelectItem } from 'primeng/api';
import { ActivatedRoute, Router } from '@angular/router';
import {
  Achievement,
  AchievementCategory,
  AchievementReward,
  AchievementService, AchievementWebModel,
} from '../../shared/web.api.service';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent implements OnInit, OnDestroy {
  private _destroyed: Subject<unknown>;

  form: FormGroup;
  isSaving: boolean;

  achievementCategories: SelectItem[];
  achievements: SelectItem[];
  achievementRewardTypes: SelectItem[];

  constructor(private _router: Router,
              private _builder: FormBuilder,
              private _route: ActivatedRoute,
              private _messageService: MessageService,
              private _achievementService: AchievementService) {
    this._destroyed = new Subject();
    this.achievementCategories = EnumEx.getNamesAndValues(AchievementCategory);
    this.achievements = EnumEx.getNamesAndValues(Achievement);
    this.achievementRewardTypes = EnumEx.getNamesAndValues(AchievementReward);
  }

  ngOnInit(): void {
    const template = {
      id: [0, Validators.required],
      category: [0, Validators.required],
      type: [0, Validators.required],
      reward: [0, Validators.required],
      number: [0, Validators.required],
      createdAt: [null],
      updatedAt: [null]
    };

    this.form = this._builder.group(template);
    let model: AchievementWebModel;

    this._route.data.subscribe(x => {
      if (x.model) {
        model = x.model;
      }
      else {
        model = new AchievementWebModel();
        model.init({id: 0});
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

    const model = new AchievementWebModel(this.form.value);

    if (model.number < 0) {
      this._messageService.add({severity: 'error', key: 'error', summary: 'Error', detail: 'Number must be greater than zero!'});
      return;
    }

    this.isSaving = true;
    model.updatedAt = new Date();

    this._achievementService.edit(model.id, model)
        .subscribe(x => {
          this.form.patchValue(x);
          this.isSaving = false;
        });

    this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Achievement updated!'});
  }

}
