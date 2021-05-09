﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyBussinesModel
{
    public class Servicio
    {
        private int _id;
        private Usuario _Creador;
        private List<Candidatura> _Candidaturas;
        private List<Categoria> _Categorias;
        private String _Titulo;
        private String _Descripcion;
        private DateTime? _FechaDeInicio;

        public Servicio(string titulo, 
            string descripcion, 
            DateTime? fechaDeInicio,  
            int id, 
            Usuario creador)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            FechaDeInicio = fechaDeInicio;
            Id = id;
        }

        public string Titulo { get => _Titulo; set => _Titulo = value; }
        public string Descripcion { get => _Descripcion; set => _Descripcion = value; }
        public DateTime? FechaDeInicio { get => _FechaDeInicio; set => _FechaDeInicio = value; }
        public int Id { get => _id; set => _id = value; }
        internal Usuario Creador { get => _Creador; set => _Creador = value; }
        internal List<Candidatura> Candidaturas { get => _Candidaturas; set => _Candidaturas = value; }
        internal List<Categoria> Categorias { get => _Categorias; set => _Categorias = value; }
    }
}
