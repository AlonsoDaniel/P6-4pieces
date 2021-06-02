Imports LyBussinesModel
Imports LyDataAcces

''' <summary>
''' FrmMisServicios class.
''' Formulario resumen actividad servicios.
''' </summary>
Public Class FrmMisServicios

    ''' <summary>
    ''' Volver_Disposed.
    ''' </summary>
    Private Sub Volver_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Main.VolverAlMenu(Me)
    End Sub

    ''' <summary>
    ''' FrmMisServicios_VisibleChanged.
    ''' </summary>
    Private Sub FrmMisServicios_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        Main.AbribVentana(Me)
    End Sub

    ''' <summary>
    ''' UcListaServicios_Load.
    ''' </summary>
    Private Sub UcListaServicios_Load(sender As Object, e As EventArgs) Handles UcListaServicios.Load
        Dim listaServiciosUser As List(Of DTO.DTOServicios)
        LblCurrentMenu.Text = "Activos"
        UploadListaServicios(listaServiciosUser, True)
    End Sub

    ''' <summary>
    ''' UploadListaServicios.
    ''' </summary>
    Private Sub UploadListaServicios(listaServiciosUser As List(Of DTO.DTOServicios), estado As Boolean)
        Dim dao As DAO.DaoServicios = New DAO.DaoServicios()
        Dim dto As DTO.DTOServicios = New DTO.DTOServicios()
        listaServiciosUser = dao.GetListServicios(UsuarioAutenticado.Id, estado)
    End Sub

    ''' <summary>
    ''' BtnActivos_Click.
    ''' </summary>
    Private Sub BtnActivos_Click(sender As Object, e As EventArgs) Handles BtnActivos.Click
        Dim listaServiciosUser As List(Of DTO.DTOServicios)
        LblCurrentMenu.Text = "Activos"
        UploadListaServicios(listaServiciosUser, True)
    End Sub

    ''' <summary>
    ''' BtnFinalizados_Click.
    ''' </summary>
    Private Sub BtnFinalizados_Click(sender As Object, e As EventArgs) Handles BtnFinalizados.Click
        Dim listaServiciosUser As List(Of DTO.DTOServicios)
        LblCurrentMenu.Text = "Finalizados"
        UploadListaServicios(listaServiciosUser, False)
    End Sub

    ''' <summary>
    ''' BtnVolver_Click.
    ''' </summary>
    Private Sub BtnVolver_Click(sender As Object, e As EventArgs) Handles BtnVolver.Click
        Dispose()
    End Sub

End Class