﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyBussinesModel.DTO
{
    public class DTOCategoria
    {
        public int idCategoria;
        public String nombre;

        public DTOCategoria(int id)
        {
            idCategoria = id;
        }

        public DTOCategoria(string nombre)
        {
            this.nombre = nombre;
        }

        public DTOCategoria(int idCategoria, string nombre)
        {
            this.idCategoria = idCategoria;
            this.nombre = nombre;
        }
    }
}
