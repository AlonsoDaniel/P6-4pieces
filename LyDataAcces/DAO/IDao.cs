using LyBussinesModel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyDataAcces.DAO
{
    /// <summary>
    /// IDao interface.
    /// Acceso a datos.
    /// </summary>
    public interface IDao
    {
        Exception Errores
        {
            get;
            set;
        }

        //bool RegistrarServicio(DTOServicios datos);
    }
}
