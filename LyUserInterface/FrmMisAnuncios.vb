Imports LyBussinesModel.DTO

''' <summary>
''' FrmMisAnuncios class.
''' Formulario resumen actividad anuncios.
''' </summary>
Public Class FrmMisAnuncios

    Private _Servicio As LyBussinesModel.DTO.DTOServiciosDetalles
    Private _Editando As Boolean = False

    ''' <summary>
    ''' FrmMisAnuncios_VisibleChanged.
    ''' </summary>
    Sub FrmMisAnuncios_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        Main.AbribVentana(Me)
    End Sub

    ''' <summary>
    ''' IniciarVentana_Load.
    ''' </summary>
    Private Sub IniciarVentana_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BtnActivos_Click(sender, e)
    End Sub

    ''' <summary>
    ''' BtnActivos_Click.
    ''' </summary>
    Private Sub BtnActivos_Click(sender As Object, e As EventArgs) Handles BtnActivos.Click
        LblSubtitulo.Text = "Activos"
        UcListaAnuncios.TextoBotones = "Gestionar"
        Dim dao As New LyDataAcces.DAO.DaoServicios
        Dim listaServicios As List(Of LyBussinesModel.DTO.DTOServicios)
        listaServicios = dao.GetListServicios(Main.UsuarioAutenticado.Id, False)
        If listaServicios IsNot Nothing Then

            If listaServicios.Count > 0 Then
                UcListaAnuncios.ConstruirDatos(listaServicios)
            End If
        End If
    End Sub

    ''' <summary>
    ''' BtnFinalizados_Click.
    ''' </summary>
    Private Sub BtnFinalizados_Click(sender As Object, e As EventArgs) Handles BtnFinalizados.Click
        LblSubtitulo.Text = "Finalizados"
        UcListaAnuncios.TextoBotones = "Ver registros"
        Dim dao As New LyDataAcces.DAO.DaoServicios
        Dim listaServicios As List(Of LyBussinesModel.DTO.DTOServicios)
        listaServicios = dao.GetListServicios(Main.UsuarioAutenticado.Id, True)
        UcListaAnuncios.ConstruirDatos(listaServicios)
    End Sub

    ''' <summary>
    ''' BtnAddAnuncio_Click.
    ''' </summary>
    Private Sub BtnAddAnuncio_Click(sender As Object, e As EventArgs) Handles BtnAddAnuncio.Click
        FrmCrearAnuncio.ClearText()
        FrmCrearAnuncio.Show()
    End Sub

    ''' <summary>
    ''' UcListaAnuncios_OnClickButton.
    ''' </summary>
    Private Sub UcListaAnuncios_OnClickButton(id As Integer) Handles UcListaAnuncios.OnClickButton
        LoadServicioParaGestion(id)
    End Sub

    ''' <summary>
    ''' BtnVolver_Click.
    ''' </summary>
    Private Sub BtnVolver_Click(sender As Object, e As EventArgs) Handles BtnVolver.Click
        Dispose()
    End Sub

    ''' <summary>
    ''' Volver_Disposed.
    ''' </summary>
    Private Sub Volver_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Main.VolverAlMenu(Me)
    End Sub

    ''' <summary>
    ''' LoadServicioParaGestion.
    ''' </summary>
#Region "SubApartado Gestionar Anuncio"
    Public Sub LoadServicioParaGestion(idServicio As Integer)
        PrepararSoloVisualizacion()
        Dim Dao As New LyDataAcces.DAO.DaoServicios
        Dim listaCandidaturas As New List(Of DTOListadoCandidaturasEnServicio)

        _Servicio = Dao.GetServiciosDetalles(idServicio, Main.UsuarioAutenticado.Id)
        If _Servicio IsNot Nothing Then
            listaCandidaturas = Dao.GetListadoCandidaturas(_Servicio.id)
            DTGCandidaturas.DataSource = Nothing

            If listaCandidaturas IsNot Nothing Then
                DTGCandidaturas.DataSource = listaCandidaturas
                DTGCandidaturas.Columns.Item(0).Visible = False
                DTGCandidaturas.Columns.Item("Fecha_Inscripcion").HeaderText = "Fecha"
                DTGCandidaturas.Columns.Item("Horas_Solicitadas").HeaderText = "Horas Requeridas"
                DTGCandidaturas.Columns.Item("Horas_Ganadas").HeaderText = "Horas Realizadas"

            End If


            TxtTitulo.Text = _Servicio.Titulo
            RtxtDescripcion.Text = _Servicio.Descripcion

            If _Servicio.Finalizado Then
                LblTituloVentana.Text = "Visualización de anuncio finalizado"
                BtnFinalizar.Enabled = False
                BtnFinalizar.Visible = False
                BtnEditar.Enabled = False
                BtnEditar.Visible = False
                lblAccionesTitulo.Visible = False
            Else
                LblTituloVentana.Text = "GESTIONAR ANUNCIO"
                BtnFinalizar.Enabled = True
                BtnFinalizar.Visible = True
                BtnEditar.Enabled = True
                BtnEditar.Visible = True
                lblAccionesTitulo.Visible = True
            End If

            LoadCategorias()

            If _Servicio.Categorias.Count > 0 Then
                For Each categoria As DTOCategoria In _Servicio.Categorias

                    For i As Integer = 0 To ChkList.Items.Count - 1
                        Dim item = ChkList.Items(i)
                        Dim element = DirectCast(item, LyBussinesModel.Categoria)
                        If element.Id = categoria.idCategoria Then
                            ChkList.SetItemChecked(ChkList.Items.IndexOf(item), True)
                        End If
                    Next

                Next
            End If
            PnVerGestionarAnuncio.Show()
            PnVerGestionarAnuncio.BringToFront()
        Else
            Me.Close()
        End If
    End Sub

    ''' <summary>
    ''' BtnFinalizar_Click.
    ''' </summary>
    Private Sub BtnFinalizar_Click(sender As Object, e As EventArgs) Handles BtnFinalizar.Click
        Dim dao As LyDataAcces.DAO.DaoServicios = New LyDataAcces.DAO.DaoServicios()

        If dao.FinalizarServicio(_Servicio.id) Then
            LoadServicioParaGestion(_Servicio.id)
        Else
            MsgBox("Error producido:" + DirectCast(dao, LyDataAcces.DAO.IDao).Errores.Message)
        End If
    End Sub

    ''' <summary>
    ''' BtnSalir_Click.
    ''' </summary>
    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        PrepararSoloVisualizacion()
        PnVerGestionarAnuncio.Hide()
        If _Servicio.Finalizado Then
            BtnFinalizados_Click(sender, e)
        Else
            BtnActivos_Click(sender, e)
        End If
    End Sub

    ''' <summary>
    ''' BtnEditar_Click.
    ''' </summary>
    Private Sub BtnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If Not _Editando Then
            PrepararEdicion()
        Else
            PrepararSoloVisualizacion()
        End If

    End Sub

    ''' <summary>
    ''' PrepararEdicion.
    ''' </summary>
    Private Sub PrepararEdicion()
        _Editando = True
        BtnFinalizar.Visible = False
        BtnEditar.Text = "CANCELAR EDICIÓN"

        BtnGuardar.Visible = True
        BtnGuardar.Enabled = True

        TxtTitulo.Enabled = True
        RtxtDescripcion.Enabled = True
        ChkList.Enabled = True

        TxtTitulo.ReadOnly = False
        RtxtDescripcion.ReadOnly = False

    End Sub

    ''' <summary>
    ''' PrepararSoloVisualizacion.
    ''' </summary>
    Private Sub PrepararSoloVisualizacion()
        _Editando = False
        BtnFinalizar.Visible = True
        BtnEditar.Text = "EDICIÓN"

        BtnGuardar.Visible = False
        BtnGuardar.Enabled = False

        TxtTitulo.Enabled = False
        RtxtDescripcion.Enabled = False
        ChkList.Enabled = False

        TxtTitulo.ReadOnly = True
        RtxtDescripcion.ReadOnly = True
    End Sub

    ''' <summary>
    ''' BtnGuardar_Click.
    ''' </summary>
    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim dao As LyDataAcces.DAO.DaoServicios = New LyDataAcces.DAO.DaoServicios
        Dim datos As DTOServicios = New DTOServicios()

        datos.id = _Servicio.id
        datos.titulo = TxtTitulo.Text
        datos.descripcion = RtxtDescripcion.Text
        datos.Categorias = ReadCategorias()

        If dao.ModificarServicio(datos) Then
            LoadServicioParaGestion(datos.id)
            PrepararSoloVisualizacion()
        Else
            MsgBox("Se ha producido un error " + DirectCast(dao, LyDataAcces.DAO.IDao).Errores.Message)
        End If
    End Sub

    ''' <summary>
    ''' LoadCategorias.
    ''' </summary>
    Private Sub LoadCategorias()
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
    ''' ReadCategorias.
    ''' </summary>
    Private Function ReadCategorias() As List(Of LyBussinesModel.DTO.DTOCategoria)
        Dim retorno As New List(Of DTOCategoria)
        For Each cat In ChkList.CheckedItems
            Dim categoria = DirectCast(cat, LyBussinesModel.Categoria)
            retorno.Add(New DTOCategoria(categoria.Id, categoria.Nombre))
        Next
        Return retorno
    End Function


    ''' <summary>
    ''' DTGCandidaturas_CellContentClick.
    ''' </summary>
    Private Sub DTGCandidaturas_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DTGCandidaturas.CellClick
        If DTGCandidaturas.SelectedRows.Count > 0 Then
            Dim selectIndex As Integer = DTGCandidaturas.SelectedRows.Item(0).Index
            Dim dtoSel As DTOListadoCandidaturasEnServicio = DTGCandidaturas.SelectedRows.Item(0).DataBoundItem
            FrmGestionCandidaturas.OpenDialog(dtoSel.Id)
            LoadServicioParaGestion(_Servicio.id)
            DTGCandidaturas.Rows.Item(selectIndex).Selected = True
        End If
    End Sub





#End Region



End Class