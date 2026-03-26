// ============================================
// DETALLE.JS - Registro de Comprobantes
// ============================================

$(document).ready(function () {
    if ($('#tabsDetalle').length > 0) {
        inicializar();
        bindEventos();
    }
});

// ── Inicialización ────────────────────────────
function inicializar() {
    inicializarFechas();

    const folio = obtenerParametroUrl('folio');
    if (folio) {
        $('#hdnFolio').val(folio);
        $('#divCorreo').addClass('d-none');
        $('#Resultados').removeClass('d-none');
        cargarCombos();
        cargarComprobante(folio);
    } else {
        $('#divCorreo').removeClass('d-none');
        $('#Resultados').addClass('d-none');
        $('#btnLimpiarCorreo').removeClass('d-none');
        cargarCombos();
        bloquearCamposElectronico();
    }
}

// ── Inicializar fechas HTML5 ──────────────────
function inicializarFechas() {
    $('#txtFechaVencimiento').on('change', function () {
        const emision = $('#txtFechaEmision').val();
        if (emision && $(this).val()) {
            const fechaEmision = new Date(emision);
            const fechaVenc = new Date($(this).val());
            const diff = Math.round(
                (fechaVenc - fechaEmision) / (1000 * 60 * 60 * 24));
            if (diff > 0) $('#txtPlazoPago').val(diff);
        }
    });
}

// ── Cargar combos ─────────────────────────────
function cargarCombos() {
    cargarTipoDocumento();
    cargarTipoSunat();
    cargarMoneda();
    cargarLugarPago();
    cargarTipoDetraccion();
    cargarTipoDocumentoAsociado();
}

function cargarTipoDocumento() {
    CorporativoQuery.ajaxGet('/Comprobante/ObtenerTiposDocumento',
        function (data) {
            let options = '<option value="">-- Seleccione --</option>';
            data.forEach(t =>
                options += `<option value="${t.codigo}">${t.descripcion}</option>`);
            $('#ddlTipoDocumento').html(options).prop('disabled', false);
        });
}

function cargarTipoSunat() {
    CorporativoQuery.ajaxGet('/Comprobante/ObtenerTiposSunat',
        function (data) {
            let options = '<option value="">-- Seleccione --</option>';
            data.forEach(t =>
                options += `<option value="${t.codigo}">${t.descripcion}</option>`);
            $('#ddlTipoSunat').html(options).prop('disabled', false);
        });
}

function cargarMoneda() {
    CorporativoQuery.ajaxGet('/Comprobante/ObtenerMonedas',
        function (data) {
            let options = '<option value="">-- Seleccione --</option>';
            data.forEach(m =>
                options += `<option value="${m.codigo}">${m.descripcion}</option>`);
            $('#ddlMoneda').html(options).prop('disabled', false);
        });
}

function cargarLugarPago() {
    CorporativoQuery.ajaxGet('/Comprobante/ObtenerLugaresPago',
        function (data) {
            let options = '<option value="">-- Seleccione --</option>';
            data.forEach(l =>
                options += `<option value="${l.codigo}">${l.descripcion}</option>`);
            $('#dldLugarPago').html(options).prop('disabled', false);
        });
}

function cargarTipoDetraccion() {
    CorporativoQuery.ajaxGet('/Comprobante/ObtenerTiposDetraccion',
        function (data) {
            const combo = $('#ddlTipoDetraccion');
            combo.empty().append(
                $('<option>', { value: '', text: '-- Seleccione --' }));
            data.forEach(function (item) {
                combo.append($('<option>', {
                    value: item.codigo,
                    text: item.descripcion,
                    'data-porcentaje': item.porcentaje
                }));
            });
        });
}

function cargarTipoDocumentoAsociado() {
    CorporativoQuery.ajaxGet('/Comprobante/ObtenerTiposDocumento',
        function (data) {
            let options = '<option value="">-- Seleccione --</option>';
            data.forEach(t =>
                options += `<option value="${t.codigo}">${t.descripcion}</option>`);
            $('#ddlTipoDocumentoAsociado').html(options);
        });
}

