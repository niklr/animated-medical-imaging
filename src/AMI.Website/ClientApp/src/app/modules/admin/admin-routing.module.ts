import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AdminLogsComponent } from './logs/logs.component';

const routes: Routes = [
    {
        path: '',
        component: AdminComponent
    },
    {
        path: 'logs',
        component: AdminLogsComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminRoutingModule {}
