import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Producto,
  ProductoRequest
} from '../models/producto.model';

@Injectable({
  providedIn: 'root'
})
export class ProductosService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5062/api/productos';

  obtenerProductos(): Observable<Producto[]> {
    return this.http.get<Producto[]>(this.apiUrl);
  }

  obtenerProducto(id: number): Observable<Producto> {
    return this.http.get<Producto>(
      `${this.apiUrl}/${id}`
    );
  }

  crearProducto(
    producto: ProductoRequest
  ): Observable<Producto> {
    return this.http.post<Producto>(
      this.apiUrl,
      producto
    );
  }

  actualizarProducto(
    id: number,
    producto: ProductoRequest
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      producto
    );
  }

  eliminarProducto(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}