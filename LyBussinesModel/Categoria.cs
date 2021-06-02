using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyBussinesModel
{
    /// <summary>
    /// Categoria class.
    /// Recoge el tipo de servicio para su clasificación.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza de filtraje.
    /// </remarks>
    public class Categoria
    {
        /// <value>Valores de identificación</value>
        private int _Id;
        private String _Nombre;

        public Categoria(int id, string nombre)
        {
            _Id = id;
            _Nombre = nombre;
        }

        public string Nombre { get => _Nombre; set => _Nombre = value; }
        public int Id { get => _Id; set => _Id = value; }
    }
}
