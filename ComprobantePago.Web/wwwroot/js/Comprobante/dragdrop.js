// ============================================
// DRAGDROP.JS - Drag & Drop XML/PDF
// ============================================

$(document).ready(function () {
    inicializarDragDrop();
    bindEventosDragDrop();
});

// ── Inicializar ───────────────────────────────
function inicializarDragDrop() {
    const area = document.getElementById('areaDragDrop');
    if (!area) return;

    area.addEventListener('dragover', function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).addClass('drag_over');
    });

    area.addEventListener('dragleave', function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).removeClass('drag_over');
    });

    area.addEventListener('drop', function (e) {
        e.preventDefault();
        e.stopPropagation();
        $(this).removeClass('drag_over');

        const archivos = e.dataTransfer.files;
        if (archivos.length === 0) return;

        procesarArchivo(archivos[0]);
    });
}

// ── Procesar archivo ──────────────────────────
function procesarArchivo(archivo) {
    const extension = archivo.name.split('.').pop().toLowerCase();

    if (extension !== 'xml' && extension !== 'pdf') {
        CorporativoCore.notificarError('Solo se aceptan archivos XML o PDF.');
        return;
    }

    if (extension === 'xml') {
        validarXmlSunat(archivo);
    } else {
        validarPdfSunat(archivo);
    }
}

// ── Validar XML contra SUNAT ──────────────────
function validarXmlSunat(archivo) {
    const formData = new FormData();
    formData.append('archivo', archivo);

    CorporativoCore.showLoading();
    $.ajax({
        url: '/Comprobante/ValidarXmlSunat',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'RequestVerificationToken': CorporativoCore.obtenerToken()
        },
        success: function (response) {
            CorporativoCore.hideLoading();
            mostrarResultadoSunat(response, 'xml', archivo);
        },
        error: function (xhr) {
            CorporativoCore.hideLoading();
            CorporativoCore.handleError(xhr, {
                onCustom: function () {
                    CorporativoCore.notificarError(
                        'Error al validar el archivo XML.');
                }
            });
        }
    });
}

// ── Validar PDF contra SUNAT ──────────────────
function validarPdfSunat(archivo) {
    const formData = new FormData();
    formData.append('archivo', archivo);

    CorporativoCore.showLoading();
    $.ajax({
        url: '/Comprobante/ValidarPdfSunat',
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        headers: {
            'RequestVerificationToken': CorporativoCore.obtenerToken()
        },
        success: function (response) {
            CorporativoCore.hideLoading();

            if (!response.exito) {
                $('#divResultadoSunat').removeClass('d-none');
                $('#alertaSunat').html(`
                    <div class="alert alert-warning d-flex align-items-center gap-2">
                        <i class="bi bi-exclamation-triangle-fill fs-5"></i>
                        <div>
                            <strong>Validación PDF no disponible</strong>
                            <p class="mb-0 small">
                                ${response.motivo || response.mensaje ||
                    'Por favor use el archivo XML.'}
                            </p>
                        </div>
                    </div>`);
                return;
            }

            mostrarResultadoSunat(response, 'pdf', archivo);
        },
        error: function (xhr) {
            CorporativoCore.hideLoading();
            CorporativoCore.handleError(xhr, {
                onCustom: function () {
                    CorporativoCore.notificarError(
                        'Error al validar el archivo PDF.');
                }
            });
        }
    });
}

