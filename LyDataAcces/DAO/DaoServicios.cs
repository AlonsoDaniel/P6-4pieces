using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyBussinesModel;
using LyBussinesModel.DTO;

namespace LyDataAcces.DAO
{
    /// <summary>
    /// DaoServicios class.
    /// Acceso a datos de Servicios.
    /// </summary>
    public class DaoServicios : IDao
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
        /// RegistrarServicio.
        /// Registra un Servicio en la base de datos
        /// </summary>
        /// <param name="datos">DTOServicios con toda la información del servicio</param>
        /// <returns>Si la operación ha sido correcta true, de lo contrario false y se puede consultar el error desde la propiedad Errores</returns>
        public bool RegistrarServicio(LyBussinesModel.DTO.DTOServicios datos)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {

                    ORM.Servicios nuevoServicio = new ORM.Servicios
                    {
                        idCreador = datos.idCreador,
                        titulo = datos.titulo,
                        descripcion = datos.descripcion,
                        fechaCreacion = DateTime.Now,
                        finalizado = false
                    };

                    //Creación de las categorias
                    if (datos.Categorias != null)
                    {
                        if (datos.Categorias.Count > 0)
                        {
                            /// <value>Valor consulta bd</value>
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
                            nuevoServicio.Categorias = nuevasCategorias;
                        }
                    }
                    db.Servicios.Add(nuevoServicio);
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
        /// ListadoServicios.
        /// Petición del listado de Servicios no finalizados (sólo activos)
        /// </summary>
        /// <returns>Listado</returns>
        public List<Servicio> ListadoServicios()
        {
            try
            {
                List<Servicio> allServicios = new List<Servicio>();
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.finalizado == false select b;

                    if(queryServicios.Count() > 0)
                    {
                        return PeticionListado(queryServicios.ToList());

                    }else {
                        return null; 
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
        /// ListadoServicios.
        /// Listar N servicios (sólo activos)
        /// </summary>
        /// <remarks>
        /// <para>Devuelve N Servicios. Si p != null devuelve N servicios a partir de la página p</para>
        /// <para>NOTA: La p equivale a la página a mostrar, por lo que en la función siempre se le restará 1 </para>
        /// <para>porque para la página 1 se muestran los N primeros y no se salta ningún resultado</para>
        /// </remarks>
        /// <param name="n">Número de servicios a mostrar</param>
        /// <param name="p">Página Actual del registro</param>
        /// <returns>Listado de Servicios </returns>
        public List<Servicio> ListadoServicios(int n, int? p)
        {
            try
            {
                List<Servicio> allServicios = new List<Servicio>();
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.finalizado == false select b;

                    if (queryServicios.Count() > 0)
                    {

                        int page = p == null || p < 0 ? 0 : (int) p - 1;
                        /// <value>Valor consulta bd</value>
                        var queryResult = page == 0 ? queryServicios.Take(n).ToList() : queryServicios.Skip(n * page).Take(n).ToList();

                        return PeticionListado(queryResult);
                    }
                    else { return null; }

                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return null;
            }
        }

        /// <summary>
        /// ListadoServiciosUsuario.
        /// Listado de Servicios de un usuario (Todos)
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Listado</returns>
        public List<Servicio> ListadoServiciosUsuario(Usuario usuario)
        {
            try
            {
                List<Servicio> allServicios = new List<Servicio>();
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.idCreador == usuario.Id select b;

                    if (queryServicios.Count() > 0)
                    {
                        return PeticionListado(queryServicios.ToList());
                    }
                    else { return null; }

                }

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return null;
            }
        }

        /// <summary>
        /// PeticionListado.
        /// Método para realizar la petición del listado que se repite en los métodos para listar todos o listar de user
        /// </summary>
        /// <param name="queryServicios"></param>
        /// <returns>Listado</returns>
        private List<Servicio> PeticionListado(List<ORM.Servicios> queryResult)
        {
            List<Servicio> allServicios = new List<Servicio>();
            DaoUsuario _DaoUsuario = new DaoUsuario();
            DaoCategoria _DaoCategoria = new DaoCategoria();
            // Faltan CATEGORIAS
            //DaoCandidaturas _DaoCandidatura = new DaoCandidaturas();
            try
            {

                foreach (ORM.Servicios dbservicios in queryResult)
                {
                    Usuario user = _DaoUsuario.GetPerfilUsuario((int)dbservicios.idCreador);

                    Servicio servicio = new Servicio(dbservicios.titulo,
                                                            dbservicios.descripcion,
                                                            dbservicios.fechaCreacion,
                                                            dbservicios.id,
                                                            user,
                                                            (bool)dbservicios.finalizado
                                                    )
                    {


                        //Recogida de las categorias

                        //* # Categorias no tiene el método correcto para devolver el listado de categorias de un servicio.
                        Categorias = new List<Categoria>()
                    };
                    _DaoCategoria.LoadCategoriaServicio(servicio);

                    allServicios.Add(servicio);
                }

                return allServicios;

            }
            catch (Exception ex)
            {
                _Errores = ex;
                return null;
            }
        }


        /// <summary>
        /// GetListServicios.
        /// Método para realizar la petición del listado que se repite en los métodos para listar todos o listar de user
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="finalizados"></param>
        /// <returns>Listado</returns>
        public List<DTOServicios> GetListServicios(int idUsuario, Boolean finalizados)
        {
            try
            {
                List<DTOServicios> allServicios = new List<DTOServicios>();
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.idCreador == idUsuario && b.finalizado == finalizados select b;

                    if (queryServicios.Count() > 0)
                    {
                        foreach (ORM.Servicios servicio in queryServicios)
                        {
                            DTOServicios nuevoSErvicio = new DTOServicios()
                            {
                                id = servicio.id,
                                descripcion = servicio.descripcion,
                                titulo = servicio.titulo,
                                fechaCreacion = servicio.fechaCreacion,
                                nombreCreador = servicio.Usuarios.nombre
                            };

                            nuevoSErvicio.Categorias = new List<DTOCategoria>();

                            if (servicio.Categorias != null && servicio.Categorias.Count() > 0)
                            {
                                foreach (ORM.Categorias categoria in servicio.Categorias)
                                {
                                    nuevoSErvicio.Categorias.Add(new DTOCategoria(categoria.id, categoria.nombre));
                                }

                            }
                            allServicios.Add(nuevoSErvicio);
                        }

                        return allServicios;
                    }
                    else
                    {
                        return null;
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
        /// GetListServicios.
        /// Método para realizar la petición del listado que se repite en los métodos para listar todos o listar de user
        /// </summary>
        /// <returns>Listado</returns>
        public List<DTOServicios> GetListServicios()
        {
            try
            {
                List<DTOServicios> allServicios = new List<DTOServicios>();
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios 
                                         where  b.finalizado == false
                                         orderby b.id descending
                                         select b ;

                    if (queryServicios.Count() > 0)
                    {
                        foreach (ORM.Servicios servicio in queryServicios)
                        {
                            DTOServicios nuevoSErvicio = new DTOServicios()
                            {
                                id = servicio.id,
                                descripcion = servicio.descripcion,
                                titulo = servicio.titulo,
                                fechaCreacion = servicio.fechaCreacion,
                                nombreCreador = servicio.Usuarios.nombre
                            };

                            nuevoSErvicio.Categorias = new List<DTOCategoria>();
                            

                            if (servicio.Categorias != null && servicio.Categorias.Count() > 0)
                            {
                                foreach (ORM.Categorias categoria in servicio.Categorias)
                                {
                                    nuevoSErvicio.Categorias.Add(new DTOCategoria(categoria.id, categoria.nombre));
                                }

                            }
                            allServicios.Add(nuevoSErvicio);
                        }

                        return allServicios;
                    }
                    else
                    {
                        return null;
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
        /// GetServiciosDetalles.
        /// Petición de detalles de Servicio
        /// </summary>
        /// <param name="idServicio"></param>
        /// <param name="idUsuarioQueQuiereSolicitarla"></param>
        /// <returns>Cadena</returns>
        public DTOServiciosDetalles GetServiciosDetalles(int idServicio, int idUsuarioQueQuiereSolicitarla = -1)
        {
            try
            {
       
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    DTOServiciosDetalles resultados = null;

                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.id == idServicio select b;

                    if (queryServicios.Count() > 0)
                    {

                        DaoUsuario daoUser = new DaoUsuario();
                        DTOUsuario creador = null;

                        int idCreador = -1;

                        ORM.Servicios sv = queryServicios.FirstOrDefault();
                        if(sv.idCreador != null)
                        {
                            idCreador = (int)sv.idCreador;
                            creador = daoUser.GetDetellaUsuario(idCreador);
                        }
                        
                        int cantidadServicios = 0;
                        ResultadoDetalleServicioPuedeSolicitarse puedeSolicitarse =ResultadoDetalleServicioPuedeSolicitarse.PUEDE;


                        if(sv.Candidatura != null && sv.Candidatura.Count() > 0)
                        {
                            cantidadServicios = sv.Candidatura.Count();
                            //TODO si tiene candidaturas, se recorren las que esten con estado finalizada y se obtiene
                            //La puntuación para realizar media
                            /// <value>Valor consulta bd</value>
                            var candidaturas = sv.Candidatura.Where(x => x.idUsuario == idUsuarioQueQuiereSolicitarla);
                            //Si la función recibe la id de usuario que quiere solicitar se verifica si ya esta solicitada
                            foreach (ORM.Candidatura candidatura in candidaturas)
                            {

                                if((candidatura.estado == (int)EstadoCandidatura.PENDIENTE
                                    ||  candidatura.estado == (int)EstadoCandidatura.ACEPTADA ))
                                {
                                    puedeSolicitarse = ResultadoDetalleServicioPuedeSolicitarse.YA_SOLICITADA;
                                    break;
                                }

                            }

                        }
                        
                        if(creador.HorasAcumuladas <= 0)
                        {
                            puedeSolicitarse = ResultadoDetalleServicioPuedeSolicitarse.HORAS_INSUFICIENTES;
                        }

                        
                        resultados = new DTOServiciosDetalles
                        {
                            id = sv.id,
                            Titulo = sv.titulo,
                            Descripcion = sv.descripcion,
                            CantidadSolicitudes = cantidadServicios,
                            FechaDeInicio = sv.fechaCreacion,
                            SePuedeSolicitar = puedeSolicitarse,
                            Finalizado = (bool)sv.finalizado
                        };

                        if(idCreador > 0)
                        {
                            resultados.Creador = creador;
                        }

                        resultados.Categorias = new List<DTOCategoria>();

                        
                        if (sv.Categorias != null  && sv.Categorias.Count() > 0)
                        {
                            foreach (ORM.Categorias categoria in sv.Categorias)
                            {
                                resultados.Categorias.Add(new DTOCategoria(categoria.id, categoria.nombre));
                            }

                        }

                        return resultados;
                    }else{ 
                        return null; 
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
        /// GetListadoCandidaturas.
        /// Petición de candidatos
        /// </summary>
        /// <param name="idServicio"></param>
        /// <returns>Listado</returns>
        public List<DTOListadoCandidaturasEnServicio> GetListadoCandidaturas(int idServicio)
        {
            try
            {

                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    List<DTOListadoCandidaturasEnServicio> resultados =new List<DTOListadoCandidaturasEnServicio>();

                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.id == idServicio select b;

                    if (queryServicios.Count() > 0)
                    {

                        ORM.Servicios sv = queryServicios.FirstOrDefault();
                        if (sv.idCreador != null)
                        {
                            if(sv.Candidatura.Count > 0)
                            {
                                foreach(ORM.Candidatura candidatura in sv.Candidatura)
                                {
                                    DTOListadoCandidaturasEnServicio nuevo = new DTOListadoCandidaturasEnServicio
                                                                                {
                                                                                    Id = candidatura.id,
                                                                                    //Nombre = candidatura.Usuarios.nombre,
                                                                                    Usuario = candidatura.Usuarios.nombreUsuario,
                                                                                    //Apellido = candidatura.Usuarios.apellidos,
                                                                                    //Correo = candidatura.Usuarios.correo,
                                                                                    //Telefono = candidatura.Usuarios.telefono,
                                                                                    Horas_Solicitadas = (int)candidatura.horasRequeridas,
                                                                                    Horas_Ganadas = 0,
                                                                                    Fecha_Inscripcion = (DateTime)candidatura.fechaInscripcion,
                                                                                    Estado = (EstadoCandidatura)candidatura.estado
                                                                                };
                                  

                                    if(candidatura.Candidatura_Finalizada != null && candidatura.Candidatura_Finalizada.horasGanadas != null)
                                    {
                                        nuevo.Horas_Ganadas = (int)candidatura.Candidatura_Finalizada.horasGanadas;
                                    }
                                    resultados.Add(nuevo);

                                }
                            }
                        }

                        return resultados;
                    }
                    else
                    {
                        return null;
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
        /// ModificarServicio.
        /// Modificar un servicio
        /// </summary>
        /// <param name="datos"></param>
        /// <returns>Bool confirmando cambio</returns>
        public bool ModificarServicio(DTOServicios datos)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {

                    DTOServiciosDetalles resultados = null;
                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.id == datos.id select b;
                    ORM.Servicios sv = queryServicios.FirstOrDefault();


                    if (sv != null)
                    {

                        if (datos.titulo != null)
                        {
                            sv.titulo = datos.titulo;
                        }

                        if (datos.descripcion != null)
                        {
                            sv.descripcion = datos.descripcion;
                        }


                        //Creación de las categorias
                        if (datos.Categorias != null)
                        {
                            if (datos.Categorias.Count > 0)
                            {
                                /// <value>Valor consulta bd</value>
                                var queryCategorias = from b in db.Categorias select b;

                                ICollection<ORM.Categorias> nuevasCategorias = new List<ORM.Categorias>();
                                //elimna todos las categorias que no esten marcadas
                                sv.Categorias.Clear();
                                foreach (LyBussinesModel.DTO.DTOCategoria categoria in datos.Categorias)
                                {
                                    ORM.Categorias categoriaSeleccionada = queryCategorias.First(c => c.id == categoria.idCategoria);
                                    if (categoriaSeleccionada != null)
                                    {
                                        nuevasCategorias.Add(categoriaSeleccionada);
                                    }

                                }
                                sv.Categorias = nuevasCategorias;
                            }
                        }

                        db.SaveChanges();
                        return true;
                    }else{
                        _Errores = new Exception("No se encontro el servicio");
                        return false;
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
        /// FinalizarServicio.
        /// Dar de baja un servicio (Finalizar)
        /// </summary>
        /// <param name="idServicio"></param>
        /// <returns>Bool confirmando ejecución</returns>

        public bool FinalizarServicio(int idServicio)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {

                    DTOServiciosDetalles resultados = null;
                    /// <value>Valor consulta bd</value>
                    var queryServicios = from b in db.Servicios where b.id == idServicio select b;

                    if (queryServicios.Count() > 0)
                    {
                        ORM.Servicios sv = queryServicios.FirstOrDefault();
                        if (sv.Candidatura != null && sv.Candidatura.Count() > 0)
                        {
                            sv.Candidatura.Count();
                            //TODO si tiene candidaturas, se recorren las que esten con estado finalizada y se obtiene
                            //La puntuación para realizar media
                            /// <value>Valor consulta bd</value>
                            var candidaturas = sv.Candidatura.Where(x => x.estado == (int)EstadoCandidatura.ACEPTADA);
                            if (candidaturas.Count() > 0)
                            {
                                _Errores = new Exception("No puede dar de baja el servicio habiendo servicios esperando a ser finalizados");
                                return false;
                            }
                        }

                        sv.finalizado = true;
                        db.SaveChanges();
                        return true;
                    }else{
                        _Errores = new Exception("No se encontro el servicio");
                        return false;
                    }
                    return true;
                }

            } catch (Exception ex){
                _Errores = ex;
                return false;
            }
        }
    }
}
