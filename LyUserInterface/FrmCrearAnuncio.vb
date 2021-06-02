

''' <summary>
''' FrmCrearAnuncio class.
''' Formulario de creación de anuncios.
''' </summary>
Public Class FrmCrearAnuncio
    Public Sub ClearText()
        TxtTitulo.Text = ""
        RtxtDescripcion.Text = ""
        loadCategorias()
    End Sub

    ''' <summary>
    ''' loadCategorias.
    ''' </summary>
    Private Sub loadCategorias()
        ChkList.DataSource = Nothing
        Dim dao As New LyDataAcces.DAO.DaoCategoria
        Dim list As List(Of LyBussinesModel.Categoria) = dao.GetAllCategorias
        If list.Count > 0 Then
            ChkList.DataSource = list
            ChkList.ValueMember = "Id"
            ChkList.DisplayMember = "Nombre"
        End If
    End Sub

    ''' <summary>
    ''' BtnCrear_Click.
    ''' </summary>
    Private Sub BtnCrear_Click(sender As Object, e As EventArgs) Handles BtnCrear.Click
        Dim dao As New LyDataAcces.DAO.DaoServicios
        Dim nuevoServicio As New LyBussinesModel.DTO.DTOServicios With {
            .idCreador = Main.UsuarioAutenticado.Id,
            .titulo = TxtTitulo.Text,
            .descripcion = RtxtDescripcion.Text,
            .Categorias = ReadCategorias()
        }

        If dao.RegistrarServicio(nuevoServicio) Then
            Me.Close()
        Else
            Dim errores As Exception = DirectCast(dao, LyDataAcces.DAO.IDao).Errores
            MsgBox("Error producido " + errores.Message)
        End If


    End Sub

    ''' <summary>
    ''' ReadCategorias.
    ''' </summary>
    Private Function ReadCategorias() As List(Of LyBussinesModel.DTO.DTOCategoria)
        Dim retorno As New List(Of LyBussinesModel.DTO.DTOCategoria)
        For Each cat In ChkList.CheckedItems
            Dim categoria = DirectCast(cat, LyBussinesModel.Categoria)
            retorno.Add(New LyBussinesModel.DTO.DTOCategoria(categoria.Id, categoria.Nombre))
        Next
        Return retorno
    End Function
End Class