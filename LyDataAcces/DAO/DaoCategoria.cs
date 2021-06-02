using LyBussinesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyDataAcces.DAO
{
    /// <summary>
    /// DaoCategoria class.
    /// Acceso a datos de Categoria.
    /// </summary>
    public class DaoCategoria: IDao
    {
        private Exception _Errores;
        Exception IDao.Errores
        {
            get
            {
                Exception ex = _Errores;
                _Errores = null;
                return ex;
            }
            set => _Errores = value;
        }



        /// <summary>
        /// GetAllCategorias.
        /// Lista de todas las categorías disponibles
        /// </summary>
        /// <returns>Listado</returns>
        public List<Categoria> GetAllCategorias() 
        { 
            try
            {
                List<Categoria> allCategorias = new List<Categoria>();

                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Categorias select b;

                    //Recogida de las categorias
                    allCategorias = new List<Categoria>();
                    foreach (ORM.Categorias categoria in query)
                    {
                        allCategorias.Add(new Categoria(categoria.id, categoria.nombre));
                    }

                    return allCategorias;
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return null;
            }

        }

        /// <summary>
        /// LoadCategoriaUsuario.
        /// Carga en el objeto de usuario las categorias que tiene activada
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Boleano indicando si la operación se completo con exito.</returns>
        public bool LoadCategoriaUsuario(Usuario usuario)
        {

            if (usuario.Id <= 0)
            {
                _Errores = new Exception("Error de id introducida");
                return false;
            }

            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    ORM.Usuarios userEncontrado;
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Usuarios
                                where b.id == usuario.Id
                                select b;


                    if (!(query.Count() > 0))
                    {
                        _Errores = new Exception("No se ha encontrado al usuario");
                        return false;
                    }else{
                        userEncontrado = (ORM.Usuarios)query.First();

                        if (userEncontrado != null)
                        {

                            //Recogida de las categorias
                            usuario.Categorias = new List<Categoria>();
                            foreach (ORM.Categorias categoria in userEncontrado.Categorias)
                            {
                                usuario.Categorias.Add(new Categoria(categoria.id, categoria.nombre));
                            }
                            return true;
                        }else{
                            return false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return false;
            }

        }

        /// <summary>
        /// LoadCategoriaServicio.
        /// Carga en el objeto de sercio las categorías que tiene activada
        /// </summary>
        /// <param name="servicio"></param>
        /// <returns>Boleano indicando si la operación se ha realizado correctamente</returns>
        public bool LoadCategoriaServicio(Servicio servicio)
        {

            if (servicio.Id <= 0)
            {
                _Errores = new Exception("Error de id introducida");
                return false;
            }

            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    ORM.Servicios servicioEncontrado;
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Servicios
                                where b.id == servicio.Id
                                select b;


                    if (!(query.Count() > 0))
                    {
                        _Errores = new Exception("No se ha encontrado al usuario");
                        return false;
                    }
                    else
                    {
                        servicioEncontrado = (ORM.Servicios)query.First();

                        if (servicioEncontrado != null)
                        {

                            //Recogida de las categorias
                            //hay que poner publica la variable
                            servicio.Categorias= new List<Categoria>();
                            foreach (ORM.Categorias categoria in servicioEncontrado.Categorias)
                            {
                                servicio.Categorias.Add(new Categoria(categoria.id, categoria.nombre));
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return false;
            }

        }

        /// <summary>
        /// CrearCategoria.
        /// Crea una nueva categoría
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>True se ha registrado, false indica un error con la bd, o  que la categoría ya existe.</returns>
        public bool CrearCategoria(String nombre)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Categorias
                                where b.nombre == nombre
                                select b;


                    if (query.Count() > 0)
                    {
                        _Errores = new Exception("Esta categoría ya existe");
                        return false;
                    }

                    ORM.Categorias nuevaCategoria = new ORM.Categorias();
                    nuevaCategoria.nombre = nombre;
                    db.Categorias.Add(nuevaCategoria);
                    db.SaveChanges();
                    return true;
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return false;
            }
        }

        /// <summary>
        /// EliminarCategoria.
        /// Elimina una categoría
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns>booleano indicando exito en la operación</returns>
        public bool EliminarCategoria(Categoria categoria)
        {
            try
            {
                ORM.Categorias categoriaBD;

                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Categorias
                                where b.id == categoria.Id
                                select b;

                    //Recogida de las categorias
                    categoriaBD = query.First();

                    if(categoriaBD != null)
                    {
                        db.Categorias.Remove(categoriaBD);
                    }
                    db.SaveChanges();

                    return true;
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return false;
            }

        }
    }
}