// ── Cargar comprobante existente ──────────────
function cargarComprobante(folio) {
    CorporativoQuery.ajaxGet(
        `/Comprobante/ObtenerDetalle?folio=${folio}`,
        function (data) {
            if (!data) {
                CorporativoCore.notificarError(
                    'No se encontró el comprobante.');
                return;
            }
            // Esperar combos y luego poblar
            setTimeout(() => {
                poblarCabecera(data);
                mostrarBotonesSegunEstado(data.codigoEstado);
                // Si es electrónico bloquear campos
                if (data.esDocumentoElectronico === 'S') {
                    bloquearCamposElectronico();
                } else {
                    habilitarCamposManual();
                }
            }, 500);
        });
}

// ── Poblar campos de cabecera ─────────────────
function poblarCabecera(data) {
    $('#txtFolio').val(data.folio);
    $('#hdnFolio').val(data.folio);
    $('#txtNumeroDocumentoIdentidad').val(data.ruc);
    $('#txtRazonSocial').val(data.razonSocial);
    $('#txtSerie').val(data.serie);
    $('#txtNumero').val(data.numero);
    $('#txtFechaEmision').val(data.fechaEmision);
    $('#txtFechaRecepcion').val(data.fechaRecepcion);
    $('#txtTasaCambio').val(data.tasaCambio);
    $('#txtCR').val(data.centroResponsabilidad);
    $('#txtDesCR').val(data.descripcionCR);
    $('#txtObservacion').val(data.observacion);
    $('#txtPlazoPago').val(data.plazoPago);
    $('#txtRucBenef').val(data.rucBenef);
    $('#txtRazonSocialBenef').val(data.razonSocialBenef);
    $('#txtRolDigitacion').val(data.rolDigitacion);
    $('#txtRolAutorizacion').val(data.rolAutorizacion);
    $('#txtRolAprobacion').val(data.rolAprobacion);
    $('#txtFechaAprobacion').val(data.fechaAprobacion);
    $('#txtEstadoComprobante').val(data.estado);
    $('#txtRolAnulacion').val(data.rolAnulacion);
    $('#txtFechaAnulacion').val(data.fechaAnulacion);
    $('#txtMensaje').val(data.mensaje);

    // Hidden fields
    $('#hdnCodigoEstado').val(data.codigoEstado);
    $('#hdnEsDocumentoElectronico').val(data.esDocumentoElectronico);
    $('#hdnRequiereDetraccion').val(data.requiereDetraccion);
    $('#hdnAplicaIGV').val(data.aplicaIGV);
    $('#hdnOrigen').val(data.origen);

    // Combos — usar esperarComboYAsignar para garantizar carga
    esperarComboYAsignar('#ddlTipoDocumento', data.tipoDocumento);
    esperarComboYAsignar('#ddlTipoSunat', data.tipoSunat);
    esperarComboYAsignar('#ddlMoneda', data.moneda);
    esperarComboYAsignar('#dldLugarPago', data.lugarPago);

    // Montos
    poblarMontos(data);

    // Detracción
    if (data.tieneDetraccion) {
        $('#TieneDetraccion').prop('checked', true);
        $('#NoTieneDetraccion').prop('checked', false);
        habilitarCamposDetraccion(true);
        esperarComboYAsignar('#ddlTipoDetraccion', data.tipoDetraccion);
        $('#txtPorcentajeDetraccion').val(data.porcentajeDetraccion);
        $('#txtConstanciaDeposito').val(data.constanciaDeposito);
        $('#txtFechaDeposito').val(data.fechaDeposito);
    } else {
        $('#TieneDetraccion').prop('checked', false);
        $('#NoTieneDetraccion').prop('checked', true);
        habilitarCamposDetraccion(false);
    }

    // Fecha vencimiento
    if (data.plazoPago) {
        $('#panelFechaVencimiento').removeClass('d-none');
        $('#txtFechaVencimiento').val(data.fechaVencimiento);
    }

    // Aduana
    if (data.requiereAduana === 'S') {
        $('#panelCodAduana').removeClass('d-none');
        $('#panelAnoAduana').removeClass('d-none');
    }

    // Radio facturación
    if (data.esDocumentoElectronico === 'S') {
        $('#rdoFacturacionElectronica').prop('checked', true);
        $('#rdoFacturacionManual').prop('checked', false);
    } else {
        $('#rdoFacturacionManual').prop('checked', true);
        $('#rdoFacturacionElectronica').prop('checked', false);
    }

    // Habilitar campos post-folio
    habilitarCamposManuales();
}

