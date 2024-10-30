import { NgModule } from '@angular/core';
import {EmailTemplatesComponent}from './email-templates.component'
import {emailTemplateRoutingModule} from './email-template-routingmodule'
import { SharedModule } from '@shared/shared.module';


@NgModule({
    declarations: [EmailTemplatesComponent],
    imports: [emailTemplateRoutingModule,SharedModule],
})
export class emailtemplatemodule {}