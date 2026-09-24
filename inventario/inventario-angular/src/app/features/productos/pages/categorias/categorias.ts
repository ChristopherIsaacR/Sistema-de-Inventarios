import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Categoria } from '../../models/categoria.model';
import { CategoriasService } from '../../services/categorias';

@Component({
  selector: 'app-categorias',
  imports: [FormsModule],
  templateUrl: './categorias.html',
  styleUrl: './categorias.css'
})
export class Categorias implements OnInit {

  private readonly categoriasService =
    inject(CategoriasService);

  categorias: Categoria[] = [];

  nombre = '';
  descripcion = '';

  cargando = false;
  guardando = false;
  error = '';

  ngOnInit(): void {
    this.cargarCategorias();
  }

  cargarCategorias(): void {

    this.cargando = true;
    this.error = '';

    this.categoriasService.obtenerCategorias().subscribe({
      next: (categorias) => {
        this.categorias = categorias;
        this.cargando = false;
      },

      error: (error) => {
        console.error(
          'Error al cargar categorías:',
          error
        );

        this.error =
          'No se pudieron cargar las categorías.';

        this.cargando = false;
      }
    });
  }

  crearCategoria(): void {

    if (!this.nombre.trim()) {
      this.error =
        'El nombre de la categoría es obligatorio.';

      return;
    }

    this.guardando = true;
    this.error = '';

    this.categoriasService.crearCategoria({
      nombre: this.nombre.trim(),
      descripcion: this.descripcion.trim()
    }).subscribe({

      next: () => {

        this.nombre = '';
        this.descripcion = '';

        this.guardando = false;

        this.cargarCategorias();
      },

      error: (error) => {

        console.error(
          'Error al crear categoría:',
          error
        );

        this.error =
          error.status === 409
            ? 'La categoría ya existe.'
            : 'No se pudo crear la categoría.';

        this.guardando = false;
      }
    });
  }

  eliminarCategoria(categoria: Categoria): void {

    const confirmar = window.confirm(
      `¿Deseas eliminar "${categoria.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    this.categoriasService
      .eliminarCategoria(categoria.id)
      .subscribe({

        next: () => {
          this.cargarCategorias();
        },

        error: (error) => {
          console.error(
            'Error al eliminar categoría:',
            error
          );

          this.error =
            'No se pudo eliminar la categoría.';
        }
      });
  }
}