// ── Poblar montos dinámicos ───────────────────
function poblarMontos(data) {
    if (data.montoNeto) {
        $('#MontoNeto').removeClass('d-none');
        $('#lblMontoNeto').text(data.lblMontoNeto || 'Valor Venta');
        $('#txtMontoNeto').val(
            CorporativoCore.formatearMonto(data.montoNeto));
    }
    if (data.montoExento) {
        $('#MontoExento').removeClass('d-none');
        $('#lblMontoExento').text(data.lblMontoExento || 'Monto Exento');
        $('#txtMontoExento').val(
            CorporativoCore.formatearMonto(data.montoExento));
    }
    if (data.montoIGVCosto) {
        $('#MontoIGVCosto').removeClass('d-none');
        $('#lblMontoIGVCosto').text(data.lblMontoIGVCosto || 'IGV Costo');
        $('#txtMontoIGVCosto').val(
            CorporativoCore.formatearMonto(data.montoIGVCosto));
    }
    if (data.montoIGVCredito) {
        $('#MontoIGVCredito').removeClass('d-none');
        $('#lblMontoIGVCredito').text(data.lblMontoIGVCredito || 'IGV');
        $('#txtMontoIGVCredito').val(
            CorporativoCore.formatearMonto(data.montoIGVCredito));
    }
    if (data.montoTotal) {
        $('#MontoTotal').removeClass('d-none');
        $('#lblMontoTotal').text(data.lblMontoTotal || 'Total');
        $('#txtMontoTotal').val(
            CorporativoCore.formatearMonto(data.montoTotal));
    }
    if (data.montoBruto) {
        $('#MontoBruto').removeClass('d-none');
        $('#lblMontoBruto').text(data.lblMontoBruto || 'Total a Pagar');
        $('#txtMontoBruto').val(
            CorporativoCore.formatearMonto(data.montoBruto));
    }
    if (data.montoRetencion) {
        $('#MontoRetencion').removeClass('d-none');
        $('#lblMontoRetencion').text(
            data.lblMontoRetencion || 'Retención');
        $('#txtMontoRetencion').val(
            CorporativoCore.formatearMonto(data.montoRetencion));
    }
}

// ── Mostrar botones según estado ──────────────
function mostrarBotonesSegunEstado(estado) {
    ocultarTodosLosBotones();
    $('#btnAtras').removeClass('d-none');

    switch (estado) {
        case 'NUEVO':
            mostrarBotonesNuevo();
            break;
        case 'REGISTRADO':
            $('#btnRegistrar, #btnEnviar, #btnLimpiar')
                .removeClass('d-none');
            $('#btnImprimirComprobante, #btnVistaPrevia')
                .removeClass('d-none');
            $('#btnAnular, #btnModoImputacion')
                .removeClass('d-none');
            break;
        case 'ENVIADO':
            $('#btnAutorizarDetalle, #btnImprimirComprobante')
                .removeClass('d-none');
            $('#btnVistaPrevia, #btnAnular').removeClass('d-none');
            break;
        case 'AUTORIZADO':
            $('#btnAprobarDetalle, #btnImprimirComprobante')
                .removeClass('d-none');
            $('#btnVistaPrevia, #btnAnular').removeClass('d-none');
            break;
        case 'APROBADO':
            $('#btnImprimirComprobante, #btnVistaPrevia')
                .removeClass('d-none');
            $('#btnAsociados, #btnOrigen, #btnDestino, #btnCtaBancaria')
                .removeClass('d-none');
            break;
        case 'ANULADO':
            $('#btnImprimirComprobante, #btnVistaPrevia')
                .removeClass('d-none');
            break;
    }
}

