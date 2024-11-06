import { NgModule } from '@angular/core';
import { SharedModule } from '../../shared/shared.module'
import { FieldsetModule } from 'primeng/fieldset';
import { EditorModule } from 'primeng/editor';
import { TabViewModule } from 'primeng/tabview';
import { AccordionModule } from 'primeng/accordion';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FloatLabelModule } from 'primeng/floatlabel';
import { ButtonModule } from 'primeng/button';
import { SmtpSettingsComponent } from './smtpSettings.component';
import { SmtpSettingsServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import {SmtpSettingsRoutingModule} from './smtpSettings.routingmodule';
import { DialogModule } from 'primeng/dialog';
import { ChipsModule } from 'primeng/chips';


@NgModule({
    declarations: [SmtpSettingsComponent],
    imports: [ChipsModule, DialogModule,FormsModule,DropdownModule,TableModule,AccordionModule,CommonModule,ButtonModule,SmtpSettingsRoutingModule,
        EditorModule,SharedModule,FieldsetModule,TabViewModule,FloatLabelModule],
        providers: [SmtpSettingsServiceServiceProxy]
})
export class SmtpSettingmodule {}