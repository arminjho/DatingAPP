import {
  Directive,
  inject,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AccountService } from '../_service/account.service';
import { AuthStoreService } from '../_service/auth-store.service';
import { take } from 'rxjs';

@Directive({
  selector: '[appHasRole]',
  standalone: true,
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[] = [];
  private authStore = inject(AuthStoreService);
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef);

  ngOnInit(): void {
    this.authStore.roles$

      .pipe(take(1))

      .subscribe((userRoles) => {
        if (!userRoles || userRoles.length === 0) {
          this.viewContainerRef.clear();

          return;
        }

        const hasMatch = userRoles.some((role) =>
          this.appHasRole.includes(role)
        );

        if (hasMatch) {
          this.viewContainerRef.createEmbeddedView(this.templateRef);
        } else {
          this.viewContainerRef.clear();
        }
      });
  }
}
