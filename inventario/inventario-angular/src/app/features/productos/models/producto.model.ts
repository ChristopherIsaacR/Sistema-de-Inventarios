export interface Producto {
  id: number;
  codigo: string;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
  categoriaId: number | null;
  categoriaNombre: string | null;
}

export interface ProductoRequest {
  codigo: string;
  nombre: string;
  descripcion: string;
  precio: number;
  stock: number;
  categoriaId: number | null;
}