
''' <summary>
''' FrmGestionCandidaturas class.
''' Formulario de gestión y consulta de candidaturas
''' </summary>
Public Class FrmGestionCandidaturas

    Private Enum Accion As Integer
        ACEPTAR
        CANCELAR
        FINALIZAR
    End Enum

    Private _Accion As Accion
    Private _Cantidatura As LyBussinesModel.Candidatura

    ''' <summary>
    ''' OpenDialog.
    ''' </summary>
    Public Sub OpenDialog(idCandidatura As Integer)
        LoadCandidatura(idCandidatura)
        Me.ShowDialog()
    End Sub

    ''' <summary>
    ''' LoadCandidatura.
    ''' </summary>
    Private Sub LoadCandidatura(idCandidatura As Integer)
        Dim dao As New LyDataAcces.DAO.DaoCandidatura
        _Cantidatura = dao.GetCandidatura(idCandidatura)
        PnAccion.Hide()
        PreparaVentana()

        If _Cantidatura Is Nothing Then

            Dim Errores As Exception = DirectCast(dao, LyDataAcces.DAO.IDao).Errores
            If Errores IsNot Nothing Then
                MsgBox("error producido" + Errores.Message)
            End If
            Me.Close()
        Else

            LblUsuario.Text = _Cantidatura.Candidato.NombreUsuario
            lblNombre.Text = _Cantidatura.Candidato.Nombre
            LblApellidos.Text = _Cantidatura.Candidato.Apellidos
            LblCorreo.Text = _Cantidatura.Candidato.Correo
            LblTelefono.Text = _Cantidatura.Candidato.Telefono
            LblFechaInscripcion.Text = _Cantidatura.FechaInscripcion.ToString()
            LblHorasRequeridas.Text = _Cantidatura.HorasRequeridas.ToString()
            LblTags.Text = String.Join("   ", _Cantidatura.Candidato.Categorias.Select(Function(x) "#" + x.Nombre))
            LblHorasGanadas.Text = _Cantidatura.HorasGanadas.ToString()
            Select Case _Cantidatura.Estado
                Case LyBussinesModel.EstadoCandidatura.PENDIENTE
                    BtnAceptar.Visible = True : BtnAceptar.Enabled = True
                    BtnCancelar.Visible = True : BtnCancelar.Enabled = True
                    LblEstado.Text = "PENDIENTE"
                    LblEstado.ForeColor = Color.Orange
                    Label3.Visible = True
                Case LyBussinesModel.EstadoCandidatura.ACEPTADA
                    BtnFinalizar.Visible = True : BtnCancelar.Enabled = True
                    BtnCancelar.Visible = True : BtnCancelar.Enabled = True
                    LblEstado.Text = "ACEPTADA"
                    LblEstado.ForeColor = Color.Lime
                    Label3.Visible = True
                Case LyBussinesModel.EstadoCandidatura.CANCELADA
                    LblEstado.Text = "CANCELADA"
                    LblEstado.ForeColor = Color.Red
                    Label3.Visible = False
                Case LyBussinesModel.EstadoCandidatura.FINALIZADA
                    LblEstado.Text = "FINALIZADA"
                    LblEstado.ForeColor = Color.Green
                    Label3.Visible = False
            End Select
        End If
    End Sub

    ''' <summary>
    ''' PreparaVentana.
    ''' </summary>
    Public Sub PreparaVentana()
        BtnAceptar.Visible = False : BtnAceptar.Enabled = False
        BtnCancelar.Visible = False : BtnCancelar.Enabled = False
        BtnFinalizar.Visible = False : BtnCancelar.Enabled = False

        LblUsuario.Text = ""
        lblNombre.Text = ""
        LblApellidos.Text = ""
        LblCorreo.Text = ""
        LblTelefono.Text = ""
        LblFechaInscripcion.Text = ""
        LblHorasRequeridas.Text = ""
        LblEstado.Text = ""
        LblTags.Text = ""
        LblHorasGanadas.Text = ""
    End Sub

    ''' <summary>
    ''' BtnAccion_Click.
    ''' </summary>
    Private Sub BtnAccion_Click(sender As Object, e As EventArgs) Handles BtnAceptar.Click, BtnCancelar.Click, BtnFinalizar.Click

        RtxtMotivoCancelacion.Visible = False
        NudHoras.Visible = False
        NudHoras.Value = 1
        Select Case DirectCast(sender, Button).Name
            Case "BtnAceptar"
                _Accion = Accion.ACEPTAR
                LblAccion.Text = "Aceptar Solicitud"
                lblInfo.Text = "Antes de realizar la aceptación de la solicitud," +
                    "pongase en contacto con el candidato por medio de los datos facilitados"
            Case "BtnCancelar"
                _Accion = Accion.CANCELAR
                LblAccion.Text = "Cancelar Solicitud"
                lblInfo.Text = " Indique los motivos de la cancelacion"
                RtxtMotivoCancelacion.Visible = True
            Case "BtnFinalizar"
                _Accion = Accion.FINALIZAR
                LblAccion.Text = "Finalizar Solicitud"
                lblInfo.Text = "Indique las horas acordadas"
                NudHoras.Visible = True
        End Select

        PnAccion.Show()
    End Sub

    ''' <summary>
    ''' BtnAccion_Click_1.
    ''' </summary>
    Private Sub BtnAccion_Click_1(sender As Object, e As EventArgs) Handles BtnAccion.Click
        Dim dao As New LyDataAcces.DAO.DaoCandidatura
        Dim resultado As Boolean = False

        Select Case _Accion
            Case Accion.ACEPTAR
                resultado = dao.Aceptarcandidatura(_Cantidatura.Id, 0, Date.Now)
            Case Accion.CANCELAR
                resultado = dao.CancelarCandidatura(_Cantidatura.Id, LyBussinesModel.CanceladoPor.OFERTANTE, RtxtMotivoCancelacion.Text)
            Case Accion.FINALIZAR
                resultado = dao.FinalizarCandidatura(_Cantidatura.Id, NudHoras.Value, 0, "")
        End Select

        If resultado = False Then
            Dim errores As Exception = DirectCast(dao, LyDataAcces.DAO.IDao).Errores

            If errores IsNot Nothing Then
                MsgBox("No se ha podido procesar la solicitud " + errores.Message)
            Else
                MsgBox("No se ha podido procesar la solicitud error no especificado")
            End If

        End If
        LoadCandidatura(_Cantidatura.Id)
    End Sub


End Class