function mostrarBotonesNuevo() {
    ocultarTodosLosBotones();
    $('#btnLimpiar, #btnRegistrar, #btnAtras, #btnModoImputacion')
        .removeClass('d-none');
}

function ocultarTodosLosBotones() {
    $('#barraAcciones .btn').addClass('d-none');
}

// ── Guardar comprobante ───────────────────────
function guardarComprobante() {
    if (!validarCabecera()) return;

    CorporativoQuery.ajaxPost(
        '/Comprobante/Guardar',
        { comprobante: obtenerDatosCabecera() },
        function (response) {
            if (response.exito) {
                CorporativoCore.notificarExito(
                    'Comprobante guardado correctamente.');
                $('#txtFolio').val(response.folio);
                $('#hdnFolio').val(response.folio);
                mostrarBotonesSegunEstado('REGISTRADO');
            } else {
                CorporativoCore.notificarError(response.mensaje);
            }
        }
    );
}

// ── Obtener datos de cabecera ─────────────────
function obtenerDatosCabecera() {
    return {
        folio: $('#hdnFolio').val(),
        ruc: $('#txtNumeroDocumentoIdentidad').val(),
        razonSocial: $('#txtRazonSocial').val(),
        tipoDocumento: $('#ddlTipoDocumento').val(),
        tipoSunat: $('#ddlTipoSunat').val(),
        serie: $('#txtSerie').val(),
        numero: $('#txtNumero').val(),
        fechaEmision: $('#txtFechaEmision').val(),
        fechaRecepcion: $('#txtFechaRecepcion').val(),
        moneda: $('#ddlMoneda').val(),
        tasaCambio: parseFloat($('#txtTasaCambio').val()) || 1,
        centroResponsabilidad: $('#txtCR').val(),
        lugarPago: $('#dldLugarPago').val(),
        plazoPago: $('#txtPlazoPago').val(),
        fechaVencimiento: $('#txtFechaVencimiento').val(),
        rucBenef: $('#txtRucBenef').val(),
        observacion: $('#txtObservacion').val(),
        ordenCompra: $('#txtOrdenCompra').val(),
        factMultiple: $('#chkFactMultiple').is(':checked'),
        tieneDetraccion: $('#TieneDetraccion').is(':checked'),
        tipoDetraccion: $('#ddlTipoDetraccion').val(),
        porcentajeDetraccion: parseFloat(
            $('#txtPorcentajeDetraccion').val()
                .replace(',', '.')) || 0,
        montoDetraccion: parseFloat(
            $('#hdnMontoDetraccion').val()) || 0,
        constanciaDeposito: $('#txtConstanciaDeposito').val(),
        fechaDeposito: $('#txtFechaDeposito').val(),
        esDocumentoElectronico: $('#rdoFacturacionElectronica')
            .is(':checked') ? 'S' : 'N',
        aplicaIGV: $('#hdnAplicaIGV').val(),
        origen: $('#hdnOrigen').val()
    };
}

// ── Validaciones ──────────────────────────────
function validarCabecera() {
    const campos = [
        {
            selector: '#txtNumeroDocumentoIdentidad',
            msg: 'Debe ingresar el RUC del proveedor.'
        },
        {
            selector: '#ddlTipoDocumento',
            msg: 'Debe seleccionar el tipo de comprobante.'
        },
        {
            selector: '#txtSerie',
            msg: 'Debe ingresar la serie.'
        },
        {
            selector: '#txtNumero',
            msg: 'Debe ingresar el número.'
        },
        {
            selector: '#txtFechaEmision',
            msg: 'Debe ingresar la fecha de emisión.'
        },
        {
            selector: '#ddlMoneda',
            msg: 'Debe seleccionar la moneda.'
        },
    ];

    for (const campo of campos) {
        if (CorporativoCore.esVacio($(campo.selector).val())) {
            CorporativoCore.notificarAdvertencia(campo.msg);
            $(campo.selector).focus();
            return false;
        }
    }
    return true;
}

