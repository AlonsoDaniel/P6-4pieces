using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyDataAcces;
using LyBussinesModel;
using System.Data.Entity.Validation;

namespace TestDataAccesConsole
{

    /// <summary>
    /// Program class.
    /// </summary>
    class Program
    {


        /// <summary>
        /// Main.
        /// </summary>
        static void Main(string[] args)
        {
            bool resultados = false;
            LyDataAcces.DAO.DaoUsuario _DaoUsuario = new LyDataAcces.DAO.DaoUsuario();
            LyDataAcces.DAO.DaoCategoria _DaoCategoria = new LyDataAcces.DAO.DaoCategoria();
            LyDataAcces.DAO.DaoServicios _DaoServicio = new LyDataAcces.DAO.DaoServicios();
            LyDataAcces.DAO.DaoCandidatura _Daocandidatura = new LyDataAcces.DAO.DaoCandidatura();
            LyDataAcces.XML.DaoXmlRead _DaoXml = new LyDataAcces.XML.DaoXmlRead();

            resultados = _DaoUsuario.RegistrarUsuario(new LyBussinesModel.DTO.DTOUsuario("prueba",
                                                                                        "prueba",
                                                                                        "prueba",
                                                                                        "aprueba",
                                                                                        "correofalso@falso.com",
                                                                                        Usuario.CreateHash("b", "b")));

            return;
            const int ESPACIOS_LINEAS = 45;
            //Inicio de Aplicación de test
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*"+"Iniciando Test de Persistencia de datos".PadRight(ESPACIOS_LINEAS, '-')+"*");
            Console.ForegroundColor = ConsoleColor.White;

            #region "Admin"
            VerificarOperacion("Crear Categoría Informática",
                _DaoCategoria.CrearCategoria("Informática"),
                _DaoCategoria);


            VerificarOperacion("Crear Categoría Electricista",
                _DaoCategoria.CrearCategoria("Electricista"),
                _DaoCategoria);


            VerificarOperacion("Crear Categoría Voluntariado",
                _DaoCategoria.CrearCategoria("Voluntariado"),
                _DaoCategoria);


            List<Categoria> categorias = new List<Categoria>();
            categorias=_DaoCategoria.GetAllCategorias();

            VerificarOperacion("Descarga todas las Categorías",
               categorias.Count() > 0,
               _DaoCategoria);

            if(categorias.Count() > 0)
                VerificarOperacion("Eliminar Categoría Voluntariado",
               _DaoCategoria.EliminarCategoria(categorias[categorias.Count()-1]),
               _DaoCategoria);
 
            VerificarOperacion("Obtener todos los usuarios",
                _DaoUsuario.GetAllUsuarios().Count>0,
                _DaoCategoria);
            #endregion

            #region "Aplicación inicial"

            //1.Registrar Usuario
            Console.WriteLine("Aplicación inicial");


            

            resultados = _DaoUsuario.RegistrarUsuario( new LyBussinesModel.DTO.DTOUsuario("prueba",
                                                                                            "prueba",
                                                                                            "prueba",
                                                                                            "aprueba", 
                                                                                            "correofalso@falso.com", 
                                                                                            Usuario.CreateHash("a","a")));

            VerificarOperacion("RegistroUsuario",resultados, _DaoUsuario);


            //2.Iniciar Sesión
            int idUsuario = _DaoUsuario.IniciarSesion("Usuario2", "1234");
            resultados = idUsuario > -1;
            VerificarOperacion("IniciarSesion",resultados, _DaoUsuario);

            if(resultados)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t"+"Inicio de sesión correcta");
                Console.ResetColor();
            }else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t"+"Fallo al iniciar sesión");
                Console.ResetColor();
            }
            #endregion

            #region "Mi Perfil"
            Console.WriteLine("Mi perfil");
            //3.Visualizar perfil de usuario.
            Usuario user = _DaoUsuario.GetPerfilUsuario(idUsuario);
            VerificarOperacion("Visualizando perfil usuario", user != null, _DaoUsuario);

            //4.Modificar perfil de usuario.
            VerificarOperacion("Modificación datos",resultados, _DaoUsuario);
            VerificarOperacion("Descargando Categorias de Usuario",_DaoCategoria.LoadCategoriaUsuario(user),_DaoUsuario);


            List<Categoria> listaCategorias = _DaoCategoria.GetAllCategorias();
            VerificarOperacion("Descarga de todas las categorias", listaCategorias.Count > 0, _DaoCategoria);

