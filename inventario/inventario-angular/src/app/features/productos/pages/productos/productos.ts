import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { DecimalPipe } from '@angular/common';

import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Producto } from '../../models/producto.model';
import { Categoria } from '../../models/categoria.model';

import { ProductosService } from '../../services/productos';
import { CategoriasService } from '../../services/categorias';

@Component({
  selector: 'app-productos',
  imports: [
    ReactiveFormsModule,
    DecimalPipe
  ],
  templateUrl: './productos.html',
  styleUrl: './productos.css'
})


export class Productos implements OnInit {

  private readonly productosService =
    inject(ProductosService);

  private readonly categoriasService =
    inject(CategoriasService);

  private readonly fb =
    inject(NonNullableFormBuilder);

  private readonly cdr =
    inject(ChangeDetectorRef);

  productos: Producto[] = [];

  categorias: Categoria[] = [];

  cargando = false;

  guardando = false;

  error = '';

  mostrarFormulario = false;

  editandoId: number | null = null;

  formulario = this.fb.group({
    codigo: ['', Validators.required],

    nombre: ['', Validators.required],

    descripcion: [''],

    precio: [
      0,
      [
        Validators.required,
        Validators.min(0)
      ]
    ],

    stock: [
      0,
      [
        Validators.required,
        Validators.min(0)
      ]
    ],

    categoriaId: [
      null as number | null
    ]
  });

  ngOnInit(): void {

    this.cargarProductos();

    this.cargarCategorias();
  }

  cargarProductos(): void {

    this.cargando = true;

    this.error = '';

    this.productosService
      .obtenerProductos()
      .subscribe({

        next: (productos) => {

          this.productos = productos;

          this.cargando = false;

          this.cdr.markForCheck();
        },

        error: (error) => {

          console.error(
            'Error al cargar productos:',
            error
          );

          this.error =
            'No se pudieron cargar los productos.';

          this.cargando = false;

          this.cdr.markForCheck();
        }
      });
  }

  

  cargarCategorias(): void {
  this.categoriasService
    .obtenerCategorias()
    .subscribe({
      next: (categorias) => {
        console.log('CATEGORIAS RECIBIDAS:', categorias);

        this.categorias = categorias;

        this.cdr.markForCheck();
      },

      error: (error) => {
        console.error(
          'Error al cargar categorías:',
          error
        );
      }
    });
}

  nuevoProducto(): void {

    this.editandoId = null;

    this.formulario.reset({
      codigo: '',
      nombre: '',
      descripcion: '',
      precio: 0,
      stock: 0,
      categoriaId: null
    });

    this.error = '';

    this.mostrarFormulario = true;
  }

  editarProducto(producto: Producto): void {

    this.editandoId = producto.id;

    this.formulario.patchValue({

      codigo: producto.codigo,

      nombre: producto.nombre,

      descripcion: producto.descripcion,

      precio: producto.precio,

      stock: producto.stock,

      categoriaId: producto.categoriaId
    });

    this.error = '';

    this.mostrarFormulario = true;
  }

  cancelarFormulario(): void {

    this.mostrarFormulario = false;

    this.editandoId = null;

    this.error = '';

    this.formulario.reset({

      codigo: '',

      nombre: '',

      descripcion: '',

      precio: 0,

      stock: 0,

      categoriaId: null
    });
  }

  guardarProducto(): void {

    if (this.formulario.invalid) {

      this.formulario.markAllAsTouched();

      return;
    }

    this.guardando = true;

    this.error = '';

    const datos = this.formulario.getRawValue();

    if (this.editandoId === null) {

      this.productosService
        .crearProducto(datos)
        .subscribe({

          next: () => {

            this.guardando = false;

            this.cancelarFormulario();

            this.cargarProductos();
          },

          error: (error) => {

            console.error(
              'Error al crear producto:',
              error
            );

            this.error =
              'No se pudo crear el producto.';

            this.guardando = false;
          }
        });

    } else {

      this.productosService
        .actualizarProducto(
          this.editandoId,
          datos
        )
        .subscribe({

          next: () => {

            this.guardando = false;

            this.cancelarFormulario();

            this.cargarProductos();
          },

          error: (error) => {

            console.error(
              'Error al actualizar producto:',
              error
            );

            this.error =
              'No se pudo actualizar el producto.';

            this.guardando = false;
          }
        });
    }
  }

  eliminarProducto(producto: Producto): void {

    const confirmar = window.confirm(
      `¿Deseas eliminar "${producto.nombre}"?`
    );

    if (!confirmar) {
      return;
    }

    this.productosService
      .eliminarProducto(producto.id)
      .subscribe({

        next: () => {

          this.cargarProductos();
        },

        error: (error) => {

          console.error(
            'Error al eliminar producto:',
            error
          );

          this.error =
            'No se pudo eliminar el producto.';
        }
      });
  }
}