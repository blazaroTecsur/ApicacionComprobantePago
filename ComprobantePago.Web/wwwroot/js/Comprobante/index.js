// ============================================
// INDEX.JS - Consulta de Comprobantes
// ============================================

let tablaComprobantes;

$(document).ready(function () {
    if ($('#index_tblComprobantes').length > 0) {
        inicializarTabla();
        cargarTipos();
        cargarEstados();
        bindEventos();
        buscar();
    }
});

// ── Inicializar DataTable ─────────────────────
function inicializarTabla() {
    if ($.fn.DataTable.isDataTable('#index_tblComprobantes')) {
        $('#index_tblComprobantes').DataTable().destroy();
    }

    tablaComprobantes = $('#index_tblComprobantes').DataTable({
        language: { url: '/lib/datatables.net/i18n/es-ES.json' },
        searching: false,
        ordering: true,
        columns: [
            // ── Checkbox ──────────────────────
            {
                data: null,
                orderable: false,
                width: '30px',
                render: function (data, type, row) {
                    return `
                        <input type="checkbox"
                               class="chkComprobante form-check-input"
                               data-folio="${row.folio}"
                               data-estado="${row.estado}" />`;
                }
            },
            { data: 'folio' },
            { data: 'tipoComprobante' },
            { data: 'serie' },
            { data: 'numero' },
            { data: 'proveedor' },
            { data: 'fecha' },
            { data: 'moneda' },
            {
                data: 'montoTotal',
                render: d => CorporativoCore.formatearMonto(d)
            },
            {
                data: 'estado',
                render: d => renderEstado(d)
            },
            {
                data: 'folio',
                orderable: false,
                width: '60px',
                render: function (folio) {
                    return `
                        <button class="btn btn-sm btn-primary btn-ver"
                                data-folio="${folio}"
                                title="Ver detalle">
                            <i class="bi bi-eye"></i>
                        </button>`;
                }
            }
        ]
    });
}

// ── Cargar combos ─────────────────────────────
function cargarTipos() {
    CorporativoQuery.ajaxGet(
        '/Comprobante/ObtenerTiposDocumento',
        function (data) {
            let options = '<option value="">-- Todos --</option>';
            data.forEach(t =>
                options += `<option value="${t.codigo}">
                    ${t.descripcion}</option>`);
            $('#index_cboTipo').html(options);
        });
}

function cargarEstados() {
    CorporativoQuery.ajaxGet(
        '/Comprobante/ObtenerEstados',
        function (data) {
            let options = '<option value="">-- Todos --</option>';
            data.forEach(e =>
                options += `<option value="${e.codigo}">
                    ${e.descripcion}</option>`);
            $('#index_cboEstado').html(options);
        });
}

// ── Buscar comprobantes ───────────────────────
function buscar() {
    const filtros = {
        tipo: $('#index_cboTipo').val(),
        estado: $('#index_cboEstado').val(),
        proveedor: $('#index_txtProveedor').val(),
        folio: $('#index_txtFolio').val()
    };

    CorporativoQuery.ajaxPost(
        '/Comprobante/Buscar',
        filtros,
        function (data) {
            tablaComprobantes.clear().rows.add(data).draw();
            // Resetear checkboxes al recargar
            $('#chkTodos').prop('checked', false);
            actualizarBotonesExportar();
        }
    );
}

// ── Render estado (badge) ─────────────────────
function renderEstado(estado) {
    const colores = {
        'REGISTRADO': 'secondary',
        'ENVIADO': 'primary',
        'AUTORIZADO': 'info',
        'APROBADO': 'success',
        'ANULADO': 'danger',
        'DERIVADO': 'warning'
    };
    const color = colores[estado] || 'secondary';
    return `<span class="badge bg-${color}">${estado}</span>`;
}

// ── Seleccionar / deseleccionar todos ─────────
function bindChkTodos() {
    $('#chkTodos').off('change').on('change', function () {
        const checked = $(this).is(':checked');
        $('.chkComprobante').prop('checked', checked);
        actualizarBotonesExportar();
    });
}

