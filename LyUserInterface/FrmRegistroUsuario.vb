Imports LyBussinesModel
Imports LyDataAcces

''' <summary>
''' FrmRegistroUsuario class.
''' Formulario de registro.
''' </summary>
Public Class FrmRegistroUsuario

    ''' <summary>
    ''' FrmRegistroUsuario_Disposed.
    ''' </summary>
    Private Sub FrmRegistroUsuario_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        FrmInicioSesion.Show()
    End Sub

    ''' <summary>
    ''' BtnRegistrarUsuario_Click.
    ''' </summary>
    Private Sub BtnRegistrarUsuario_Click(sender As Object, e As EventArgs) Handles BtnRegistrar.Click

        Dim nuevoUsuario = New DTO.DTOUsuario(
            TxtNombreUsuario.Text.ToLower(),
            TxtNombre.Text,
            TxtApellidos.Text,
            TxtTelefono.Text,
            TxtEmail.Text,
            Usuario.CreateHash(TxtNombreUsuario.Text, TxtPassword.Text)
        )

        Dim daoUsuario = New DAO.DaoUsuario()

        If (daoUsuario.RegistrarUsuario(nuevoUsuario)) Then

            Login(TxtNombreUsuario.Text, TxtPassword.Text)
            FrmMain.Show()
            Me.Hide()

        Else
            LblError.Text = DirectCast(daoUsuario, LyDataAcces.DAO.IDao).Errores.Message
            MsgBox("Error producido: " + LblError.Text, MsgBoxStyle.Critical, "Error registrando usuario")

        End If
    End Sub

    ''' <summary>
    ''' BtnCancelarRegistro_Click.
    ''' </summary>
    Private Sub BtnCancelarRegistro_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        FrmInicioSesion.Show()
        Me.Hide()
    End Sub

    ''' <summary>
    ''' FrmRegistroUsuario_Load.
    ''' </summary>
    Private Sub FrmRegistroUsuario_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class