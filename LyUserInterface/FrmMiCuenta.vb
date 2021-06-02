Imports System.Text.RegularExpressions
Imports LyBussinesModel
Imports LyBussinesModel.DTO
Imports LyDataAcces

''' <summary>
''' FrmMiCuenta class.
''' Formulario resumen cuenta.
''' </summary>
Public Class FrmMiCuenta

    ''' <summary>
    ''' CargarDatosPanelPpal.
    ''' </summary>
    Private Sub CargarDatosPanelPpal()
        TxtNombre.Text = Main.UsuarioAutenticado.Nombre
        TxtApellidos.Text = Main.UsuarioAutenticado.Apellidos
        TxtTelefono.Text = Main.UsuarioAutenticado.Telefono
        TxtEmail.Text = Main.UsuarioAutenticado.Correo
        TxtUsuario.Text = Main.UsuarioAutenticado.NombreUsuario
        LblSaldo.Text = Main.UsuarioAutenticado.HorasAcumuladas.ToString() + " h."

        LBoxCategorias.Items.Clear()
        For Each Categoria In Main.UsuarioAutenticado.Categorias
            LBoxCategorias.Items.Add(Categoria.Nombre)
        Next Categoria
        If LBoxCategorias.Items.Count = 0 Then
            LBoxCategorias.Items.Add("Sin categorías asignadas")
        End If
        PnlMisDatos.Visible = True
        PnlMisDatos.Enabled = True
        TxtNombre.ReadOnly = True
        TxtApellidos.ReadOnly = True
        TxtTelefono.ReadOnly = True
        TxtEmail.ReadOnly = True
        TxtUsuario.ReadOnly = True
        TxtPassword.ReadOnly = True
        TxtPasswordConfirm.ReadOnly = True
        BtnSaveChanges.Visible = False
        BtnCancelEdit.Visible = False
        BtnEditarUser.Visible = True
        PnlCategorias.Visible = False
        PnlCategorias.Enabled = False
        PnlMiCartera.Visible = False
        PnlMiCartera.Enabled = False

    End Sub

    ''' <summary>
    ''' ReadCategorias.
    ''' </summary>
    Private Function ReadCategorias() As List(Of LyBussinesModel.DTO.DTOCategoria)
        Dim retorno As New List(Of DTOCategoria)
        For Each cat In CblCategorias.CheckedItems
            Dim categoria = DirectCast(cat, LyBussinesModel.Categoria)
            retorno.Add(New DTOCategoria(categoria.Id, categoria.Nombre))
        Next
        Return retorno
    End Function

    ''' <summary>
    ''' loadCategorias.
    ''' </summary>
    Private Sub loadCategorias()
        CblCategorias.DataSource = Nothing
        Dim dao As New LyDataAcces.DAO.DaoCategoria
        Dim list As List(Of LyBussinesModel.Categoria) = dao.GetAllCategorias
        If list.Count > 0 Then
            CblCategorias.DataSource = list
            CblCategorias.ValueMember = "Id"
            CblCategorias.DisplayMember = "Nombre"
        End If
    End Sub

    ''' <summary>
    ''' loadMarkedCategorias.
    ''' </summary>
    Private Sub loadMarkedCategorias()
        Dim cats = Main.UsuarioAutenticado.Categorias
        Dim counter = 0
        Dim listaIndices = New List(Of Integer)
        For Each lista In CblCategorias.Items
            For Each cat In cats
                If lista.Id = cat.Id Then
                    listaIndices.Add(counter)
                End If
            Next
            counter = counter + 1
        Next
        For Each num In listaIndices
            CblCategorias.SetItemChecked(num, True)
        Next
    End Sub

    ''' <summary>
    ''' FrmMisAnuncios_VisibleChanged.
    ''' </summary>
    Sub FrmMisAnuncios_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Me.Visible = True Then
            Main.AbribVentana(Me)
            CargarDatosPanelPpal()
            loadCategorias()
        End If

    End Sub

    ''' <summary>
    ''' Volver_Disposed.
    ''' </summary>
    Private Sub Volver_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Main.VolverAlMenu(Me)
    End Sub

    ''' <summary>
    ''' BtnEditarUser_Click.
    ''' </summary>
    Private Sub BtnEditarUser_Click(sender As Object, e As EventArgs) Handles BtnEditarUser.Click
        TxtNombre.ReadOnly = False
        TxtApellidos.ReadOnly = False
        TxtTelefono.ReadOnly = False
        TxtEmail.ReadOnly = False
        TxtPassword.ReadOnly = False
        TxtPasswordConfirm.ReadOnly = False
        BtnSaveChanges.Visible = True
        BtnCancelEdit.Visible = True
        BtnEditarUser.Visible = False
        BtnSaveChanges.Enabled = True
    End Sub

    ''' <summary>
    ''' BtnCancelEdit_Click.
    ''' </summary>
    Private Sub BtnCancelEdit_Click(sender As Object, e As EventArgs) Handles BtnCancelEdit.Click
        TxtNombre.ReadOnly = True
        TxtApellidos.ReadOnly = True
        TxtTelefono.ReadOnly = True
        TxtEmail.ReadOnly = True
        TxtPassword.ReadOnly = True
        TxtPasswordConfirm.ReadOnly = True
        BtnSaveChanges.Visible = False
        BtnCancelEdit.Visible = False
        BtnEditarUser.Visible = True
        BtnSaveChanges.Enabled = False
    End Sub

    ''' <summary>
    ''' BtnMiCuenta_Click.
    ''' </summary>
    Private Sub BtnMiCuenta_Click(sender As Object, e As EventArgs) Handles BtnMiCuenta.Click
        CargarDatosPanelPpal()
    End Sub

    ''' <summary>
    ''' TxtUsuario_TextChanged.
    ''' </summary>
    Private Sub TxtUsuario_TextChanged(sender As Object, e As EventArgs) Handles TxtUsuario.TextChanged
        'Expresión regular, solo se permite texto.
        If Regex.IsMatch(TxtUsuario.Text, "^[a-zA-Z ]*$") Then
            BtnSaveChanges.Enabled = True
        Else
            BtnSaveChanges.Enabled = False
        End If
    End Sub

    ''' <summary>
    ''' TxtPassword_TextChanged.
    ''' </summary>
    Private Sub TxtPassword_TextChanged(sender As Object, e As EventArgs) Handles TxtPassword.TextChanged
        If TxtPassword.Text <> TxtPasswordConfirm.Text Then
            LblError.Text = "Las contraseñas no coinciden"
            BtnSaveChanges.Enabled = False
        Else
            LblError.Text = ""
            BtnSaveChanges.Enabled = True
        End If

    End Sub

    ''' <summary>
    ''' BtnSaveChanges_Click.
    ''' </summary>
    Private Sub BtnSaveChanges_Click(sender As Object, e As EventArgs) Handles BtnSaveChanges.Click

        Dim nuevoUsuario
        Dim categoriasDao = New List(Of DTO.DTOCategoria)
        For Each Categoria In Main.UsuarioAutenticado.Categorias
            categoriasDao.Add(New DTO.DTOCategoria(Categoria.Id, Categoria.Nombre))
        Next Categoria

        If TxtPassword.Text = TxtPasswordConfirm.Text And TxtPassword.TextLength > 0 Then
            Dim userPass = Usuario.CreateHash(TxtUsuario.Text, TxtPassword.Text)
            nuevoUsuario = New DTO.DTOUsuario(
                Main.UsuarioAutenticado.Id,
                TxtUsuario.Text,
                TxtNombre.Text,
                TxtApellidos.Text,
                TxtTelefono.Text,
                TxtEmail.Text,
                userPass,
                categoriasDao
            )
        Else
            nuevoUsuario = New DTO.DTOUsuario(
                Main.UsuarioAutenticado.Id,
                TxtUsuario.Text,
                TxtNombre.Text,
                TxtApellidos.Text,
                TxtTelefono.Text,
                TxtEmail.Text,
                categoriasDao
            )
        End If
        Dim daoUsuario = New DAO.DaoUsuario()

        If (daoUsuario.ModificarUsuario(nuevoUsuario)) Then
            BtnCancelEdit_Click(sender, e)
            Main.updateUserOnEdit(nuevoUsuario.Id)
        Else
            LblError.Text = DirectCast(daoUsuario, DAO.IDao).Errores.Message
            MsgBox("Error producido: " + LblError.Text, MsgBoxStyle.Critical, "Error actualizando el usuario")
        End If


    End Sub

    ''' <summary>
    ''' BtnCategorias_Click.
    ''' </summary>
    Private Sub BtnCategorias_Click(sender As Object, e As EventArgs) Handles BtnCategorias.Click
        PnlCategorias.Visible = True
        PnlCategorias.Enabled = True
        PnlMisDatos.Visible = False
        PnlMisDatos.Enabled = False
        loadCategorias()
        loadMarkedCategorias()
    End Sub

    ''' <summary>
    ''' BtnSaveCategorias_Click.
    ''' </summary>
    Private Sub BtnSaveCategorias_Click(sender As Object, e As EventArgs) Handles BtnSaveCategorias.Click

        Dim dto As New DTOCategoria()
        Dim dao = New DAO.DaoUsuario()
        Dim nuevoUsuario = New DTO.DTOUsuario(
                Main.UsuarioAutenticado.Id,
                Main.UsuarioAutenticado.NombreUsuario,
                Main.UsuarioAutenticado.Nombre,
                Main.UsuarioAutenticado.Apellidos,
                Main.UsuarioAutenticado.Telefono,
                Main.UsuarioAutenticado.Correo,
                ReadCategorias()
        )
        Main.UsuarioAutenticado.Categorias = dto.ListConverter(ReadCategorias())


        If (dao.ModificarUsuario(nuevoUsuario)) Then
            CargarDatosPanelPpal()
            Main.updateUserOnEdit(nuevoUsuario.id)
        Else
            LblError.Text = DirectCast(dao, DAO.IDao).Errores.Message
            MsgBox("Error producido: " + LblError.Text, MsgBoxStyle.Critical, "Error actualizando las Categorias del usuario")
        End If

    End Sub

    ''' <summary>
    ''' BtnMiCartera_Click.
    ''' </summary>
    Private Sub BtnMiCartera_Click(sender As Object, e As EventArgs) Handles BtnMiCartera.Click
        PnlCategorias.Visible = False
        PnlCategorias.Enabled = False
        PnlMisDatos.Visible = False
        PnlMisDatos.Enabled = False
        PnlMiCartera.Visible = True
        PnlMiCartera.Enabled = True
    End Sub

    ''' <summary>
    ''' BtnVolver_Click.
    ''' </summary>
    Private Sub BtnVolver_Click(sender As Object, e As EventArgs) Handles BtnVolver.Click
        Dispose()
    End Sub


End Class