// ── Limpiar formulario ────────────────────────
function limpiarFormulario() {
    // Campos texto
    $('#txtFolio, #txtNumeroDocumentoIdentidad, #txtRazonSocial').val('');
    $('#ddlTipoDocumento, #ddlTipoSunat, #txtSerie, #txtNumero').val('');
    $('#txtFechaEmision, #txtFechaRecepcion, #ddlMoneda, #txtTasaCambio').val('');
    $('#txtCR, #txtDesCR, #dldLugarPago, #txtPlazoPago').val('');
    $('#txtFechaVencimiento, #txtRucBenef, #txtRazonSocialBenef').val('');
    $('#txtObservacion, #txtOrdenCompra').val('');
    $('#chkFactMultiple').prop('checked', false);

    // Detracción
    $('#TieneDetraccion').prop('checked', false);
    $('#NoTieneDetraccion').prop('checked', true);
    $('#ddlTipoDetraccion, #txtPorcentajeDetraccion').val('');
    $('#txtConstanciaDeposito, #txtFechaDeposito').val('');
    $('#hdnMontoDetraccion').val('0');
    habilitarCamposDetraccion(false);

    // Montos
    $('#txtMontoNeto, #txtMontoExento, #txtMontoIGVCosto').val('');
    $('#txtMontoIGVCredito, #txtMontoTotal, #txtMontoBruto').val('');
    $('#txtMontoRetencion, #txtMontoMultas, #txtValorAduana').val('');

    // Estado
    $('#txtRolDigitacion, #txtRolAutorizacion, #txtRolAprobacion').val('');
    $('#txtFechaAprobacion, #txtEstadoComprobante').val('');
    $('#txtRolAnulacion, #txtFechaAnulacion, #txtMensaje').val('');

    // Hidden
    $('#hdnFolio, #hdnCodigoEstado').val('');

    // Paneles condicionales
    $('#panelFechaVencimiento, #panelCodAduana, #panelAnoAduana')
        .addClass('d-none');
    $('#divOrdenCompra, #divTipoOrden').addClass('d-none');

    // Montos dinámicos
    $('#MontoNeto, #MontoExento, #MontoIGVCosto, #MontoIGVCredito')
        .addClass('d-none');
    $('#MontoTotal, #MontoBruto, #MontoRetencion, #MontoMultas, #ValorAduana')
        .addClass('d-none');

    // Restaurar vista inicial
    $('#divCorreo').removeClass('d-none');
    $('#Resultados').addClass('d-none');
    $('#divResultadoSunat').addClass('d-none');
    $('#alertaSunat').html('');

    // Restaurar radio a electrónica
    $('#rdoFacturacionElectronica').prop('checked', true);
    $('#rdoFacturacionManual').prop('checked', false);

    // Bloquear campos electrónicos
    bloquearCamposElectronico();

    // Ocultar botones
    ocultarTodosLosBotones();
    $('#btnLimpiarCorreo').removeClass('d-none');
}

// ── Bloquear campos modo electrónico ──────────
function bloquearCamposElectronico() {
    $('#txtNumeroDocumentoIdentidad').prop('readonly', true);
    $('#txtRazonSocial').prop('readonly', true);
    $('#txtRucBenef').prop('readonly', true);
    $('#txtRazonSocialBenef').prop('readonly', true);
    $('#txtSerie').prop('readonly', true);
    $('#txtNumero').prop('readonly', true);
    $('#txtFechaEmision').prop('disabled', true);
    $('#ddlTipoSunat').prop('disabled', true);
    $('#btnBuscarProveedorPrincipal').prop('disabled', true);
    $('#btnBuscarRucBenef').prop('disabled', true);
    $('.monto').prop('readonly', true);
}

