using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyBussinesModel
{
    /// <summary>
    /// Servicio class.
    /// Recoge la descripción del trabajo a realizar.
    /// </summary>
    /// <remarks>
    /// <para>Esta clase identifica al usuario publicador</para>
    /// <para>Esta clase clasifica, describe y fecha el servicio</para>
    /// </remarks>
    public class Servicio
    {
        /// <value>Valores de identificación</value>
        private int _id;
        private Usuario _Creador;
        private List<Candidatura> _Candidaturas;

        /// <value>Valores de filtraje</value>
        private List<Categoria> _Categorias;

        /// <value>Valores de descripción</value>
        private String _Titulo;
        private String _Descripcion;

        /// <value>Valores de fecha/tiempo</value>
        private DateTime? _FechaDeInicio;
        private bool _Finalizado;

        public Servicio() { }

        public Servicio(string titulo, 
            string descripcion, 
            DateTime? fechaDeInicio,  
            int id, 
            Usuario creador, 
            bool finalizado)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            FechaDeInicio = fechaDeInicio;
            Id = id;
            Creador = creador;
            Finalizado = finalizado;
        }

        public string Titulo { get => _Titulo; set => _Titulo = value; }
        public string Descripcion { get => _Descripcion; set => _Descripcion = value; }
        public DateTime? FechaDeInicio { get => _FechaDeInicio; set => _FechaDeInicio = value; }
        public int Id { get => _id; set => _id = value; }
        public Usuario Creador { get => _Creador; set => _Creador = value; }
        public List<Candidatura> Candidaturas { get => _Candidaturas; set => _Candidaturas = value; }
        public List<Categoria> Categorias { get => _Categorias; set => _Categorias = value; }
        public bool Finalizado { get => _Finalizado; set => _Finalizado = value; }
    }
}
