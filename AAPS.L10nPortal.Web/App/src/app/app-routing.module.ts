import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { AssetsComponent } from './components/assets/assets.component';

const routes: Routes = [
  {
    path: '',
    component: DashboardComponent
  }, {
    path: 'app-assets/:applicationLocaleId',
    component: AssetsComponent
  },
  {
    path: 'asset-links/:applicationLocaleId',
    component: AssetsComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {useHash: true})],
  exports: [RouterModule]
})

export class AppRoutingModule { }