// ── Mostrar resultado validación SUNAT ────────
function mostrarResultadoSunat(response, tipo, archivo) {
    $('#divResultadoSunat').removeClass('d-none');

    const estado = response.estadoSunat;
    const codigo = response.codigoEstado;
    let alerta = '';

    switch (codigo) {
        case '1': // ACEPTADO
            alerta = `
                <div class="alert alert-success d-flex align-items-center gap-2">
                    <i class="bi bi-check-circle-fill fs-5"></i>
                    <div>
                        <strong>Comprobante ACEPTADO por SUNAT</strong>
                        <p class="mb-0 small">
                            El archivo ha sido validado correctamente.
                            Se poblarán los campos automáticamente.
                        </p>
                    </div>
                </div>`;
            if (tipo === 'xml' || tipo === 'pdf') {
                Swal.fire({
                    title: 'Comprobante validado',
                    html: `
                        Se registró el comprobante con folio:
                        <strong>${response.folio}</strong>.<br>
                        Este ya ha sido validado ante SUNAT.<br><br>
                        Favor de proceder a completar los datos y
                        <strong>ENVIAR</strong> el comprobante vía SyteLine.
                    `,
                    icon: 'info',
                    confirmButtonText: 'Aceptar',
                    confirmButtonColor: '#185FA5'
                }).then(() => {
                    $('#hdnFolio').val(response.folio);
                    $('#txtFolio').val(response.folio)
                        .addClass('folio-generado');
                    poblarCamposDesdeXml(response.datos);
                    mostrarVistaDetalle();
                });
            }
            break;

        case '3': // AUTORIZADO
            alerta = `
                <div class="alert alert-success d-flex align-items-center gap-2">
                    <i class="bi bi-check-circle-fill fs-5"></i>
                    <div>
                        <strong>Comprobante AUTORIZADO por imprenta</strong>
                        <p class="mb-0 small">
                            El comprobante tiene autorización de imprenta.
                            Se poblarán los campos automáticamente.
                        </p>
                    </div>
                </div>`;
            if (tipo === 'xml' || tipo === 'pdf') {
                Swal.fire({
                    title: 'Comprobante validado',
                    html: `
                        Se registró el comprobante con folio:
                        <strong>${response.folio}</strong>.<br>
                        Este ya ha sido validado ante SUNAT.<br><br>
                        Favor de proceder a completar los datos y
                        <strong>ENVIAR</strong> el comprobante vía SyteLine.
                    `,
                    icon: 'info',
                    confirmButtonText: 'Aceptar',
                    confirmButtonColor: '#185FA5'
                }).then(() => {
                    $('#hdnFolio').val(response.folio);
                    $('#txtFolio').val(response.folio)
                        .addClass('folio-generado');
                    poblarCamposDesdeXml(response.datos);
                    mostrarVistaDetalle();
                });
            }
            break;

        case '2': // ANULADO
            alerta = `
                <div class="alert alert-warning d-flex align-items-center gap-2">
                    <i class="bi bi-exclamation-triangle-fill fs-5"></i>
                    <div>
                        <strong>Comprobante ANULADO en SUNAT</strong>
                        <p class="mb-0 small">
                            Este comprobante fue comunicado en una baja.
                            No puede ser procesado.
                        </p>
                    </div>
                </div>`;
            break;

        case '4': // NO AUTORIZADO
            alerta = `
                <div class="alert alert-danger d-flex align-items-center gap-2">
                    <i class="bi bi-x-circle-fill fs-5"></i>
                    <div>
                        <strong>Comprobante NO AUTORIZADO por imprenta</strong>
                        <p class="mb-0 small">
                            Este comprobante no fue autorizado por imprenta.
                            No puede ser procesado.
                        </p>
                    </div>
                </div>`;
            break;

        case '0': // NO EXISTE
            alerta = `
                <div class="alert alert-secondary d-flex align-items-center gap-2">
                    <i class="bi bi-question-circle-fill fs-5"></i>
                    <div>
                        <strong>Comprobante NO EXISTE en SUNAT</strong>
                        <p class="mb-0 small">
                            Este comprobante no ha sido informado a SUNAT.
                            Verifique los datos del archivo.
                        </p>
                    </div>
                </div>`;
            break;

        default:
            alerta = `
                <div class="alert alert-secondary d-flex align-items-center gap-2">
                    <i class="bi bi-question-circle-fill fs-5"></i>
                    <div>
                        <strong>Estado desconocido: ${estado}</strong>
                        <p class="mb-0 small">${response.motivo || ''}</p>
                    </div>
                </div>`;
    }

    $('#alertaSunat').html(alerta);
}

