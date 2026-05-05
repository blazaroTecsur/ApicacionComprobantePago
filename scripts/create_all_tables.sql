-- ============================================================
-- SCRIPT COMPLETO: Tablas e Inserts
-- Base de datos: ComprobantePago
-- Motor: MySQL 8.0+
-- Generado: 2026-05-05
-- ============================================================

-- ============================================================
-- TABLAS CATÁLOGO
-- ============================================================

CREATE TABLE IF NOT EXISTS rcoestadocomprobante (
    IdEstadoComprobante INT           NOT NULL AUTO_INCREMENT,
    Codigo              VARCHAR(20)   NOT NULL,
    Descripcion         VARCHAR(100)  NOT NULL,
    Activo              TINYINT(1)    NOT NULL DEFAULT 1,
    UsuarioReg          VARCHAR(50)   NOT NULL,
    FechaReg            DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct          VARCHAR(50)   NULL,
    FechaAct            DATETIME      NULL,
    PRIMARY KEY (IdEstadoComprobante)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS rcotipodocumento (
    IdTipoDocumento INT          NOT NULL AUTO_INCREMENT,
    Codigo          VARCHAR(5)   NOT NULL,
    Descripcion     VARCHAR(100) NOT NULL,
    Activo          TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME     NULL,
    PRIMARY KEY (IdTipoDocumento)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS rcotiposunat (
    IdTipoSunat INT          NOT NULL AUTO_INCREMENT,
    Codigo      VARCHAR(5)   NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    Activo      TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg  VARCHAR(50)  NOT NULL,
    FechaReg    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct  VARCHAR(50)  NULL,
    FechaAct    DATETIME     NULL,
    PRIMARY KEY (IdTipoSunat)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS rcomoneda (
    IdMoneda    INT          NOT NULL AUTO_INCREMENT,
    Codigo      VARCHAR(5)   NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    Simbolo     VARCHAR(5)   NULL,
    Activo      TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg  VARCHAR(50)  NOT NULL,
    FechaReg    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct  VARCHAR(50)  NULL,
    FechaAct    DATETIME     NULL,
    PRIMARY KEY (IdMoneda)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS rcolugarpago (
    IdLugarPago INT          NOT NULL AUTO_INCREMENT,
    Codigo      VARCHAR(5)   NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    Activo      TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg  VARCHAR(50)  NOT NULL,
    FechaReg    DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct  VARCHAR(50)  NULL,
    FechaAct    DATETIME     NULL,
    PRIMARY KEY (IdLugarPago)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS rcotipodetraccion (
    IdTipoDetraccion INT           NOT NULL AUTO_INCREMENT,
    Codigo           VARCHAR(5)    NOT NULL,
    Descripcion      VARCHAR(200)  NOT NULL,
    Porcentaje       DECIMAL(5,2)  NOT NULL DEFAULT 0,
    Activo           TINYINT(1)    NOT NULL DEFAULT 1,
    UsuarioReg       VARCHAR(50)   NOT NULL,
    FechaReg         DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct       VARCHAR(50)   NULL,
    FechaAct         DATETIME      NULL,
    PRIMARY KEY (IdTipoDetraccion)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tmacuentacontable (
    IdCuentaContable INT          NOT NULL AUTO_INCREMENT,
    Codigo           VARCHAR(20)  NOT NULL,
    Descripcion      VARCHAR(200) NOT NULL,
    Activo           TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg       VARCHAR(50)  NOT NULL,
    FechaReg         DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct       VARCHAR(50)  NULL,
    FechaAct         DATETIME     NULL,
    PRIMARY KEY (IdCuentaContable)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tmacodigounidad1 (
    IdCodigoUnidad1 INT          NOT NULL AUTO_INCREMENT,
    Codigo          VARCHAR(10)  NOT NULL,
    Descripcion     VARCHAR(200) NOT NULL,
    Empresa         VARCHAR(50)  NOT NULL,
    Activo          TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME     NULL,
    PRIMARY KEY (IdCodigoUnidad1)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tmacodigounidad3 (
    IdCodigoUnidad3 INT          NOT NULL AUTO_INCREMENT,
    Codigo          VARCHAR(10)  NOT NULL,
    Descripcion     VARCHAR(200) NOT NULL,
    Activo          TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME     NULL,
    PRIMARY KEY (IdCodigoUnidad3)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tmacodigounidad4 (
    IdCodigoUnidad4 INT          NOT NULL AUTO_INCREMENT,
    Codigo          VARCHAR(10)  NOT NULL,
    Descripcion     VARCHAR(200) NOT NULL,
    Activo          TINYINT(1)   NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME     NULL,
    PRIMARY KEY (IdCodigoUnidad4)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tmaempleado (
    IdEmpleado           INT          NOT NULL AUTO_INCREMENT,
    IdEmpleadoExternal   VARCHAR(20)  NOT NULL,
    Codigo               VARCHAR(20)  NOT NULL,
    NombreCompleto       VARCHAR(200) NOT NULL,
    Apellido             VARCHAR(150) NULL,
    Nombre               VARCHAR(150) NULL,
    Alias                VARCHAR(150) NULL,
    Cargo                VARCHAR(150) NULL,
    Departamento         VARCHAR(20)  NULL,
    Estado               VARCHAR(20)  NULL,
    Turno                VARCHAR(20)  NULL,
    Categoria            VARCHAR(20)  NULL,
    IdUsuario            VARCHAR(50)  NULL,
    FrecuenciaPago       VARCHAR(20)  NULL,
    TipoEmpleado         VARCHAR(50)  NULL,
    GeneraNomina         VARCHAR(50)  NULL,
    CuentaSueldo         VARCHAR(50)  NULL,
    PrimerNombre         VARCHAR(100) NULL,
    SegundoNombre        VARCHAR(100) NULL,
    PrimerApellido       VARCHAR(100) NULL,
    SegundoApellido      VARCHAR(100) NULL,
    Direccion1           VARCHAR(200) NULL,
    Direccion2           VARCHAR(200) NULL,
    Ciudad               VARCHAR(100) NULL,
    CodProvincia         VARCHAR(10)  NULL,
    CP                   VARCHAR(20)  NULL,
    Telefono             VARCHAR(50)  NULL,
    CorreoElect          VARCHAR(200) NULL,
    FechaContratacion    DATETIME     NULL,
    FechaRescision       DATETIME     NULL,
    FechaCreacion        DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaActualizacion   DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (IdEmpleado),
    UNIQUE KEY UQ_tmaempleado_IdEmpleadoExternal (IdEmpleadoExternal)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS tmaproveedor (
    IdProveedor              INT          NOT NULL AUTO_INCREMENT,
    IdProveedorExternal      INT          NOT NULL,
    NombreProveedor          VARCHAR(255) NULL,
    TipoPersona              VARCHAR(20)  NOT NULL,
    Direccion1               VARCHAR(255) NULL,
    Direccion2               VARCHAR(255) NULL,
    Direccion3               VARCHAR(255) NULL,
    Direccion4               VARCHAR(255) NULL,
    Comprador                VARCHAR(150) NULL,
    Estado                   VARCHAR(20)  NOT NULL,
    Contacto                 VARCHAR(250) NULL,
    TelefonoContacto         VARCHAR(20)  NULL,
    CorreoExternoContacto    VARCHAR(100) NULL,
    CorreoInternoContacto    VARCHAR(100) NULL,
    Ruc                      VARCHAR(20)  NOT NULL,
    UsuarioReg               VARCHAR(30)  NULL,
    FechaReg                 DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct               VARCHAR(30)  NULL,
    FechaAct                 DATETIME     NULL,
    PRIMARY KEY (IdProveedor),
    UNIQUE KEY UQ_tmaproveedor_IdProveedorExternal (IdProveedorExternal)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================
-- TABLAS TRANSACCIONALES
-- ============================================================

CREATE TABLE IF NOT EXISTS rcocomprobante (
    IdComprobante        INT           NOT NULL AUTO_INCREMENT,
    Folio                VARCHAR(20)   NOT NULL,
    RucReceptor          VARCHAR(11)   NOT NULL,
    RazonSocialReceptor  VARCHAR(200)  NOT NULL,
    TipoDocumento        VARCHAR(5)    NOT NULL,
    TipoSunat            VARCHAR(5)    NOT NULL,
    Serie                VARCHAR(10)   NOT NULL,
    Numero               VARCHAR(20)   NOT NULL,
    FechaEmision         DATETIME      NOT NULL,
    FechaRecepcion       DATETIME      NULL,
    Moneda               VARCHAR(5)    NOT NULL,
    TasaCambio           DECIMAL(10,4) NOT NULL DEFAULT 1,
    LugarPago            VARCHAR(50)   NULL,
    PlazoPago            VARCHAR(10)   NULL,
    FechaVencimiento     DATETIME      NULL,
    RucBeneficiario      VARCHAR(11)   NULL,
    RazonSocialBenef     VARCHAR(200)  NULL,
    Observacion          VARCHAR(500)  NULL,
    OrdenCompra          VARCHAR(50)   NULL,
    FactMultiple         TINYINT(1)    NOT NULL DEFAULT 0,
    TipoDocAsociado      VARCHAR(5)    NULL,
    SerieAsociado        VARCHAR(10)   NULL,
    NumeroAsociado       VARCHAR(20)   NULL,
    TieneDetraccion      TINYINT(1)    NOT NULL DEFAULT 0,
    TipoDetraccion       VARCHAR(10)   NULL,
    PorcentajeDetraccion DECIMAL(5,2)  NULL,
    MontoDetraccion      DECIMAL(18,2) NOT NULL DEFAULT 0,
    ConstanciaDeposito   VARCHAR(20)   NULL,
    FechaDeposito        DATETIME      NULL,
    MontoNeto            DECIMAL(18,2) NOT NULL DEFAULT 0,
    MontoExento          DECIMAL(18,2) NOT NULL DEFAULT 0,
    PorcentajeIGV        DECIMAL(5,2)  NOT NULL DEFAULT 0,
    MontoIGVCosto        DECIMAL(18,2) NOT NULL DEFAULT 0,
    MontoIGVCredito      DECIMAL(18,2) NOT NULL DEFAULT 0,
    MontoTotal           DECIMAL(18,2) NOT NULL DEFAULT 0,
    MontoBruto           DECIMAL(18,2) NOT NULL DEFAULT 0,
    MontoRetencion       DECIMAL(18,2) NOT NULL DEFAULT 0,
    MontoMultas          DECIMAL(18,2) NOT NULL DEFAULT 0,
    ValorAduana          DECIMAL(18,2) NOT NULL DEFAULT 0,
    MontoRedondeo        DECIMAL(18,2) NOT NULL DEFAULT 0,
    EsDocumentoElectronico TINYINT(1)  NOT NULL DEFAULT 1,
    AplicaIGV            VARCHAR(1)    NOT NULL DEFAULT 'N',
    RequiereDetraccion   VARCHAR(1)    NOT NULL DEFAULT 'N',
    RequiereAduana       VARCHAR(1)    NOT NULL DEFAULT 'N',
    TienePdf             TINYINT(1)    NOT NULL DEFAULT 0,
    EdicionManual        TINYINT(1)    NOT NULL DEFAULT 0,
    EstaDerivado         TINYINT(1)    NOT NULL DEFAULT 0,
    CodigoEstado         VARCHAR(20)   NOT NULL DEFAULT 'NUEVO',
    EstadoSunat          VARCHAR(20)   NULL,
    RolDigitacion        VARCHAR(200)  NULL,
    FechaDigitacion      DATETIME      NULL,
    RolAutorizacion      VARCHAR(200)  NULL,
    FechaAutorizacion    DATETIME      NULL,
    RolAprobacion        VARCHAR(200)  NULL,
    FechaAprobacion      DATETIME      NULL,
    RolAnulacion         VARCHAR(200)  NULL,
    FechaAnulacion       DATETIME      NULL,
    Mensaje              VARCHAR(500)  NULL,
    Origen               VARCHAR(10)   NULL,
    SPO                  VARCHAR(50)   NULL,
    UsuarioReg           VARCHAR(50)   NOT NULL,
    FechaReg             DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct           VARCHAR(50)   NULL,
    FechaAct             DATETIME      NULL,
    EsEmpleado           TINYINT(1)    NOT NULL DEFAULT 0,
    EmpleadoCodigo       VARCHAR(20)   NULL,
    EmpleadoNombre       VARCHAR(200)  NULL,
    VoucherSyteline      INT           NULL,
    PRIMARY KEY (IdComprobante),
    UNIQUE KEY UQ_rcocomprobante_Folio (Folio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS rcoimputacioncontable (
    IdImputacionContable INT           NOT NULL AUTO_INCREMENT,
    Folio                VARCHAR(20)   NOT NULL,
    Secuencia            INT           NOT NULL DEFAULT 0,
    AliasCuenta          VARCHAR(50)   NULL,
    CuentaContable       VARCHAR(20)   NULL,
    DescripcionCuenta    VARCHAR(200)  NULL,
    Monto                DECIMAL(18,2) NOT NULL DEFAULT 0,
    Descripcion          VARCHAR(500)  NULL,
    Proyecto             VARCHAR(20)   NULL,
    CodUnidad1Cuenta     VARCHAR(10)   NULL,
    CodUnidad3Cuenta     VARCHAR(10)   NULL,
    CodUnidad4Cuenta     VARCHAR(10)   NULL,
    UsuarioReg           VARCHAR(50)   NOT NULL,
    FechaReg             DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioAct           VARCHAR(50)   NULL,
    FechaAct             DATETIME      NULL,
    PRIMARY KEY (IdImputacionContable),
    CONSTRAINT FK_rcoimputacioncontable_Folio
        FOREIGN KEY (Folio) REFERENCES rcocomprobante (Folio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS rcodocumentoelectronico (
    IdDocumento   INT          NOT NULL AUTO_INCREMENT,
    Folio         VARCHAR(20)  NOT NULL,
    TipoArchivo   VARCHAR(10)  NOT NULL,
    SubTipo       VARCHAR(30)  NOT NULL DEFAULT '',
    NombreArchivo VARCHAR(255) NOT NULL,
    Contenido     LONGBLOB     NOT NULL,
    FechaReg      DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UsuarioReg    VARCHAR(50)  NOT NULL,
    PRIMARY KEY (IdDocumento),
    INDEX IX_rcodocumentoelectronico_Folio (Folio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ============================================================
-- INSERTS CATÁLOGO — rcoestadocomprobante
-- ============================================================

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'NUEVO', 'Nuevo', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'NUEVO');

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'REGISTRADO', 'Registrado', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'REGISTRADO');

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'ENVIADO', 'Enviado', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'ENVIADO');

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'AUTORIZADO', 'Autorizado', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'AUTORIZADO');

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'APROBADO', 'Aprobado', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'APROBADO');

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'ANULADO', 'Anulado', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'ANULADO');

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'DERIVADO', 'Derivado', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'DERIVADO');

INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'DERIVADO SYT', 'Derivado a Syteline', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'DERIVADO SYT');

-- ============================================================
-- INSERTS CATÁLOGO — rcotipodocumento
-- ============================================================

INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'FAC', 'Factura', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'FAC');

INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'NCR', 'Nota de Crédito', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'NCR');

INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'NDB', 'Nota de Débito', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'NDB');

INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'LIQ', 'Liquidación de Compra', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'LIQ');

INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT 'REC', 'Recibo por Honorarios', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'REC');

-- ============================================================
-- INSERTS CATÁLOGO — rcotiposunat
-- ============================================================

INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT '01', 'Factura', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '01');

INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT '07', 'Nota de Crédito', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '07');

INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT '08', 'Nota de Débito', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '08');

INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT '40', 'Recibo por Honorarios', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '40');

INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
SELECT '03', 'Boleta de Venta', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '03');

