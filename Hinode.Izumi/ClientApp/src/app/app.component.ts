import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  items: MenuItem[];

  constructor(private primengConfig: PrimeNGConfig) {}

  ngOnInit() {
    this.primengConfig.ripple = true;
    this.items = [
      {
        label: 'Database',
        icon: 'pi pi-fw pi-folder',
        items: [
          {
            label: 'Properties',
            icon: 'pi pi-fw pi-folder',
            items: [
              {
                label: 'World Properties',
                icon: 'pi pi-fw pi-file',
                routerLink: ['/world-property']
              },
              {
                label: 'Mastery Properties',
                icon: 'pi pi-fw pi-file',
                routerLink: ['/mastery-property']
              }
            ]
          },
          {
            label: 'Resources',
            icon: 'pi pi-fw pi-folder',
            items: [
              {
                label: 'Gathering',
                icon: 'pi pi-fw pi-folder',
                items: [
                  {
                    label: 'Gathering',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/gathering']
                  },
                  {
                    label: 'Properties',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/gathering-property']
                  }
                ]
              },
              {
                label: 'Crafting',
                icon: 'pi pi-fw pi-folder',
                items: [
                  {
                    label: 'Crafting',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/crafting']
                  },
                  {
                    label: 'Ingredients',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/crafting-ingredient']
                  },
                  {
                    label: 'Properties',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/crafting-property']
                  }
                ]
              },
              {
                label: 'Food',
                icon: 'pi pi-fw pi-folder',
                items: [
                  {
                    label: 'Food',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/food']
                  },
                  {
                    label: 'Ingredients',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/food-ingredient']
                  }
                ]
              },
              {
                label: 'Alcohol',
                icon: 'pi pi-fw pi-folder',
                items: [
                  {
                    label: 'Alcohol',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/alcohol']
                  },
                  {
                    label: 'Properties',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/alcohol-property']
                  },
                  {
                    label: 'Ingredients',
                    icon: 'pi pi-fw pi-file',
                    routerLink: ['/alcohol-ingredient']
                  }
                ]
              },
              {
                label: 'Products',
                icon: 'pi pi-fw pi-file',
                routerLink: ['/product']
              },
              {
                label: 'Seeds',
                icon: 'pi pi-fw pi-file',
                routerLink: ['/seed']
              },
              {
                label: 'Fish',
                icon: 'pi pi-fw pi-file',
                routerLink: ['/fish']
              },
              {
                label: 'Drinks',
                icon: 'pi pi-fw pi-file',
                routerLink: ['/drink']
              }
            ]
          },
          {
            label: 'Users',
            icon: 'pi pi-fw pi-file',
            routerLink: ['/user']
          },
          {
            label: 'Transits',
            icon: 'pi pi-fw pi-file',
            routerLink: '/transit'
          },
          {
            label: 'Contracts',
            icon: 'pi pi-fw pi-file',
            routerLink: '/contract'
          },
          {
            label: 'Achievements',
            icon: 'pi pi-fw pi-file',
            routerLink: '/achievement'
          },
          {
            label: 'Localizations',
            icon: 'pi pi-fw pi-file',
            routerLink: '/localization'
          },
          {
            label: 'Images',
            icon: 'pi pi-fw pi-file',
            routerLink: ['/image']
          },
          {
            label: 'Emotes',
            icon: 'pi pi-fw pi-file',
            routerLink: ['/emote']
          }
        ]
      },
      {
        label: 'Hangfire',
        icon: 'pi pi-fw pi-globe',
        url: '/hangfire'
      },
      {
        label: 'API',
        icon: 'pi pi-fw pi-share-alt',
        url: '/swagger'
      }
    ];
  }

}
