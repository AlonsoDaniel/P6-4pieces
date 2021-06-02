using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyBussinesModel;
using LyBussinesModel.DTO;

namespace LyDataAcces.DAO
{
    /// <summary>
    /// DaoUsuario class.
    /// Acceso a datos de Usuario.
    /// </summary>
    public class DaoUsuario : IDao
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
        /// RegistrarUsuario.
        /// Registra un usuario en la base de datos
        /// </summary>
        /// <param name="datos">DTOUsuario con toda la información del usuario</param>
        /// <returns>Si la operación ha sido correcta true, de lo contrario false y se puede consultar el error desde la propiedad Errores</returns>
        public bool RegistrarUsuario(LyBussinesModel.DTO.DTOUsuario datos)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Usuarios
                                where b.nombreUsuario == datos.NombreUsuario
                                select b;



                    //Impide registrar un usuario que ya existe.
                    Debug.Write(query.Count().ToString());


                    if (query.Count() > 0)
                    {
                        _Errores = new Exception("El usuario ya existe, escoja otro nombre");
                        return false;
                    }

                    ORM.Usuarios nuevoUsuario = new ORM.Usuarios
                    {
                        nombreUsuario = datos.NombreUsuario,
                        nombre = datos.Nombre,
                        apellidos = datos.Apellidos,
                        correo = datos.Correo,
                        telefono = datos.Telefono,
                        hasPassword = datos.HasContraseña
                    };


                    //Creación de las categorias
                    if (datos.Categorias != null)
                    {
                        if (datos.Categorias.Count > 0)
                        {
                            var queryCategorias = from b in db.Categorias select b;

                            ICollection<ORM.Categorias> nuevasCategorias = new List<ORM.Categorias>();
                            //elimna todos las categorias que no esten marcadas
                            foreach (LyBussinesModel.DTO.DTOCategoria categoria in datos.Categorias)
                            {
                                ORM.Categorias categoriaSeleccionada = queryCategorias.First(c => c.id == categoria.idCategoria);
                                if (categoriaSeleccionada != null)
                                {
                                    nuevasCategorias.Add(categoriaSeleccionada);
                                }

                            }
                            nuevoUsuario.Categorias = nuevasCategorias;
                        }
                    }

