using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TiendaSimplificada
{
    class Producto
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
    }

    class Inventario
    {
        private List<Producto> productos;
        private string rutaArchivo;

        public Inventario(string archivo)
        {
            productos = new List<Producto>();
            rutaArchivo = archivo;
            CargarProductosDesdeArchivo();
        }

        public void CargarProductosDesdeArchivo()
        {
            if (File.Exists(rutaArchivo))
            {
                try
                {
                    string json = File.ReadAllText(rutaArchivo);
                    productos = JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
                    Console.WriteLine("Productos  desde el archivo.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al cargar los productos: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Archivo no encontrado.");
            }
        }

        public void GuardarProductosEnArchivo()
        {
            try
            {
                string json = JsonConvert.SerializeObject(productos, Formatting.Indented);
                File.WriteAllText(rutaArchivo, json);
                Console.WriteLine("Inventario guardado en el archivo.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al guardar los productos: " + ex.Message);
            }
        }

        public void VerInventario()
        {
            Console.WriteLine("\nInventario de productos:");
            foreach (var producto in productos)
            {
                Console.WriteLine($"Código: {producto.Codigo}, Nombre: {producto.Nombre}, Stock: {producto.Stock}, Precio: {producto.Precio:C}");
            }
        }

        public Producto BuscarProducto(int codigo)
        {
            return productos.Find(p => p.Codigo == codigo);
        }

        public void AgregarProducto(Producto nuevoProducto)
        {
            if (productos.Exists(p => p.Codigo == nuevoProducto.Codigo))
            {
                Console.WriteLine("El producto con este código ya existe.");
            }
            else
            {
                productos.Add(nuevoProducto);
                Console.WriteLine("Producto agregado correctamente.");
            }
        }

        public void ComprarProducto(int codigo, int cantidad)
        {
            Producto producto = BuscarProducto(codigo);

            if (producto != null && producto.Stock >= cantidad)
            {
                producto.Stock -= cantidad;
                decimal total = cantidad * producto.Precio;
                Console.WriteLine($"Compra realizada. Total a pagar: {total:C}");
            }
            else
            {
                Console.WriteLine("Producto no disponible o stock insuficiente.");
            }
        }
    }

    class Program
    {
        static Inventario inventario = new Inventario("productos.json");

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n1. Buscar producto");
                Console.WriteLine("2. Comprar producto");
                Console.WriteLine("3. Ver inventario");
                Console.WriteLine("4. Agregar producto");
                Console.WriteLine("5. Salir");
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        BuscarProducto();
                        break;
                    case "2":
                        ComprarProducto();
                        break;
                    case "3":
                        VerInventario();
                        break;
                    case "4":
                        AgregarProducto();
                        break;
                    case "5":
                        inventario.GuardarProductosEnArchivo();
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        static void BuscarProducto()
        {
            Console.Write("Ingrese el código del producto: ");
            int codigo = int.Parse(Console.ReadLine());
            Producto producto = inventario.BuscarProducto(codigo);

            if (producto != null)
                Console.WriteLine($"Producto: {producto.Nombre}, Stock: {producto.Stock}, Precio: {producto.Precio:C}");
            else
                Console.WriteLine("Producto no encontrado.");
        }

        static void ComprarProducto()
        {
            Console.Write("Ingrese el código del producto: ");
            int codigo = int.Parse(Console.ReadLine());
            Console.Write("¿Cuántos desea comprar?: ");
            int cantidad = int.Parse(Console.ReadLine());

            inventario.ComprarProducto(codigo, cantidad);
        }

        static void VerInventario()
        {
            inventario.VerInventario();
        }

        static void AgregarProducto()
        {
            Console.Write("Ingrese el código del producto: ");
            int codigo = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el nombre del producto: ");
            string nombre = Console.ReadLine();
            Console.Write("Ingrese el stock del producto: ");
            int stock = int.Parse(Console.ReadLine());
            Console.Write("Ingrese el precio del producto: ");
            decimal precio = decimal.Parse(Console.ReadLine());

            Producto nuevoProducto = new Producto { Codigo = codigo, Nombre = nombre, Stock = stock, Precio = precio };
            inventario.AgregarProducto(nuevoProducto);
        }
    }
}
 