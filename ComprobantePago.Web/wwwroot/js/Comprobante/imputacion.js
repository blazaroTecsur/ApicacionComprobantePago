// ============================================
// IMPUTACION.JS - Imputación Contable
// ============================================

let tablaImputacion;
let modoEdicion = false;
let secuenciaEditando = null;
let listaImputaciones = [];

$(document).ready(function () {
    if ($('#tblImputacion').length > 0) {
        inicializarTablaImputacion();
        bindEventosImputacion();
    }
});

// ── Inicializar DataTable ─────────────────────
function inicializarTablaImputacion() {
    tablaImputacion = $('#tblImputacion').DataTable({
        language: { url: '/lib/datatables.net/i18n/es-ES.json' },
        paging: false,
        searching: false,
        info: false,
        ordering: false,
        columns: [
            { data: 'aliasCuenta' },
            { data: 'proyecto' },
            { data: 'codUnidad1Cuenta' },
            { data: 'codUnidad3Cuenta' },
            { data: 'codUnidad4Cuenta' },
            {
                data: 'monto',
                render: d => CorporativoCore.formatearMonto(d),
                className: 'text-end'
            },
            { data: 'descripcion' },
            { data: 'cuentaContable', visible: false },
            {
                data: 'secuencia',
                orderable: false,
                render: function (secuencia) {
                    return `
                        <button class="btn btn-sm btn-primary btn-editar-imp"
                                data-secuencia="${secuencia}">
                            <i class="bi bi-pencil"></i>
                        </button>
                        <button class="btn btn-sm btn-danger btn-eliminar-imp"
                                data-secuencia="${secuencia}">
                            <i class="bi bi-trash"></i>
                        </button>`;
                }
            }
        ]
    });
}

// ── Cargar imputaciones del comprobante ───────
function cargarImputaciones(folio) {
    CorporativoQuery.ajaxGet(`/Comprobante/ObtenerImputaciones?folio=${folio}`, function (data) {
        listaImputaciones = data;
        tablaImputacion.clear().rows.add(data).draw();
        calcularTotales();
    });
}

// ── Mostrar formulario nueva imputación ───────
function mostrarFormularioImputacion() {
    limpiarFormularioImputacion();
    modoEdicion = false;
    secuenciaEditando = null;
    $('#pnlDetalleImputacion').removeClass('d-none');
    $('#btnAgregarNuevaImputacion').removeClass('d-none');
    $('#btnCancelarDetalle').removeClass('d-none');
    $('#btnEliminarDetalle').addClass('d-none');
    $('#btnEditarDetalle').addClass('d-none');
    $('#txtAliasCuenta').focus();
}

// ── Mostrar formulario editar imputación ──────
function mostrarFormularioEditar(secuencia) {
    const imp = listaImputaciones.find(i => i.secuencia == secuencia);
    if (!imp) return;

    modoEdicion = true;
    secuenciaEditando = secuencia;

    $('#txtSecuencia').val(imp.secuencia);
    $('#txtAliasCuenta').val(imp.aliasCuenta);
    $('#txtCuentaContable').val(imp.cuentaContable);
    $('#txtDescripcionCuenta').val(imp.descripcionCuenta);
    $('#txtDescripcion').val(imp.descripcion);
    $('#txtMonto').val(CorporativoCore.formatearMonto(imp.monto));
    $('#txtProyecto').val(imp.proyecto);

    if (imp.cuentaContable) {
        mostrarCodigosUnidad('Cuenta');
        $('#txtCodUnidad1Cuenta').val(imp.codUnidad1Cuenta);
        $('#txtCodUnidad3Cuenta').val(imp.codUnidad3Cuenta);
        $('#txtCodUnidad4Cuenta').val(imp.codUnidad4Cuenta);
    }

    $('#pnlDetalleImputacion').removeClass('d-none');
    $('#btnAgregarNuevaImputacion').addClass('d-none');
    $('#btnEditarDetalle').removeClass('d-none');
    $('#btnEliminarDetalle').removeClass('d-none');
    $('#btnCancelarDetalle').removeClass('d-none');
}

