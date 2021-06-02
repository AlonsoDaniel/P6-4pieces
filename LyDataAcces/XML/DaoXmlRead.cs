using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LyDataAcces.XML
{
    /// <summary>
    /// DaoXmlRead class.
    /// Acceso a datos XML.
    /// </summary>
    public class DaoXmlRead : DAO.IDao
    {
        private Exception _Errores;
        private XmlDocument _DocuemntoXml;

        Exception DAO.IDao.Errores
        {
            get
            {
                Exception ex = _Errores;
                _Errores = null;
                return ex;
            }
            set => _Errores = value;
        }

        public struct DatosConexion
        {
            public String Host;
            public String User;
            public String Password;
        }


        /// <summary>
        /// Load.
        /// Carga datos XML.
        /// </summary>
        private bool Load(String data) {
            _DocuemntoXml = new XmlDocument();

            try
            {
                _DocuemntoXml.LoadXml(data);
                return true;
            }catch(Exception ex)
            {
                _Errores = ex;
                return false;
            }
        }


        /// <summary>
        /// ObtenerDatosServidorSql.
        /// Administración de datos registro a bd.
        /// </summary>
        /// <param name="pathOrXml"></param>
        /// <returns>Cadena</returns>
        public DatosConexion? ObtenerDatosServidorSql(String pathOrXml)
        {
            DatosConexion datos = new DatosConexion();
            try
            {
                if (!Load(pathOrXml))//Si falla al cargar xml retorna null.
                    return null;

                XmlNodeList elemList = _DocuemntoXml.GetElementsByTagName("host");
                if(elemList.Count > 0)
                {
                    datos.Host = elemList.Item(0).InnerText;
                
                }

                elemList = _DocuemntoXml.GetElementsByTagName("user");
                if (elemList.Count > 0)
                {
                    datos.User = elemList.Item(0).InnerText;
                }

                elemList = _DocuemntoXml.GetElementsByTagName("password");
                if (elemList.Count > 0)
                {
                    datos.Password = elemList.Item(0).InnerText;
                }

            }
            catch(Exception ex)
            {
                _Errores = ex;
                return null;
            }

            return datos;
        }
    }
}
