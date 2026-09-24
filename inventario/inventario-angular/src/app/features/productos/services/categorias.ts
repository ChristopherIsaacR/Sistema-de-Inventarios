import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Categoria } from '../models/categoria.model';

@Injectable({
  providedIn: 'root'
})
export class CategoriasService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5062/api/categorias';

  obtenerCategorias(): Observable<Categoria[]> {
    return this.http.get<Categoria[]>(this.apiUrl);
  }

  crearCategoria(
    categoria: Omit<Categoria, 'id'>
  ): Observable<Categoria> {
    return this.http.post<Categoria>(
      this.apiUrl,
      categoria
    );
  }

  eliminarCategoria(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}