// ── Obtener datos del formulario ──────────────
function obtenerDatosFormulario() {
    return {
        secuencia: $('#txtSecuencia').val(),
        folio: $('#hdnFolio').val(),
        aliasCuenta: $('#txtAliasCuenta').val(),
        cuentaContable: $('#txtCuentaContable').val(),
        descripcionCuenta: $('#txtDescripcionCuenta').val(),
        descripcion: $('#txtDescripcion').val(),
        monto: CorporativoCore.limpiarMonto($('#txtMonto').val()),
        proyecto: $('#txtProyecto').val(),
        codUnidad1Cuenta: $('#txtCodUnidad1Cuenta').val(),
        codUnidad3Cuenta: $('#txtCodUnidad3Cuenta').val(),
        codUnidad4Cuenta: $('#txtCodUnidad4Cuenta').val(),
    };
}

// ── Agregar imputación ────────────────────────
function agregarImputacion() {
    if (!validarFormularioImputacion()) return;

    const datos = obtenerDatosFormulario();

    CorporativoQuery.ajaxPost('/Comprobante/AgregarImputacion',
        { imputacion: datos },
        function (response) {
            if (response.exito) {
                listaImputaciones.push(response.imputacion);
                tablaImputacion.row.add(response.imputacion).draw();
                calcularTotales();
                limpiarFormularioImputacion();
                CorporativoCore.notificarExito('Imputación agregada correctamente.');
            } else {
                CorporativoCore.notificarError(response.mensaje);
            }
        }
    );
}

// ── Guardar edición imputación ────────────────
function guardarEdicionImputacion() {
    if (!validarFormularioImputacion()) return;

    const datos = obtenerDatosFormulario();

    CorporativoQuery.ajaxPost('/Comprobante/EditarImputacion',
        { imputacion: datos },
        function (response) {
            if (response.exito) {
                const idx = listaImputaciones
                    .findIndex(i => i.secuencia == secuenciaEditando);
                if (idx >= 0) listaImputaciones[idx] = response.imputacion;

                tablaImputacion.clear().rows.add(listaImputaciones).draw();
                calcularTotales();
                ocultarFormularioImputacion();
                CorporativoCore.notificarExito('Imputación actualizada correctamente.');
            } else {
                CorporativoCore.notificarError(response.mensaje);
            }
        }
    );
}

// ── Eliminar imputación ───────────────────────
async function eliminarImputacion(secuencia) {
    const ok = await CorporativoCore.confirmar('¿Desea eliminar esta imputación?');
    if (!ok) return;

    const folio = $('#hdnFolio').val();

    CorporativoQuery.ajaxPost('/Comprobante/EliminarImputacion',
        { folio, secuencia },
        function (response) {
            if (response.exito) {
                listaImputaciones = listaImputaciones
                    .filter(i => i.secuencia != secuencia);
                tablaImputacion.clear().rows.add(listaImputaciones).draw();
                calcularTotales();
                ocultarFormularioImputacion();
                CorporativoCore.notificarExito('Imputación eliminada correctamente.');
            } else {
                CorporativoCore.notificarError(response.mensaje);
            }
        }
    );
}

// ── Calcular totales ──────────────────────────
function calcularTotales() {
    const montoTotal = CorporativoCore.limpiarMonto($('#txtMontoTotal').val());

    const totalImputado = listaImputaciones.reduce((sum, imp) => {
        return sum + (parseFloat(imp.monto) || 0);
    }, 0);

    const diferencia = montoTotal - totalImputado;

    $('#txtPorImputar').val(CorporativoCore.formatearMonto(montoTotal));
    $('#txtTotalImputacion').val(CorporativoCore.formatearMonto(totalImputado));
    $('#txtDiferenciaImputacion').val(CorporativoCore.formatearMonto(diferencia));

    if (Math.abs(diferencia) > 0.01) {
        $('#txtDiferenciaImputacion')
            .addClass('text-danger fw-bold')
            .removeClass('text-success');
    } else {
        $('#txtDiferenciaImputacion')
            .addClass('text-success')
            .removeClass('text-danger fw-bold');
    }
}

// ── Validar formulario ────────────────────────
function validarFormularioImputacion() {
    if (CorporativoCore.esVacio($('#txtCuentaContable').val())) {
        CorporativoCore.notificarAdvertencia('Debe seleccionar una cuenta contable.');
        $('#txtAliasCuenta').focus();
        return false;
    }
    // La primera cuenta contable (cabecera) no requiere monto > 0
    const esPrimeraImputacion =
        (!modoEdicion && listaImputaciones.length === 0) ||
        (modoEdicion && secuenciaEditando == 1);
    if (!esPrimeraImputacion &&
        (CorporativoCore.esVacio($('#txtMonto').val()) ||
         CorporativoCore.limpiarMonto($('#txtMonto').val()) <= 0)) {
        CorporativoCore.notificarAdvertencia('El monto debe ser mayor a 0.');
        $('#txtMonto').focus();
        return false;
    }
    return true;
}

