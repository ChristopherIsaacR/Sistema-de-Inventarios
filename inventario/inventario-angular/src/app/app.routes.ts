import { Routes } from '@angular/router';

import { Login } from './features/auth/login/login';

import { Dashboard } from './features/dashboard/dashboard';

import { Productos } from './features/productos/pages/productos/productos';

import { Categorias } from './features/productos/pages/categorias/categorias';

import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [

  {
    path: 'login',
    component: Login,
    title: 'Iniciar sesión'
  },

  {
    path: 'dashboard',
    component: Dashboard,
    canActivate: [authGuard],
    title: 'Dashboard'
  },

  {
    path: 'productos',
    component: Productos,
    canActivate: [authGuard],
    title: 'Productos'
  },

  {
    path: 'categorias',
    component: Categorias,
    canActivate: [authGuard],
    title: 'Categorías'
  },

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: '**',
    redirectTo: 'login'
  }
];