import {
  Component,
  Injector,
  OnInit,
  EventEmitter,
  Output,
  ChangeDetectorRef
} from '@angular/core';
import { EmailTemplateDto, TemplateEmailServiceServiceProxy, EmailTemplateDtoPagedResultDto } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService  } from 'ngx-bootstrap/modal';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs';
import { CreateTemplatesComponent } from './createTemplate/createTemplateComponent.';

class EmailTemplateRequestDto extends PagedRequestDto {
  keyword: string;
  status : string; 
  sorting: string;
}

@Component({
  selector: 'app-email-templates',
  templateUrl: './email-templates.component.html',
})
export class EmailTemplatesComponent extends PagedListingComponentBase<EmailTemplateDto> {
  keyword = "";
  status = "";
  sorting = "";
  emailTemplate: EmailTemplateDto[] = [];
  selectedEmail: EmailTemplateDto = new EmailTemplateDto;
  visible: boolean = false;
  tokenList: string[] = [];


  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _emailService: TemplateEmailServiceServiceProxy,
    public _modalService: BsModalService,
    private cdr: ChangeDetectorRef
  ) {
    super(injector, cdr);
  }
  
ngOnInit(): void {
  this.refresh();
}

protected list(request: EmailTemplateRequestDto, pageNumber: number, finishedCallback: Function): void {
  request.keyword = this.keyword;

  this._emailService.getTemplate( request.keyword,request.status, request.sorting,  request.skipCount,
    request.maxResultCount).pipe(
      finalize(() => {
        finishedCallback();
      })
    ).subscribe((result: EmailTemplateDtoPagedResultDto) => {
        this.emailTemplate = result.items;
        
        this.showPaging(result, pageNumber);  
        console.log(this.tokenList );
             
      });    
}


protected delete(entity: EmailTemplateDto): void {
  this._emailService.deleteTemplate(entity.id).subscribe(
    response => {             
     console.log("deleted");
     
    });
}

showDialog(email: EmailTemplateDto) {
  this.selectedEmail = email;
  this.tokenList = this.parseTokens(this.selectedEmail.token);
  this.visible = true;   
}

saveChanges(editemail:EmailTemplateDto){
  console.log(editemail);
  this._emailService.createOrEditTemplate(editemail).subscribe(
    response => {                   
    });
    this.closeDialog();
}
closeDialog() {
  this.visible = false; 
}

parseTokens(tokenString: string | null): string[] {
  return tokenString ? tokenString.split(',').map(token => token.trim()) : [];
}

openCreateTemplateModal() {
  const initialState = { createEmail: new EmailTemplateDto() };
  this._modalService.show(CreateTemplatesComponent,);
}


}