-- ============================================================
-- INSERTS CATÁLOGO — rcomoneda
-- ============================================================

INSERT INTO rcomoneda (Codigo, Descripcion, Simbolo, Activo, UsuarioReg, FechaReg)
SELECT 'PEN', 'Sol Peruano', 'S/.', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcomoneda WHERE Codigo = 'PEN');

INSERT INTO rcomoneda (Codigo, Descripcion, Simbolo, Activo, UsuarioReg, FechaReg)
SELECT 'USD', 'Dólar Americano', '$', 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcomoneda WHERE Codigo = 'USD');

-- ============================================================
-- INSERTS CATÁLOGO — rcotipodetraccion (SUNAT)
-- ============================================================

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '001', 'Azúcar y melaza de caña', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '001');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '003', 'Alcohol etílico', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '003');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '004', 'Recursos hidrobiológicos', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '004');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '005', 'Maíz amarillo duro', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '005');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '006', 'Algodón', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '006');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '007', 'Caña de azúcar', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '007');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '008', 'Madera', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '008');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '009', 'Arena y piedra', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '009');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '010', 'Residuos, subproductos, desechos, recortes y desperdicios', 15.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '010');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '011', 'Bienes gravados con el IGV por renuncia a la exoneración', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '011');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '012', 'Intermediación laboral y tercerización', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '012');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '014', 'Carnes y despojos comestibles', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '014');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '017', 'Harina, polvo y "pellets" de pescado', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '017');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '019', 'Arrendamiento de bienes', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '019');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '020', 'Mantenimiento y reparación de bienes muebles', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '020');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '021', 'Movimiento de carga', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '021');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '022', 'Otros servicios empresariales', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '022');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '023', 'Leche', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '023');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '024', 'Comisión mercantil', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '024');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '025', 'Fabricación de bienes por encargo', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '025');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '026', 'Servicio de transporte de personas', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '026');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '030', 'Contratos de construcción', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '030');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '031', 'Oro gravado con el IGV', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '031');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '032', 'Páprika y otros frutos de los géneros capsicum o pimienta', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '032');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '034', 'Minerales metálicos no auríferos', 10.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '034');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '035', 'Bienes exonerados del IGV', 1.50, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '035');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '036', 'Oro y demás minerales metálicos exonerados del IGV', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '036');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '039', 'Minerales no metálicos', 12.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '039');

INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
SELECT '040', 'Bien inmueble gravado con IGV', 4.00, 1, 'SYSTEM', NOW()
FROM DUAL WHERE NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '040');

-- ============================================================
-- ALTER TABLE (columnas agregadas después de creación inicial)
-- ============================================================

ALTER TABLE rcocomprobante ADD COLUMN IF NOT EXISTS VoucherSyteline  INT          NULL;
ALTER TABLE rcocomprobante ADD COLUMN IF NOT EXISTS EsEmpleado       TINYINT(1)   NOT NULL DEFAULT 0;
ALTER TABLE rcocomprobante ADD COLUMN IF NOT EXISTS EmpleadoCodigo   VARCHAR(20)  NULL;
ALTER TABLE rcocomprobante ADD COLUMN IF NOT EXISTS EmpleadoNombre   VARCHAR(200) NULL;
