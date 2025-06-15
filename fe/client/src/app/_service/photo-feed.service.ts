import { Injectable } from '@angular/core';
import { interval, Observable } from 'rxjs';
import { switchMap, distinctUntilChanged, shareReplay } from 'rxjs/operators';
import { MembersService } from './members.service';
import { Photo } from '../_models/photo';

@Injectable({
  providedIn: 'root',
})
export class PhotoFeedService {
  constructor(private membersService: MembersService) {}

  getUserPhotoStream(username: string): Observable<Photo[]> {
    return interval(10000).pipe(
      switchMap(() => this.membersService.getMemberPhotos(username)),
      distinctUntilChanged(
        (prev, curr) => JSON.stringify(prev) === JSON.stringify(curr)
      ),
      shareReplay(1)
    );
  }
}
