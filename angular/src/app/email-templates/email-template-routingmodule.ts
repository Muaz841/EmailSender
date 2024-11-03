import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EmailTemplatesComponent } from './email-templates.component';
import { TemplateEmailServiceServiceProxy } from '@shared/service-proxies/service-proxies';




const routes: Routes = [
    {
        path: '',
        component: EmailTemplatesComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    providers: [TemplateEmailServiceServiceProxy]
})
export class emailTemplateRoutingModule {}