// ── Limpiar formulario imputación ─────────────
function limpiarFormularioImputacion() {
    $('#txtSecuencia').val('');
    $('#txtAliasCuenta, #txtCuentaContable, #txtDescripcionCuenta').val('');
    $('#txtDescripcion').val('');
    $('#txtMonto').val('0.00');
    $('#txtProyecto').val('');

    ocultarCodigosUnidad('Cuenta');
}

// ── Ocultar formulario ────────────────────────
function ocultarFormularioImputacion() {
    $('#pnlDetalleImputacion').addClass('d-none');
    $('#btnAgregarNuevaImputacion').addClass('d-none');
    $('#btnEditarDetalle').addClass('d-none');
    $('#btnEliminarDetalle').addClass('d-none');
    $('#btnCancelarDetalle').addClass('d-none');
    limpiarFormularioImputacion();
    modoEdicion = false;
    secuenciaEditando = null;
}

// ── Cargar imputación masiva ──────────────────
function procesarImputacionMasiva(archivo) {
    const formData = new FormData();
    formData.append('file', archivo);
    formData.append('folio', $('#hdnFolio').val());

    CorporativoCore.showLoading();
    $.ajax({
        url: '/Comprobante/CargarImputacionMasiva',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'RequestVerificationToken': CorporativoCore.obtenerToken()
        },
        success: function (response) {
            CorporativoCore.hideLoading();
            if (response.exito) {
                listaImputaciones = response.imputaciones;
                tablaImputacion.clear().rows.add(listaImputaciones).draw();
                calcularTotales();
                CorporativoCore.notificarExito('Imputación masiva cargada correctamente.');
            } else {
                CorporativoCore.notificarError(response.mensaje);
            }
        },
        error: function (xhr) {
            CorporativoCore.hideLoading();
            CorporativoCore.handleError(xhr, {
                onCustom: function () {
                    CorporativoCore.notificarError('Error al procesar el archivo.');
                }
            });
        }
    });
}

// ── Descargar plantilla ───────────────────────
function descargarPlantillaImputacion() {
    window.location.href = '/Comprobante/DescargarPlantillaImputacion';
}

// ════════════════════════════════════════════
// CÓDIGOS DE UNIDAD
// ════════════════════════════════════════════

function mostrarCodigosUnidad(campo) {
    $(`#divCodigosUnidad${campo}`).removeClass('d-none');
}

function ocultarCodigosUnidad(campo) {
    $(`#divCodigosUnidad${campo}`).addClass('d-none');
    $(`#txtCodUnidad1${campo}`).val('');
    $(`#txtCodUnidad3${campo}`).val('');
    $(`#txtCodUnidad4${campo}`).val('');
}

function buscarCuentaContable() {
    CorporativoQuery.ajaxGet('/Comprobante/ObtenerCuentasContables',
        function (data) {
            if (!data || data.length === 0) {
                CorporativoCore.notificarInfo('No hay cuentas contables disponibles.');
                return;
            }
            mostrarModalBusqueda(data, 'Seleccionar Cuenta Contable', 'seleccionar-cuenta');
        }
    );
}

function buscarCodigoUnidad(campo, unidad, inputId) {
    const codigoOrigen = $('#txtCuentaContable').val();
    CorporativoQuery.ajaxGet(
        `/Comprobante/ObtenerCodigosUnidad?campo=${campo}&unidad=${unidad}&codigo=${codigoOrigen}`,
        function (data) {
            if (!data || data.length === 0) {
                CorporativoCore.notificarInfo('No hay códigos de unidad disponibles.');
                return;
            }
            mostrarModalBusqueda(data, 'Seleccionar Código de Unidad', 'seleccionar-unidad', inputId);
        }
    );
}

