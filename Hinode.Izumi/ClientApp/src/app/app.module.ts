import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserModule } from './user/user.module';
import { MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { PaginatorModule } from 'primeng/paginator';
import { MenubarModule } from 'primeng/menubar';
import { InputTextModule } from 'primeng/inputtext';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { SliderModule } from 'primeng/slider';
import { EmoteModule } from './emote/emote.module';
import { ImageModule } from './image/image.module';
import { MasteryPropertyModule } from './mastery-property/mastery-property.module';
import { WorldPropertyModule } from './world-property/world-property-module';
import { MessagesModule } from 'primeng/messages';
import { MessageModule } from 'primeng/message';
import { TransitModule } from './transit/transit.module';
import { SeedModule } from './seed/seed.module';
import { FishModule } from './fish/fish.module';
import { FoodModule } from './food/food.module';
import { FoodIngredientModule } from './food-ingredient/food-ingredient.module';
import { CraftingModule } from './crafting/crafting.module';
import { CraftingIngredientModule } from './crafting-ingredient/crafting-ingredient.module';
import { GatheringModule } from './gathering/gathering.module';
import { ProductModule } from './product/product.module';
import { CraftingPropertyModule } from './crafting-property/crafting-property.module';
import { GatheringPropertyModule } from './gathering-property/gathering-property.module';
import { AlcoholModule } from './alcohol/alcohol.module';
import { DrinkModule } from './drink/drink.module';
import { AchievementModule } from './achievement/achievement.module';
import { ContractModule } from './contract/contract.module';
import { LocalizationModule } from './localization/localization.module';
import { AlcoholPropertyModule } from './alcohol-property/alcohol-property.module';
import { AlcoholIngredientModule } from './alcohol-ingredient/alcohol-ingredient.module';
import {ToastModule} from "primeng/toast";

@NgModule({
    imports: [
        HttpClientModule,
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        MenubarModule,
        ButtonModule,
        InputTextModule,
        FontAwesomeModule,
        DropdownModule,
        ReactiveFormsModule,
        FormsModule,
        UserModule,
        EmoteModule,
        ImageModule,
        MasteryPropertyModule,
        WorldPropertyModule,
        TransitModule,
        MessagesModule,
        SeedModule,
        FishModule,
        FoodModule,
        FoodIngredientModule,
        CraftingModule,
        CraftingIngredientModule,
        GatheringModule,
        ProductModule,
        CraftingPropertyModule,
        GatheringPropertyModule,
        AlcoholModule,
        DrinkModule,
        AchievementModule,
        ContractModule,
        LocalizationModule,
        AlcoholPropertyModule,
        AlcoholIngredientModule,
        ToastModule
    ],
    providers:[
        MessageService,
        TableModule,
        ButtonModule,
        PaginatorModule,
        MenubarModule,
        InputTextareaModule,
        SliderModule,
        MessagesModule,
        MessageModule,
        MultiSelectModule
    ],
    declarations:[
        AppComponent
    ],
    bootstrap:[
        AppComponent
    ]
})
export class AppModule {
}
