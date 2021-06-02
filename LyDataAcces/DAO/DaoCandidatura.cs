using LyBussinesModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyDataAcces.DAO
{
    /// <summary>
    /// DaoCandidatura class.
    /// Acceso a datos de Candidatura.
    /// </summary>
    public class DaoCandidatura: IDao
    {
        /// <value>Error?</value>
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
        /// GetCandidatura.
        /// Acepta candidatura.
        /// </summary>
        /// <remarks>
        /// Esta clase valida la candidatura
        /// </remarks>
        /// <returns>
        /// Cierto o falso.
        /// </returns>
        public Candidatura GetCandidatura(int idCandidatura)
        {

            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    DaoUsuario daoUsuario = new DaoUsuario();
                    Candidatura retorno = new Candidatura();

                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda de la categoria en BD
                    var canDb = db.Candidatura.Find(idCandidatura);

                    if (canDb == null)
                    {
                        _Errores = new Exception("No se ha encontrado el id buscado");
                        return null;
                    }


                    retorno.Id = canDb.id;
                    retorno.Candidato = daoUsuario.GetPerfilUsuario(canDb.Usuarios.id);
                    retorno.FechaInscripcion = (DateTime)canDb.fechaInscripcion;
                    retorno.HorasRequeridas = (int)canDb.horasRequeridas;
                    retorno.Estado = (EstadoCandidatura)canDb.estado;

                    if(canDb.Candidatura_Finalizada != null && canDb.Candidatura_Finalizada.horasGanadas != null)
                    {
                        retorno.HorasGanadas = (int)canDb.Candidatura_Finalizada.horasGanadas;
                    }else{
                        retorno.HorasGanadas = 0;
                    }

                    return retorno;
                }
            }catch (Exception ex){
                _Errores = ex;
                return null;
            }

        }
 

        /// <summary>
        /// Crearcandidatura.
        /// Crea candidatura a servicio
        /// </summary>
        /// <param name="candidatura"></param>
        /// <returns>
        /// Cierto o falso
        /// </returns>
        public bool Crearcandidatura(LyBussinesModel.DTO.DTOCandidatura candidatura)
        {

            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    //Se realiza una introducción de datos en BD
                    ORM.Candidatura newcandidatura = new ORM.Candidatura();

                    newcandidatura.estado = (int?)EstadoCandidatura.PENDIENTE;
                    newcandidatura.fechaInscripcion = candidatura.FechaInscripcion;
                    newcandidatura.idServicio = candidatura.IdServicio;
                    newcandidatura.idUsuario = candidatura.IdUsuario;
                    newcandidatura.horasRequeridas = candidatura.HorasRequeridas;
/*
                    newcandidatura.Candidatura_Finalizada.valoracion = candidatura.Valoracion;
                    newcandidatura.Candidatura_Finalizada.comentarios = candidatura.Comentarios;
                    newcandidatura.Candidatura_Finalizada.horasGanadas = candidatura.HorasGanadas;

                    newcandidatura.Candidatura_Aceptada.fechaAceptacion = candidatura.FechaAceptacion;
                    newcandidatura.Candidatura_Aceptada.fechaAcordadaServicio = candidatura.FechaAcordadaServicio;
                    newcandidatura.Candidatura_Aceptada.horasAcordadas = candidatura.HorasAcordadas;

                    newcandidatura.Candidatura_Cancelada.fechaCancelacion = candidatura.FechaDeCancelacion;
                    newcandidatura.Candidatura_Cancelada.motivoCancelacion = candidatura.MotivoCancelacíon;
                    newcandidatura.Candidatura_Cancelada.autorCancelado = (int?)candidatura.AutorCancelado;
*/
                    db.Candidatura.Add(newcandidatura);
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
        /// CancelarCandidatura.
        /// Tranforma candidatura a cancelada
        /// </summary>
        /// <param name="idCandidatura"></param>
        /// <param name="canceladopor"></param>
        /// <param name="motivo"></param>
        /// <returns>
        /// Cierto o falso
        /// </returns>
        public bool CancelarCandidatura(int idCandidatura,CanceladoPor canceladopor,String motivo)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda de la categoria en BD
                    var query = db.Candidatura.Find(idCandidatura);
                    
                    if (query == null)
                    {
                        _Errores = new Exception("No se ha encontrado el id buscado");
                        return false;
                    }

                    //para cancelar la candidatura no puede estar finalizada
                    EstadoCandidatura estadoAnterior = (EstadoCandidatura)query.estado;
                    if (estadoAnterior == EstadoCandidatura.FINALIZADA )
                    {
                        _Errores = new Exception("La candidatura se encuentra "+ estadoAnterior.ToString());
                        return false;
                    }

                    ORM.Candidatura_Cancelada cancelacion = new ORM.Candidatura_Cancelada();
                    cancelacion.motivoCancelacion = motivo;
                    query.estado = (int?)EstadoCandidatura.CANCELADA;
                    query.Candidatura_Cancelada = cancelacion;
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
        /// Aceptarcandidatura.
        /// Transforma candidatura a seleccionada.
        /// </summary>
        /// <param name="idCandidatura"></param>
        /// <param name="fechacordada"></param>
        /// <param name="horascaordadas"></param>
        /// <returns>
        /// Cierto o falso
        /// </returns>
        public bool Aceptarcandidatura(int idCandidatura, int horascaordadas, DateTime fechacordada)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda de la categoria en BD
                    var query = db.Candidatura.Find(idCandidatura);

                    if (query == null)
                    {
                        _Errores = new Exception("No se ha encontrado el id buscado");
                        return false;
                    }

                    EstadoCandidatura estadoAnterior = (EstadoCandidatura)query.estado;
                    if (estadoAnterior != EstadoCandidatura.PENDIENTE)
                    {
                        _Errores = new Exception("La candidatura se encuentra " + estadoAnterior.ToString());
                        return false;
                    }


                    ORM.Candidatura_Aceptada aceptada = new ORM.Candidatura_Aceptada();

                    query.estado = (int?)EstadoCandidatura.ACEPTADA;
                    aceptada.fechaAceptacion = DateTime.Now;
                    aceptada.horasAcordadas = horascaordadas;
                    aceptada.fechaAcordadaServicio = fechacordada;
                    query.Candidatura_Aceptada = aceptada;

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
        /// FinalizarCandidatura.
        /// Transforma candidatura a finalizada.
        /// </summary>
        /// <param name="idCandidatura"></param>
        /// <param name="horasGanadas"></param>
        /// <param name="valoracion"></param>
        /// <param name="comentarios"></param>
        /// <returns>
        /// Cierto o falso
        /// </returns>
        public bool FinalizarCandidatura(int idCandidatura, int horasGanadas, int valoracion, String comentarios)
        {
            try
            {
                using (ORM.EFBancoTiempo db = new ORM.EFBancoTiempo())
                {
                    /// <value>Valor consulta bd</value>
                    //Se realiza una busqueda de la categoria en BD
                    var candidatura = db.Candidatura.Find(idCandidatura);

                    if (candidatura == null)
                    {
                        _Errores = new Exception("No se ha encontrado el id buscado");
                        return false;
                    }

                   EstadoCandidatura estadoAnterior = (EstadoCandidatura)candidatura.estado;
                    if (estadoAnterior != EstadoCandidatura.ACEPTADA)
                    {
                        _Errores = new Exception("La candidatura se encuentra " + estadoAnterior.ToString());
                        return false;
                    }


  
                    ORM.Candidatura_Finalizada finalizada = new ORM.Candidatura_Finalizada();
                    candidatura.estado = (int)EstadoCandidatura.FINALIZADA;
                    finalizada.valoracion = valoracion;
                    finalizada.comentarios = comentarios;
                    finalizada.horasGanadas = horasGanadas;
                    candidatura.Candidatura_Finalizada = finalizada;

                    //REALIZAR CAMBIOS DE HORAS
                    if(candidatura.Servicios.Usuarios.horasAcumuladas - horasGanadas < 0)
                    {
                        _Errores = new Exception("No pude darle estas horas lo maximo que tiene son: " +
                            candidatura.Servicios.Usuarios.horasAcumuladas.ToString());
                        return false;
                    }

                    candidatura.Servicios.Usuarios.horasAcumuladas -=  horasGanadas;

                    int horasCandidato = horasGanadas;
                    if(candidatura.Usuarios.horasAcumuladas != null && candidatura.Usuarios.horasAcumuladas > 0)
                    {

                        horasCandidato = horasCandidato + (int)candidatura.Usuarios.horasAcumuladas;
                    }

                   candidatura.Usuarios.horasAcumuladas = horasCandidato;





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
