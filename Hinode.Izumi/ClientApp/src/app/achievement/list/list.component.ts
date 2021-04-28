import { Component, OnInit } from '@angular/core';
import { MenuItem, MessageService, SelectItem } from 'primeng/api';
import { Achievement, AchievementCategory, AchievementReward, AchievementService, AchievementWebModel } from '../../shared/web.api.service';
import { Router } from '@angular/router';
import { EnumEx } from '../../shared/enum.extensions';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  achievementCategories: SelectItem[];
  achievementCategory = AchievementCategory;
  achievements: SelectItem[];
  achievement = Achievement;
  achievementRewardTypes: SelectItem[];
  achievementRewardType = AchievementReward;
  data: AchievementWebModel[];
  selectedItem: AchievementWebModel;
  menuItems: MenuItem[];

  constructor(private _router: Router,
              private _messageService: MessageService,
              private _achievementService: AchievementService) {
    this.achievementCategories = EnumEx.getNamesAndValues(AchievementCategory);
    this.achievements = EnumEx.getNamesAndValues(Achievement);
    this.achievementRewardTypes = EnumEx.getNamesAndValues(AchievementReward);
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
    this._achievementService
        .list()
        .subscribe(x => {
          this.data = x;
        });
  }

  upload() {
    this._achievementService
        .upload()
        .subscribe(() => {
          this.refresh();
          this._messageService.add({severity: 'success', key: 'success', summary: 'Success', detail: 'Achievements uploaded!'});
        });
  }

  edit( selectedItem: AchievementWebModel ) {
    this._router.navigateByUrl(`/achievement/edit/${selectedItem.id}`);
  }
}