// ── Habilitar campos modo manual ──────────────
function habilitarCamposManual() {
    $('#txtNumeroDocumentoIdentidad').prop('readonly', false);
    $('#btnBuscarProveedorPrincipal').prop('disabled', false);
    $('#ddlTipoDocumento').prop('disabled', false);
    $('#ddlTipoSunat').prop('disabled', false);
    $('#txtSerie').prop('readonly', false);
    $('#txtNumero').prop('readonly', false);
    $('#txtFechaEmision').prop('disabled', false);
    $('#txtFechaRecepcion').prop('disabled', false);
    $('#ddlMoneda').prop('disabled', false);
    $('#txtTasaCambio').prop('readonly', false);
    $('#dldLugarPago').prop('disabled', false);
    $('#txtObservacion').prop('readonly', false);
    $('#txtRucBenef').prop('readonly', false);
    $('#txtRazonSocialBenef').prop('readonly', false);
    $('#btnBuscarRucBenef').prop('disabled', false);
    $('#TieneDetraccion').prop('disabled', false);
    $('#NoTieneDetraccion').prop('disabled', false);
    $('.monto').prop('readonly', false);
    habilitarCamposDetraccion($('#TieneDetraccion').is(':checked'));
}

// ── Activar modo manual ───────────────────────
function activarModoManual() {
    $('#divCorreo').addClass('d-none');
    $('#Resultados').removeClass('d-none');
    $('#hdnEsDocumentoElectronico').val('N');
    habilitarCamposManual();
    $('#MontoNeto, #MontoExento, #MontoIGVCosto, #MontoIGVCredito')
        .removeClass('d-none');
    $('#MontoTotal, #MontoBruto, #MontoRetencion').removeClass('d-none');
    $('#lblMontoNeto').text('Valor Venta');
    $('#lblMontoIGVCredito').text('IGV');
    $('#lblMontoTotal').text('Total');
    $('#lblMontoBruto').text('Total a Pagar');
    $('#lblMontoRetencion').text('Retención');
    mostrarBotonesNuevo();
}

// ── Habilitar campos manuales post-folio ──────
function habilitarCamposManuales() {
    $('#txtFechaRecepcion').prop('disabled', false);
    $('#txtCR').prop('readonly', false);
    $('#dldLugarPago').prop('disabled', false);
    $('#txtPlazoPago').prop('readonly', false);
    $('#ddlMoneda').prop('disabled', false);
    $('#txtTasaCambio').prop('readonly', false);
    $('#txtObservacion').prop('readonly', false);
    $('#btnBuscarProveedorPrincipal').prop('disabled', false);
    $('#btnBuscarCRPrincipal').prop('disabled', false);
}

// ── Habilitar/deshabilitar campos detracción ──
function habilitarCamposDetraccion(habilitar) {
    $('#ddlTipoDetraccion').prop('disabled', !habilitar);
    $('#txtPorcentajeDetraccion').prop('readonly', !habilitar);
    $('#txtConstanciaDeposito').prop('readonly', !habilitar);
    $('#txtFechaDeposito').prop('disabled', !habilitar);
}

// ── Calcular fecha vencimiento ────────────────
function calcularFechaVencimiento(fechaEmision, diasPlazo) {
    if (!fechaEmision || !diasPlazo) return '';
    const fecha = new Date(fechaEmision);
    if (isNaN(fecha)) return '';
    fecha.setDate(fecha.getDate() + diasPlazo);
    return fecha.toISOString().split('T')[0];
}

// ── Esperar combo y asignar valor ─────────────
function esperarComboYAsignar(selector, valor) {
    if (!valor) return;

    const intervalo = setInterval(function () {
        const combo = $(selector);
        if (combo.find('option').length > 1) {
            combo.val(valor);
            clearInterval(intervalo);
        }
    }, 100);

    setTimeout(function () {
        clearInterval(intervalo);
        const combo = $(selector);
        if (!combo.val() || combo.val() !== valor) {
            combo.val(valor);
        }
    }, 5000);
}

// ── Utilidades ────────────────────────────────
function obtenerParametroUrl(nombre) {
    return new URLSearchParams(window.location.search).get(nombre);
}

