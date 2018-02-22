import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { OffersComponent } from './components/offers/offers.component';
import { AuthGuard } from './guards/auth.guard';

const appRoutes: Routes = [
  { path: 'lc/login', component: LoginComponent },
  {
    path: 'lc/offers',
    component: OffersComponent,
    canActivate: [AuthGuard]
  },
  // otherwise redirect to home
  { path: '**', redirectTo: 'lc/offers' }
];

export const routing = RouterModule.forRoot(appRoutes);
