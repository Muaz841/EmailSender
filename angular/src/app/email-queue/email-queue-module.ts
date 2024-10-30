import { NgModule } from '@angular/core';
import {queuedEmailComponent} from './email-queue.component';
import { queuedEmailRoutingModule } from './email-queue-routingModule';
import { SharedModule } from '../../shared/shared.module'




@NgModule({
    declarations: [queuedEmailComponent],
    imports: [queuedEmailRoutingModule,SharedModule],
})
export class queuedEmailModule {}