// ── Bind de eventos ───────────────────────────
function bindEventos() {

    // Guardar
    $('#btnRegistrar').on('click', guardarComprobante);

    // Radios facturación
    $('#rdoFacturacionManual').on('change', function () {
        if ($(this).is(':checked')) activarModoManual();
    });

    $('#rdoFacturacionElectronica').on('change', function () {
        if ($(this).is(':checked')) {
            CorporativoCore.confirmar(
                '¿Desea volver a Facturación Electrónica? ' +
                'Se limpiará el formulario.')
                .then(ok => {
                    if (ok) {
                        limpiarFormulario();
                    } else {
                        $('#rdoFacturacionManual').prop('checked', true);
                        $('#rdoFacturacionElectronica').prop('checked', false);
                    }
                });
        }
    });

    // Limpiar
    $('#btnLimpiar').on('click', async function () {
        const ok = await CorporativoCore.confirmar(
            '¿Desea limpiar el formulario?');
        if (ok) limpiarFormulario();
    });

    // Salir
    $('#btnAtras').on('click', async function () {
        const ok = await CorporativoCore.confirmar(
            '¿Desea salir sin guardar?');
        if (ok) window.location.href = '/Comprobante/Index';
    });

    // Anular
    $('#btnAnular').on('click', async function () {
        const ok = await CorporativoCore.confirmar(
            '¿Está seguro de anular este comprobante?');
        if (ok) anularComprobante();
    });

    // Enviar
    $('#btnEnviar').on('click', async function () {
        const ok = await CorporativoCore.confirmar(
            '¿Desea enviar el comprobante?');
        if (ok) enviarComprobante();
    });

    // Firmar / Autorizar
    $('#btnAutorizarDetalle').on('click', async function () {
        const ok = await CorporativoCore.confirmar(
            '¿Desea firmar el comprobante?');
        if (ok) firmarComprobante();
    });

    // Aprobar
    $('#btnAprobarDetalle').on('click', async function () {
        const ok = await CorporativoCore.confirmar(
            '¿Desea aprobar el comprobante?');
        if (ok) aprobarComprobante();
    });

    // Derivar
    $('#btnDerivar').on('click', async function () {
        const ok = await CorporativoCore.confirmar(
            '¿Desea derivar el comprobante?');
        if (ok) derivarComprobante();
    });

    // Plazo de pago → calcular vencimiento
    $('#txtPlazoPago').on('change', function () {
        const plazo = parseInt($(this).val());
        const fechaEmision = $('#txtFechaEmision').val();

        if (!plazo || plazo <= 0) {
            $('#panelFechaVencimiento').addClass('d-none');
            $('#txtFechaVencimiento').val('');
            return;
        }

        if (CorporativoCore.esVacio(fechaEmision)) {
            CorporativoCore.notificarAdvertencia(
                'Debe ingresar la fecha de emisión primero.');
            $(this).val('');
            return;
        }

        $('#txtFechaVencimiento').val(
            calcularFechaVencimiento(fechaEmision, plazo));
        $('#panelFechaVencimiento').removeClass('d-none');
    });

    // Fecha emisión cambia → recalcular vencimiento
    $('#txtFechaEmision').on('change', function () {
        const plazo = parseInt($('#txtPlazoPago').val());
        if (plazo > 0) {
            $('#txtFechaVencimiento').val(
                calcularFechaVencimiento($(this).val(), plazo));
            $('#panelFechaVencimiento').removeClass('d-none');
        }
    });

    // Detracción Si/No
    $('#TieneDetraccion').on('change', function () {
        if ($(this).is(':checked')) {
            $('#NoTieneDetraccion').prop('checked', false);
            habilitarCamposDetraccion(true);
        }
    });

    $('#NoTieneDetraccion').on('change', function () {
        if ($(this).is(':checked')) {
            $('#TieneDetraccion').prop('checked', false);
            $('#ddlTipoDetraccion').val('');
            $('#txtPorcentajeDetraccion').val('');
            $('#txtConstanciaDeposito').val('');
            $('#txtFechaDeposito').val('');
            habilitarCamposDetraccion(false);
        }
    });

    // Tipo detracción → porcentaje automático
    $('#ddlTipoDetraccion').on('change', function () {
        const porcentaje = $(this).find('option:selected')
            .data('porcentaje');
        if (porcentaje) {
            $('#txtPorcentajeDetraccion').val(
                CorporativoCore.formatearMonto(porcentaje));
            // Calcular monto detracción
            const total = parseFloat(
                $('#txtMontoTotal').val().replace(/,/g, '')) || 0;
            const monto = total * porcentaje / 100;
            $('#hdnMontoDetraccion').val(monto.toFixed(2));
        } else {
            $('#txtPorcentajeDetraccion').val('');
        }
    });

    // Validación SUNAT (xlsx)
    $('#btnValidacionSunat').on('click', function () {
        $('#inpFileValSunat').trigger('click');
    });

    $('#inpFileValSunat').on('change', function () {
        if (this.files.length > 0) subirArchivoSunat(this.files[0]);
    });

    // Imputación masiva
    $('#btnExplorar').on('click', function () {
        $('#inpFile').trigger('click');
    });

    $('#inpFile').on('change', function () {
        if (this.files.length > 0) subirImputacionMasiva(this.files[0]);
    });

    // Vista previa PDF
    $('#btnVistaPrevia').on('click', function () {
        const folio = $('#hdnFolio').val();
        if (folio) {
            $('a[href="#tabImpresion"]').tab('show');
            $('#pdfComprobante').attr('src',
                `/Comprobante/ObtenerPdf?folio=${folio}`);
        }
    });
}