// ── Poblar campos desde XML/PDF ───────────────
function poblarCamposDesdeXml(datos) {
    if (!datos) return;

    // ── Folio ─────────────────────────────────
    $('#txtFolio').val($('#hdnFolio').val());

    // ── Receptor ──────────────────────────────
    $('#txtNumeroDocumentoIdentidad').val(datos.ruc);
    $('#txtRazonSocial').val(datos.razonSocial);

    // ── RUC Beneficiario = mismo RUC ──────────
    $('#txtRucBenef').val(datos.ruc);
    $('#txtRazonSocialBenef').val(datos.razonSocial);

    // ── Comprobante ───────────────────────────
    $('#txtSerie').val(datos.serie);
    $('#txtNumero').val(datos.numero);
    poblarFecha('#txtFechaEmision', datos.fechaEmision);

    // ── Combos ────────────────────────────────
    esperarComboYAsignar('#ddlTipoDocumento', 'FP');
    esperarComboYAsignar('#ddlTipoSunat', datos.tipoSunat);
    esperarComboYAsignar('#ddlMoneda', datos.moneda);
    esperarComboYAsignar('#dldLugarPago', '04');

    // ── Montos ────────────────────────────────
    if (datos.montoTotal) {
        $('#MontoTotal').removeClass('d-none');
        $('#lblMontoTotal').text('Total');
        $('#txtMontoTotal').val(CorporativoCore.formatearMonto(datos.montoTotal));
    }
    if (datos.montoNeto) {
        $('#MontoNeto').removeClass('d-none');
        $('#lblMontoNeto').text('Valor Venta');
        $('#txtMontoNeto').val(CorporativoCore.formatearMonto(datos.montoNeto));
    }
    if (datos.montoIGV) {
        $('#MontoIGVCredito').removeClass('d-none');
        $('#lblMontoIGVCredito').text('IGV');
        $('#txtMontoIGVCredito').val(CorporativoCore.formatearMonto(datos.montoIGV));
    }

    // ── Plazo de pago ─────────────────────────
    if (datos.plazoPago) {
        $('#txtPlazoPago').val(datos.plazoPago);
        const fechaVenc = calcularFechaVencimiento(
            datos.fechaEmision, parseInt(datos.plazoPago));
        if (fechaVenc) {
            poblarFecha('#txtFechaVencimiento', fechaVenc);
            $('#panelFechaVencimiento').removeClass('d-none');
        }
    }

    // ── Hidden fields ─────────────────────────
    $('#hdnEstadoSunat').val('ACEPTADO');
    $('#hdnEsDocumentoElectronico').val('S');
    $('#rdoFacturacionElectronica').prop('checked', true);

    // ── Detracción ────────────────────────────
    if (datos.tieneDetraccion) {
        $('#TieneDetraccion').prop('checked', true);
        $('#NoTieneDetraccion').prop('checked', false);
        esperarComboYAsignar('#ddlTipoDetraccion', datos.codigoDetraccion);
        $('#txtPorcentajeDetraccion')
            .val(CorporativoCore.formatearMonto(datos.porcentajeDetraccion))
            .prop('readonly', false);
        $('#txtConstanciaDeposito').prop('readonly', false);
        $('#txtFechaDeposito').prop('disabled', false);
        const montoDetraccion = datos.montoDetraccion > 0
            ? datos.montoDetraccion
            : (datos.montoTotal * datos.porcentajeDetraccion / 100);
        $('#hdnMontoDetraccion').val(montoDetraccion.toFixed(2));
    } else {
        $('#TieneDetraccion').prop('checked', false);
        $('#NoTieneDetraccion').prop('checked', true);
        $('#ddlTipoDetraccion').prop('disabled', true);
        $('#txtPorcentajeDetraccion').prop('readonly', true);
        $('#txtConstanciaDeposito').prop('readonly', true);
        $('#txtFechaDeposito').prop('disabled', true);
    }

    // ── Bloquear campos electrónicos ──────────
    bloquearCamposElectronico(); // ← NUEVO

    CorporativoCore.notificarExito('Campos poblados correctamente.');
}

// ── Mostrar vista de detalle ──────────────────
function mostrarVistaDetalle() {
    $('#divCorreo').addClass('d-none');
    $('#Resultados').removeClass('d-none');
    mostrarBotonesNuevo();
    habilitarCamposManuales();
    $('#barraOpcionesImputacion').removeClass('d-none');
    $('#barraAccionesImputacion').removeClass('d-none');
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

// ── Bind eventos drag & drop ──────────────────
function bindEventosDragDrop() {

    $('#btnSeleccionarArchivo').on('click', function () {
        $('#inpArchivoComprobante').trigger('click');
    });

    $('#inpArchivoComprobante').on('change', function () {
        if (this.files.length > 0) {
            procesarArchivo(this.files[0]);
            $(this).val('');
        }
    });

    $('#btnLimpiarCorreo').on('click', function () {
        $('#divResultadoSunat').addClass('d-none');
        $('#alertaSunat').html('');
        $('#areaDragDrop').removeClass('drag_over');
        CorporativoCore.notificarInfo('Área limpiada.');
    });
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
            combo.append(
                `<option value="${valor}" selected>${valor}</option>`);
        }
    }, 5000);
}

// ── Poblar fecha con Flatpickr ────────────────
function poblarFecha(selector, valor) {
    if (!valor) return;
    // Convertir dd/MM/yyyy → yyyy-MM-dd para HTML5
    const partes = valor.split('/');
    if (partes.length === 3) {
        const fechaHtml5 = `${partes[2]}-${partes[1]}-${partes[0]}`;
        $(selector).val(fechaHtml5);
    } else {
        $(selector).val(valor);
    }
}