                    db.Usuarios.Add(nuevoUsuario);
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
        /// IniciarSesion.
        /// Verificar usuario y clave para loguear usuario.
        /// </summary>
        /// <param name="usuario">Usuario.</param>
        /// <param name="password"> Clave en texto claro</param>
        /// <returns>Devuelve la ide del usuario encontrado, si no encuentra ninguno devuelve 11</returns>
        public int IniciarSesion(String usuario, String password)
        {
            Usuario userLogueado = new Usuario();
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    String hash = Usuario.CreateHash(usuario, password);
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Usuarios
                                where b.hasPassword == hash
                                select b.id;

                    //Se comprueba si existe algun resultado
                    if (!(query.Count() > 0))
                    {
                        return -1;
                    }
                    else
                    {
                        return query.First();
                    }
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return -1;
            }

        }

        /// <summary>
        /// ModificarUsuario.
        /// Modifica un usuario
        /// </summary>
        /// <param name="editedUser"></param>
        /// <returns>Si no se especifica un id en el dto retorna un error</returns>
        public bool ModificarUsuario(LyBussinesModel.DTO.DTOUsuario editedUser)
        {
            try
            {
                if (!(editedUser.id > 0))
                {
                    throw new Exception("No se ha proporcinado un id valido para modificar");
                }
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Usuarios
                                where b.id == editedUser.id
                                select b;

                    if (!(query.Count() > 0))
                    {
                        _Errores = new Exception("El usuario no existe");
                        return false;
                    }



                    ORM.Usuarios usuarioBD = query.First();

                    if (editedUser.NombreUsuario != null && editedUser.NombreUsuario.Length > 0 && usuarioBD.nombreUsuario != editedUser.NombreUsuario)
                        usuarioBD.nombreUsuario = editedUser.NombreUsuario;

                    if (editedUser.Nombre != null && editedUser.Nombre.Length > 0 && usuarioBD.nombre != editedUser.Nombre)
                        usuarioBD.nombre = editedUser.Nombre;

                    if (editedUser.Apellidos != null && editedUser.Apellidos.Length > 0 && usuarioBD.apellidos != editedUser.Apellidos)
                        usuarioBD.apellidos = editedUser.Apellidos;

                    if (editedUser.Correo != null && editedUser.Correo.Length > 0 && usuarioBD.correo != editedUser.Correo)
                        usuarioBD.correo = editedUser.Correo;

                    if (editedUser.Telefono != null && editedUser.Telefono.Length > 0 && usuarioBD.telefono != editedUser.Telefono)
                        usuarioBD.telefono = editedUser.Telefono;

                    if (editedUser.HasContraseña != null && editedUser.HasContraseña.Length > 0 && usuarioBD.hasPassword != editedUser.HasContraseña)
                        usuarioBD.hasPassword = editedUser.HasContraseña;

                    //Edición de las categorias
                    if (editedUser.Categorias != null)
                    {
                        
                        var queryCategorias = from b in db.Categorias select b;
                        usuarioBD.Categorias.Clear();
                        ICollection<ORM.Categorias> nuevasCategorias = new List<ORM.Categorias>();
                        //elimna todos las categorias que no esten marcadas
                        foreach (LyBussinesModel.DTO.DTOCategoria categoria in editedUser.Categorias)
                        {
                            ORM.Categorias categoriaSeleccionada = queryCategorias.First(c => c.id == categoria.idCategoria);
                            if (categoriaSeleccionada != null)
                            {
                                nuevasCategorias.Add(categoriaSeleccionada);
                            }

                        }
                        usuarioBD.Categorias = nuevasCategorias;
                        
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

        /// <summary>
        /// GetAllUsuarios.
        /// Consultar todos los usuarios.
        /// </summary>
        /// <returns>Devuelve una lista con todos los usuarios en el sistema</returns>
        public List<Usuario> GetAllUsuarios()
        {
            try
            {
                List<Usuario> allUsers = new List<Usuario>();

                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Usuarios select b;



                    foreach (ORM.Usuarios dbuser in query.ToList<ORM.Usuarios>())
                    {
                        Usuario user = new Usuario(dbuser.id,
                                              dbuser.nombreUsuario,
                                              dbuser.nombre,
                                              dbuser.apellidos,
                                              dbuser.tiempoAcumulado,
                                              dbuser.telefono,
                                              dbuser.correo,
                                              Convert.ToInt32(dbuser.horasAcumuladas))
                        {
                            //Recogida de las categorias
                            Categorias = new List<Categoria>()
                        };
                        foreach (ORM.Categorias categoria in dbuser.Categorias)
                        {
                            user.Categorias.Add(new Categoria(categoria.id, categoria.nombre));
                        }

                        allUsers.Add(user);
                    }


                    return allUsers;
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return null;
            }

        }

        /// <summary>
        /// GetPerfilUsuario.
        /// Devuelve los datos de un usuario 
        /// </summary>
        /// <param name="idUsuario"> id del usuario</param>
        /// <returns>Usuario</returns>
        public Usuario GetPerfilUsuario(int idUsuario)
        {

            if (idUsuario <= 0)
            {
                _Errores = new Exception("Error de id introducida");
                return null;
            }

            Usuario perfil = new Usuario();
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    ORM.Usuarios userEncontrado;
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Usuarios
                                where b.id == idUsuario
                                select b;


                    if (!(query.Count() > 0))
                    {
                        _Errores = new Exception("No se ha encontrado al usuario");
                        return null;
                    }
                    else
                    {
                        userEncontrado = (ORM.Usuarios)query.First();

                        if (userEncontrado != null)
                        {
                            if(userEncontrado.tiempoAcumulado == null)
                            {
                                userEncontrado.tiempoAcumulado = 0;
                            }
                            perfil = new Usuario(userEncontrado.id,
                                                userEncontrado.nombreUsuario,
                                                userEncontrado.nombre,
                                                userEncontrado.apellidos,
                                                userEncontrado.tiempoAcumulado,
                                                userEncontrado.telefono,
                                                userEncontrado.correo,
                                                Convert.ToInt32(userEncontrado.horasAcumuladas))
                            {

                                //Recogida de las categorias
                                Categorias = new List<Categoria>()
                            };
                            foreach (ORM.Categorias categoria in userEncontrado.Categorias)
                            {
                                perfil.Categorias.Add(new Categoria(categoria.id, categoria.nombre));
                            }
                            return perfil;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return null;
            }

        }

        /// <summary>
        /// GetDetellaUsuario.
        /// Devuelve los detalles de un usuario 
        /// </summary>
        /// <param name="idUsuario"> id del usuario</param>
        /// <returns>Cadena</returns>
        public DTOUsuario GetDetellaUsuario(int idUsuario)
        {

            if (idUsuario <= 0)
            {
                _Errores = new Exception("Error de id introducida");
                return null;
            }

            DTOUsuario perfil = new DTOUsuario();
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    ORM.Usuarios userEncontrado;
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda del nombre en BD
                    var query = from b in db.Usuarios
                                where b.id == idUsuario
                                select b;


                    if (!(query.Count() > 0))
                    {
                        _Errores = new Exception("No se ha encontrado al usuario");
                        return null;
                    }
                    else
                    {
                        userEncontrado = (ORM.Usuarios)query.First();

                        if (userEncontrado != null)
                        {


                            perfil = new DTOUsuario(userEncontrado.nombreUsuario,
                                                userEncontrado.nombre,
                                                userEncontrado.apellidos,
                                                userEncontrado.telefono,
                                                userEncontrado.correo,
                                                Convert.ToInt32(userEncontrado.horasAcumuladas))
                            {
                                id = userEncontrado.id,
                                Categorias = new List<DTOCategoria>()   //Recogida de las categorias
                            };
                            foreach (ORM.Categorias categoria in userEncontrado.Categorias)
                            {
                                perfil.Categorias.Add(new DTOCategoria(categoria.id, categoria.nombre));
                            }
                            return perfil;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return null;
            }
        }

    }
}