// ── Actualizar botones exportar ───────────────
function actualizarBotonesExportar() {
    const seleccionados = obtenerSeleccionados();
    const haySeleccion = seleccionados.length > 0;
    const soloAprobados = seleccionados.length > 0 &&
        seleccionados.every(s => s.estado === 'APROBADO');

    // Cabecera — solo aprobados
    $('#index_btnExportarCabecera')
        .prop('disabled', !soloAprobados)
        .toggleClass('btn-success', soloAprobados)
        .toggleClass('btn-secondary', !soloAprobados)
        .attr('title', !haySeleccion
            ? 'Seleccione comprobantes para exportar'
            : !soloAprobados
                ? 'Solo se exportan comprobantes APROBADOS'
                : `Exportar ${seleccionados.length} comprobante(s)`);

    // Imputación — solo aprobados
    $('#index_btnExportarImputacion')
        .prop('disabled', !soloAprobados)
        .toggleClass('btn-success', soloAprobados)
        .toggleClass('btn-secondary', !soloAprobados);

    // Contador seleccionados
    if (haySeleccion) {
        $('#lblSeleccionados')
            .text(`${seleccionados.length} seleccionado(s)`)
            .removeClass('d-none');
    } else {
        $('#lblSeleccionados').addClass('d-none');
    }
}

// ── Obtener folios seleccionados ──────────────
function obtenerSeleccionados() {
    const seleccionados = [];
    $('.chkComprobante:checked').each(function () {
        seleccionados.push({
            folio: $(this).data('folio'),
            estado: $(this).data('estado')
        });
    });
    return seleccionados;
}

// ── Exportar cabecera Syteline ────────────────
function exportarCabecera() {
    const seleccionados = obtenerSeleccionados();

    if (seleccionados.length === 0) {
        CorporativoCore.notificarAdvertencia(
            'Seleccione al menos un comprobante.');
        return;
    }

    const noAprobados = seleccionados.filter(
        s => s.estado !== 'APROBADO');
    if (noAprobados.length > 0) {
        CorporativoCore.notificarAdvertencia(
            'Solo se pueden exportar comprobantes APROBADOS. ' +
            `Hay ${noAprobados.length} comprobante(s) sin aprobar.`);
        return;
    }

    const folios = seleccionados.map(s => s.folio);
    enviarFormExportar('/Comprobante/ExportarCabeceraSyteline', folios);
}

// ── Exportar imputación Syteline ──────────────
function exportarImputacion() {
    const seleccionados = obtenerSeleccionados();

    if (seleccionados.length === 0) {
        CorporativoCore.notificarAdvertencia(
            'Seleccione al menos un comprobante.');
        return;
    }

    const noAprobados = seleccionados.filter(
        s => s.estado !== 'APROBADO');
    if (noAprobados.length > 0) {
        CorporativoCore.notificarAdvertencia(
            'Solo se pueden exportar comprobantes APROBADOS.');
        return;
    }

    const folios = seleccionados.map(s => s.folio);
    enviarFormExportar('/Comprobante/ExportarDistribucionSyteline', folios);
}

// ── Enviar form dinámico para descarga ────────
function enviarFormExportar(url, folios) {
    const token = $('input[name="__RequestVerificationToken"]')
        .first().val();

    const form = $('<form>', {
        method: 'POST',
        action: url
    });

    // Token antiforgery
    if (token) {
        form.append($('<input>', {
            type: 'hidden',
            name: '__RequestVerificationToken',
            value: token
        }));
    }

    // Folios seleccionados
    folios.forEach(folio => {
        form.append($('<input>', {
            type: 'hidden',
            name: 'folios',
            value: folio
        }));
    });

    $('body').append(form);
    form.submit();
    form.remove();
}

// ── Eventos ───────────────────────────────────
function bindEventos() {

    // Buscar
    $('#index_btnBuscar').on('click', buscar);

    // Buscar con Enter
    $('#index_txtFolio, #index_txtProveedor').on('keypress', function (e) {
        if (e.which === 13) buscar();
    });

    // Nuevo
    $('#index_btnNuevo').on('click', function () {
        window.location.href = '/Comprobante/Detalle';
    });

    // Ver detalle
    $('#index_tblComprobantes').on('click', '.btn-ver', function () {
        const folio = $(this).data('folio');
        window.location.href = `/Comprobante/Detalle?folio=${folio}`;
    });

    // Checkbox individual → actualizar botones
    $('#index_tblComprobantes').on(
        'change', '.chkComprobante', function () {
            const total = $('.chkComprobante').length;
            const seleccionados = $('.chkComprobante:checked').length;
            $('#chkTodos').prop(
                'checked', total === seleccionados && total > 0);
            actualizarBotonesExportar();
        }
    );

    // Checkbox seleccionar todos — re-bind después de draw
    tablaComprobantes.on('draw', function () {
        bindChkTodos();
        actualizarBotonesExportar();
    });

    // Exportar cabecera
    $('#index_btnExportarCabecera').on('click', exportarCabecera);

    // Exportar imputación
    $('#index_btnExportarImputacion').on('click', exportarImputacion);
}