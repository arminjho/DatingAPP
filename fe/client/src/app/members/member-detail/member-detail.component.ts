import { Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MembersService } from '../../_service/members.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../_models/member';
import { CommonModule, DatePipe } from '@angular/common';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { MemberMessagesComponent } from "../member-messages/member-messages.component";
import { Message } from '../../_models/message';
import { MessageService } from '../../_service/message.service';
import { PresenceService } from '../../_service/presence.service';
import { AccountService } from '../../_service/account.service';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';


@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [CommonModule, TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessagesComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit , OnDestroy{
  @ViewChild('memberTabs',{static:true})  memberTabs?: TabsetComponent;
  private messageService= inject(MessageService);
  presenceService=inject(PresenceService);
  private route=inject(ActivatedRoute);
  private router=inject(Router);
  private accountService=inject(AccountService);
  member:Member={}as Member;
  images:GalleryItem[]=[];
  activetab?:TabDirective;

  
  
  ngOnInit():void{
    this.route.data.subscribe({
      next:data =>{
        this.member=data['member'];
        this.member&& this.member.photos.map(p=>{
          this.images.push(new ImageItem({src:p.url,thumb:p.url}))})
        }
      })
      this.route.paramMap.subscribe({
        next:_=>this.onRouteParamsChange()
      })
      this.route.queryParams.subscribe({
        next:params=>{ 
          params['tab']&& this.selectTab(params['tab'])
        }
      })
    }
    
    ngOnDestroy(): void {
      this.messageService.stopHubConnection();
    }



  selectTab(heading:string){
    if(this.memberTabs){
      const messageTab= this.memberTabs.tabs.find(x=>x.heading===heading);
      if(messageTab) messageTab.active=true;
    }
  }

onRouteParamsChange(){
  const user=this.accountService.currentUser();
  if(!user)return;
  if(this.messageService.hubConnection?.state=== 
    HubConnectionState.Connected && this.activetab?.heading === 'Messages'){
      this.messageService.hubConnection.stop().then(()=> {
        this.messageService.createHubConnection(user, this.member.username);
      })
    }
}

  onTabActivated(data:TabDirective){
    this.activetab=data;
    this.router.navigate([],{
      relativeTo:this.route,
      queryParams:{tab:this.activetab.heading},
      queryParamsHandling: 'merge'
    })
    if(this.activetab.heading === 'Messages' &&  this.member){
     const user= this.accountService.currentUser();
     if(!user) return;
     this.messageService.createHubConnection(user,this.member.username);
    }else {
      this.messageService.stopHubConnection();
    }

  }
 
}
