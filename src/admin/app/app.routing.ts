import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './components/login/login.component';
import { StatisticComponent } from './components/statistic/statistic.component';
import { AuthGuard } from './guards/auth.guard';
import { UsersComponent } from './components/users/users.component';
import { SeoComponent } from './components/seo/seo.component';
import { ModerationComponent } from './components/moderation/moderation.component';
import { ProducersComponent } from './components/producers/producers.component';
import { CatalogsComponent } from './components/catalogs/catalogs.component';
import { SuppliersComponent } from './components/suppliers/suppliers.component';
import { PagesComponent } from './components/pages/pages.component';

const appRoutes: Routes = [
  { path: 'admin/login', component: LoginComponent },
  {
    path: 'admin/home',
    component: StatisticComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin/pages',
    component: PagesComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin/suppliers',
    component: SuppliersComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin/catalogs',
    component: CatalogsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin/producers',
    component: ProducersComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin/moderation',
    component: ModerationComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin/seo',
    component: SeoComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'admin/users',
    component: UsersComponent,
    canActivate: [AuthGuard]
  },

  // otherwise redirect to home
  { path: '**', redirectTo: 'admin/home' }
];

export const routing = RouterModule.forRoot(appRoutes);