function mostrarModalBusqueda(data, titulo, claseItem, inputId) {
    const renderItem = (d) => {
        const extra = inputId ? `data-input="${inputId}"` : '';
        return `<a class="list-group-item list-group-item-action ${claseItem}"
                   data-codigo="${d.codigo}" ${extra} href="#">
                    <span class="fw-bold">${d.codigo}</span>
                    <span class="text-muted ms-2">${d.descripcion}</span>
                </a>`;
    };

    const html = `
        <div class="modal fade" id="modalBusqueda" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header py-2" style="background-color:#5b74ad;">
                        <h6 class="modal-title text-white mb-0">
                            <i class="bi bi-search"></i> ${titulo}
                        </h6>
                        <button type="button" class="btn-close btn-close-white"
                                data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body pb-0 pt-2 px-2">
                        <input type="text" class="form-control form-control-sm mb-2"
                               id="txtFiltroBusqueda" placeholder="Filtrar..." autocomplete="off" />
                    </div>
                    <div class="modal-body p-0" style="max-height:360px; overflow-y:auto;">
                        <div class="list-group list-group-flush" id="listaBusqueda">
                            ${data.map(renderItem).join('')}
                        </div>
                    </div>
                </div>
            </div>
        </div>`;

    $('#modalBusqueda').remove();
    $('#modalContainer').append(html);

    $('#txtFiltroBusqueda').on('keyup', function () {
        const texto = $(this).val().toLowerCase();
        $('#listaBusqueda a').each(function () {
            const coincide = $(this).text().toLowerCase().includes(texto);
            $(this).toggleClass('d-none', !coincide);
        });
    });

    const modal = new bootstrap.Modal(document.getElementById('modalBusqueda'));
    modal.show();
    setTimeout(() => $('#txtFiltroBusqueda').focus(), 300);
}

// ════════════════════════════════════════════
// BIND EVENTOS
// ════════════════════════════════════════════

function bindEventosImputacion() {

    $('#btnAgregarDetalle').on('click', mostrarFormularioImputacion);
    $('#btnAgregarNuevaImputacion').on('click', agregarImputacion);
    $('#btnEditarDetalle').on('click', guardarEdicionImputacion);
    $('#btnCancelarDetalle').on('click', ocultarFormularioImputacion);

    $('#btnLimpiarImputacion').on('click', async function () {
        const ok = await CorporativoCore.confirmar('¿Desea limpiar el formulario de imputación?');
        if (ok) limpiarFormularioImputacion();
    });

    $('#tblImputacion').on('click', '.btn-editar-imp', function () {
        mostrarFormularioEditar($(this).data('secuencia'));
    });

    $('#tblImputacion').on('click', '.btn-eliminar-imp', function () {
        eliminarImputacion($(this).data('secuencia'));
    });

    $('#btnExplorar').on('click', function () {
        $('#inpFile').trigger('click');
    });

    $('#inpFile').on('change', function () {
        if (this.files.length > 0) {
            procesarImputacionMasiva(this.files[0]);
            $(this).val('');
        }
    });

    $('#btnDescargarPlantillaImputacion').on('click', descargarPlantillaImputacion);

    $('a[href="#tabImputacion"]').on('shown.bs.tab', function () {
        const folio = $('#hdnFolio').val();
        if (folio) {
            $('#barraOpcionesImputacion').removeClass('d-none');
            $('#barraAccionesImputacion').removeClass('d-none');
            cargarImputaciones(folio);
        }
    });

    // ── Códigos Unidad ────────────────────────

    $('#btnBuscarCuentaContable').on('click', buscarCuentaContable);

    $('#txtCuentaContable').on('change', function () {
        if (!CorporativoCore.esVacio($(this).val())) mostrarCodigosUnidad('Cuenta');
        else ocultarCodigosUnidad('Cuenta');
    });

    $('#btnBuscarCodUnidad1Cuenta').on('click', () => buscarCodigoUnidad('Cuenta', 1, '#txtCodUnidad1Cuenta'));
    $('#btnBuscarCodUnidad3Cuenta').on('click', () => buscarCodigoUnidad('Cuenta', 3, '#txtCodUnidad3Cuenta'));
    $('#btnBuscarCodUnidad4Cuenta').on('click', () => buscarCodigoUnidad('Cuenta', 4, '#txtCodUnidad4Cuenta'));

    $(document).on('click', '.seleccionar-cuenta', function (e) {
        e.preventDefault();
        const codigo = $(this).data('codigo');
        $('#txtAliasCuenta').val(codigo);
        $('#txtCuentaContable').val(codigo);
        const descripcion = $(this).find('.text-muted').text().trim();
        $('#txtDescripcionCuenta').val(descripcion);
        $('#txtCuentaContable').trigger('change');
        bootstrap.Modal.getInstance(document.getElementById('modalBusqueda')).hide();
    });

    $(document).on('click', '.seleccionar-unidad', function (e) {
        e.preventDefault();
        const codigo = $(this).data('codigo');
        const inputId = $(this).data('input');
        $(inputId).val(codigo);
        bootstrap.Modal.getInstance(document.getElementById('modalBusqueda')).hide();
    });
}