// ── Acciones del flujo ────────────────────────
function _accionComprobante(url, estadoDestino, mensajeExito) {
    const folio = $('#hdnFolio').val();
    CorporativoQuery.ajaxPost(url,
        { comprobante: { folio } },
        function (response) {
            if (response.exito) {
                mostrarBotonesSegunEstado(estadoDestino);
                CorporativoCore.notificarExito(mensajeExito);
            } else {
                CorporativoCore.notificarError(response.mensaje);
            }
        }
    );
}

function enviarComprobante() {
    _accionComprobante('/Comprobante/Enviar', 'ENVIADO',
        'Comprobante enviado correctamente.');
}
function firmarComprobante() {
    _accionComprobante('/Comprobante/Firmar', 'AUTORIZADO',
        'Comprobante autorizado correctamente.');
}
function aprobarComprobante() {
    _accionComprobante('/Comprobante/Aprobar', 'APROBADO',
        'Comprobante aprobado correctamente.');
}
function anularComprobante() {
    _accionComprobante('/Comprobante/Anular', 'ANULADO',
        'Comprobante anulado correctamente.');
}
function derivarComprobante() {
    _accionComprobante('/Comprobante/Derivar', 'REGISTRADO',
        'Comprobante derivado correctamente.');
}

// ── Upload FormData ───────────────────────────
function _subirFormData(url, formData, mensajeExito, mensajeError) {
    CorporativoCore.showLoading();
    $.ajax({
        url,
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
                CorporativoCore.notificarExito(mensajeExito);
            } else {
                CorporativoCore.notificarError(response.mensaje);
            }
        },
        error: function (xhr) {
            CorporativoCore.hideLoading();
            CorporativoCore.handleError(xhr, {
                onCustom: function () {
                    CorporativoCore.notificarError(mensajeError);
                }
            });
        }
    });
}

function subirArchivoSunat(archivo) {
    const fd = new FormData();
    fd.append('file', archivo);
    _subirFormData('/Comprobante/ValidarSunat', fd,
        'Validación SUNAT completada.',
        'Error al procesar el archivo SUNAT.');
}

function subirImputacionMasiva(archivo) {
    const fd = new FormData();
    fd.append('file', archivo);
    _subirFormData('/Comprobante/CargarImputacionMasiva', fd,
        'Imputación cargada correctamente.',
        'Error al cargar el archivo de imputación.');
}