            //5.Asignar Categorias.
            List<LyBussinesModel.DTO.DTOCategoria> categoriasApuntadas = new List<LyBussinesModel.DTO.DTOCategoria>();
            //Se añaden las 2 primeras categorias
            categoriasApuntadas.Add(new LyBussinesModel.DTO.DTOCategoria(listaCategorias[0].Id));
            categoriasApuntadas.Add(new LyBussinesModel.DTO.DTOCategoria(listaCategorias[1].Id));

            LyBussinesModel.DTO.DTOUsuario cambioUsuario = new LyBussinesModel.DTO.DTOUsuario(user.Id)
            {
                Categorias = categoriasApuntadas
            };
            VerificarOperacion("Asignacion de categorias", _DaoUsuario.ModificarUsuario(cambioUsuario), _DaoUsuario);
            #endregion

            #region "Tablón de anuncios"
            Console.WriteLine("Tablon de anuncios");


            //11. Publicar un Servicio.
            resultados = _DaoServicio.RegistrarServicio(new LyBussinesModel.DTO.DTOServicios(idUsuario,
                                                                                "Se buscan programadores",
                                                                                "Se requieren conocimientos de c#", true, DateTime.Now));
            VerificarOperacion("Creando Un anuncio", resultados, _DaoServicio);
            //12. Anular un servicio.
            //10. Visualizar todos los anuncios
            List<Servicio> listaAnuncios = _DaoServicio.ListadoServicios();
            
            if(listaAnuncios!= null)
            {
                resultados = listaAnuncios.Count() > 0; 
            }else{
                resultados = false;
            }
            VerificarOperacion("Mostrando lista Anuncios", resultados, _DaoServicio);


            resultados = _Daocandidatura.Crearcandidatura(new LyBussinesModel.DTO.DTOCandidatura(idUsuario, listaAnuncios[0].Id, 3, DateTime.Now));
            VerificarOperacion("Registrando a un usuario en un Anuncio", resultados, _Daocandidatura);
            #endregion


            #region "Mi Cuenta"
            //6.Visualizar historial de Operaciones.
            //7.Visualizar detalles de operaciones


            //8.Visualizar Saldo.--> desde Usuario

            //9.Acceder a Anuncios propios
            //9.1 Visualizar


            //9.2 Modificar
            #endregion



            #region "Anuncios activos"
            Console.WriteLine("Gestión de Anuncios");
            //13 Editar anuncio
            //14 Visualizar perfil de candidatos.
            //15 Selección candidatura
            //16 Asignar tiempo empleado de servicio
            //17 Valorar a usuario
            //18 Eliminar anuncio
            #endregion

            #region "Candidatura Activas"
            //19 Visualizar anuncio
            //20 visualizar numero de candidatos.
            //21 Enviar Consulta.
            //22 Anular candadatura
            #endregion

            #region "XML"

            Console.WriteLine("Verificando lectura de xml:");
            String data= "<conexion><host>localhost\\SQLEXPRESS</host><user>sa</user><password>prueba</password></conexion>";
            LyDataAcces.XML.DaoXmlRead.DatosConexion? datosConexion = _DaoXml.ObtenerDatosServidorSql(data);
            resultados = datosConexion != null;
            VerificarOperacion("Leer Datos XML", resultados, _DaoUsuario);
            if(resultados)
            {
                LyDataAcces.XML.DaoXmlRead.DatosConexion conexion = (LyDataAcces.XML.DaoXmlRead.DatosConexion)datosConexion;
                Console.WriteLine("Host:"+conexion.Host);
                Console.WriteLine("User:" + conexion.User);
                Console.WriteLine("Password:" + conexion.Password);
            }


            #endregion

            //Fin de aplicació, para no salir y permitir que se lean los datos.
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*" + "Test Finalizado".PadRight(ESPACIOS_LINEAS, '-') + "*");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Pulse ENTER para finalizar...");
            Console.ReadLine();
        }
        static void VerificarOperacion(String funcion,bool resultados, LyDataAcces.DAO.IDao dao)
        {
            if (resultados)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\t"+funcion+" realizado con Exito");
                Console.ResetColor();
            }else{
                Exception error = dao.Errores;
                if (error != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\t"+"Error al procesar " + funcion +":"+ error.Message);
                    Console.ResetColor();
                    //if(error.GetHashCode == DbEntityValidationException)
                    //{
                    //    DbEntityValidationException e = (DbEntityValidationException)error;
                    //    foreach (var eve in e.EntityValidationErrors)
                    //    {
                    //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //        foreach (var ve in eve.ValidationErrors)
                    //        {
                    //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //                ve.PropertyName, ve.ErrorMessage);
                    //        }
                    //    }
                    //}



                }
            }
        }



    }
}
