using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LyBussinesModel
{

    public enum EstadoCandidatura { PENDIENTE, ACEPTADA, CANCELADA, FINALIZADA }
    public enum CanceladoPor { CANDIDATO, OFERTANTE }

    /// <summary>
    /// Candidatura class.
    /// Recoge todos los datos de alta y cancelación.
    /// </summary>
    /// <remarks>
    /// Esta clase registra inscripciones, cancelaciones de candidatos y permite anotaciones.
    /// </remarks>
    public class Candidatura
    {
        /// <value>Valores de identificación</value>
        private int _Id;
        private Usuario _Candidato;
        private Servicio _Servicio;
        private EstadoCandidatura _Estado;

        /// <value>Valores de fecha/tiempo</value>
        private DateTime _FechaInscripcion;
        private int _HorasRequeridas;

        /// <value>Valores de finalización</value>
        private int _Valoracion;
        private String _Comentarios;
        private int _HorasGanadas;

        /// <value>Valores de aceptación</value>
        private DateTime _FechaAceptacion;
        private DateTime _FechaAcordadaServicio;
        private int _HorasAcordadas;

        /// <value>Valores de cancelación</value>
        private DateTime _FechaDeCancelacion;
        private String _MotivoCancelacíon;
        private CanceladoPor _AutorCancelado;

        public Candidatura() { }

        public Candidatura(int id, Usuario candidato, Servicio servicio, EstadoCandidatura estado, DateTime fechaInscripcion, int horasRequeridas)
        {
            _Id = id;
            _Candidato = candidato;
            _Servicio = servicio;
            _Estado = estado;
            _FechaInscripcion = fechaInscripcion;
            _HorasRequeridas = horasRequeridas;
        }

        public int Id { get => _Id; set => _Id = value; }
        public Usuario Candidato { get => _Candidato; set => _Candidato = value; }
        public Servicio Servicio { get => _Servicio; set => _Servicio = value; }
        public EstadoCandidatura Estado { get => _Estado; set => _Estado = value; }
        public DateTime FechaInscripcion { get => _FechaInscripcion; set => _FechaInscripcion = value; }
        public int HorasRequeridas { get => _HorasRequeridas; set => _HorasRequeridas = value; }
        public int Valoracion { get => _Valoracion; set => _Valoracion = value; }
        public string Comentarios { get => _Comentarios; set => _Comentarios = value; }
        public int HorasGanadas { get => _HorasGanadas; set => _HorasGanadas = value; }
        public DateTime FechaAceptacion { get => _FechaAceptacion; set => _FechaAceptacion = value; }
        public DateTime FechaAcordadaServicio { get => _FechaAcordadaServicio; set => _FechaAcordadaServicio = value; }
        public int HorasAcordadas { get => _HorasAcordadas; set => _HorasAcordadas = value; }
        public DateTime FechaDeCancelacion { get => _FechaDeCancelacion; set => _FechaDeCancelacion = value; }
        public string MotivoCancelacíon { get => _MotivoCancelacíon; set => _MotivoCancelacíon = value; }
        public CanceladoPor AutorCancelado { get => _AutorCancelado; set => _AutorCancelado = value; }
    }
}
