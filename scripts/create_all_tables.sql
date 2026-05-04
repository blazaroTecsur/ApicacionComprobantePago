-- ============================================================
-- SCRIPT COMPLETO: Tablas e Inserts
-- Base de datos: erp_tecsur_pinterna
-- Motor: SQL Server
-- Generado: 2026-05-04
-- ============================================================

-- ============================================================
-- TABLAS CATÁLOGO
-- ============================================================

-- rcoestadocomprobante
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcoestadocomprobante') AND type = 'U')
CREATE TABLE rcoestadocomprobante (
    IdEstadoComprobante INT           IDENTITY(1,1) NOT NULL,
    Codigo              VARCHAR(20)   NOT NULL,
    Descripcion         VARCHAR(100)  NOT NULL,
    Activo              BIT           NOT NULL DEFAULT 1,
    UsuarioReg          VARCHAR(50)   NOT NULL,
    FechaReg            DATETIME2     NOT NULL DEFAULT GETDATE(),
    UsuarioAct          VARCHAR(50)   NULL,
    FechaAct            DATETIME2     NULL,
    CONSTRAINT PK_rcoestadocomprobante PRIMARY KEY (IdEstadoComprobante)
);

-- rcotipodocumento
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcotipodocumento') AND type = 'U')
CREATE TABLE rcotipodocumento (
    IdTipoDocumento INT          IDENTITY(1,1) NOT NULL,
    Codigo          VARCHAR(5)   NOT NULL,
    Descripcion     VARCHAR(100) NOT NULL,
    Activo          BIT          NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME2    NULL,
    CONSTRAINT PK_rcotipodocumento PRIMARY KEY (IdTipoDocumento)
);

-- rcotiposunat
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcotiposunat') AND type = 'U')
CREATE TABLE rcotiposunat (
    IdTipoSunat INT          IDENTITY(1,1) NOT NULL,
    Codigo      VARCHAR(5)   NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    Activo      BIT          NOT NULL DEFAULT 1,
    UsuarioReg  VARCHAR(50)  NOT NULL,
    FechaReg    DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct  VARCHAR(50)  NULL,
    FechaAct    DATETIME2    NULL,
    CONSTRAINT PK_rcotiposunat PRIMARY KEY (IdTipoSunat)
);

-- rcomoneda
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcomoneda') AND type = 'U')
CREATE TABLE rcomoneda (
    IdMoneda    INT          IDENTITY(1,1) NOT NULL,
    Codigo      VARCHAR(5)   NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    Simbolo     VARCHAR(5)   NULL,
    Activo      BIT          NOT NULL DEFAULT 1,
    UsuarioReg  VARCHAR(50)  NOT NULL,
    FechaReg    DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct  VARCHAR(50)  NULL,
    FechaAct    DATETIME2    NULL,
    CONSTRAINT PK_rcomoneda PRIMARY KEY (IdMoneda)
);

-- rcolugarpago
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcolugarpago') AND type = 'U')
CREATE TABLE rcolugarpago (
    IdLugarPago INT          IDENTITY(1,1) NOT NULL,
    Codigo      VARCHAR(5)   NOT NULL,
    Descripcion VARCHAR(100) NOT NULL,
    Activo      BIT          NOT NULL DEFAULT 1,
    UsuarioReg  VARCHAR(50)  NOT NULL,
    FechaReg    DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct  VARCHAR(50)  NULL,
    FechaAct    DATETIME2    NULL,
    CONSTRAINT PK_rcolugarpago PRIMARY KEY (IdLugarPago)
);

-- rcotipodetraccion
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcotipodetraccion') AND type = 'U')
CREATE TABLE rcotipodetraccion (
    IdTipoDetraccion INT           IDENTITY(1,1) NOT NULL,
    Codigo           VARCHAR(5)    NOT NULL,
    Descripcion      VARCHAR(200)  NOT NULL,
    Porcentaje       DECIMAL(5,2)  NOT NULL DEFAULT 0,
    Activo           BIT           NOT NULL DEFAULT 1,
    UsuarioReg       VARCHAR(50)   NOT NULL,
    FechaReg         DATETIME2     NOT NULL DEFAULT GETDATE(),
    UsuarioAct       VARCHAR(50)   NULL,
    FechaAct         DATETIME2     NULL,
    CONSTRAINT PK_rcotipodetraccion PRIMARY KEY (IdTipoDetraccion)
);

-- tmacuentacontable
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'tmacuentacontable') AND type = 'U')
CREATE TABLE tmacuentacontable (
    IdCuentaContable INT          IDENTITY(1,1) NOT NULL,
    Codigo           VARCHAR(20)  NOT NULL,
    Descripcion      VARCHAR(200) NOT NULL,
    Tipo             VARCHAR(20)  NOT NULL,
    Activo           BIT          NOT NULL DEFAULT 1,
    UsuarioReg       VARCHAR(50)  NOT NULL,
    FechaReg         DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct       VARCHAR(50)  NULL,
    FechaAct         DATETIME2    NULL,
    CONSTRAINT PK_tmacuentacontable PRIMARY KEY (IdCuentaContable)
);

-- tmacodigounidad1
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'tmacodigounidad1') AND type = 'U')
CREATE TABLE tmacodigounidad1 (
    IdCodigoUnidad1 INT          IDENTITY(1,1) NOT NULL,
    Codigo          VARCHAR(10)  NOT NULL,
    Descripcion     VARCHAR(200) NOT NULL,
    Empresa         VARCHAR(50)  NOT NULL,
    Activo          BIT          NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME2    NULL,
    CONSTRAINT PK_tmacodigounidad1 PRIMARY KEY (IdCodigoUnidad1)
);

-- tmacodigounidad3
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'tmacodigounidad3') AND type = 'U')
CREATE TABLE tmacodigounidad3 (
    IdCodigoUnidad3 INT          IDENTITY(1,1) NOT NULL,
    Codigo          VARCHAR(10)  NOT NULL,
    Descripcion     VARCHAR(200) NOT NULL,
    Activo          BIT          NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME2    NULL,
    CONSTRAINT PK_tmacodigounidad3 PRIMARY KEY (IdCodigoUnidad3)
);

-- tmacodigounidad4
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'tmacodigounidad4') AND type = 'U')
CREATE TABLE tmacodigounidad4 (
    IdCodigoUnidad4 INT          IDENTITY(1,1) NOT NULL,
    Codigo          VARCHAR(10)  NOT NULL,
    Descripcion     VARCHAR(200) NOT NULL,
    Activo          BIT          NOT NULL DEFAULT 1,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME2    NULL,
    CONSTRAINT PK_tmacodigounidad4 PRIMARY KEY (IdCodigoUnidad4)
);

-- tmaempleado
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'tmaempleado') AND type = 'U')
CREATE TABLE tmaempleado (
    IdEmpleado           INT          IDENTITY(1,1) NOT NULL,
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
    FechaContratacion    DATETIME2    NULL,
    FechaRescision       DATETIME2    NULL,
    UsuarioReg      VARCHAR(50)  NOT NULL,
    FechaReg        DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct      VARCHAR(50)  NULL,
    FechaAct        DATETIME2    NULL,
    CONSTRAINT PK_tmaempleado PRIMARY KEY (IdEmpleado),
    CONSTRAINT UQ_tmaempleado_IdEmpleadoExternal UNIQUE (IdEmpleadoExternal)
);

-- tmaproveedor
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'tmaproveedor') AND type = 'U')
CREATE TABLE tmaproveedor (
    IdProveedor              INT          IDENTITY(1,1) NOT NULL,
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
    FechaReg                 DATETIME2    NOT NULL DEFAULT GETDATE(),
    UsuarioAct               VARCHAR(30)  NULL,
    FechaAct                 DATETIME2    NULL,
    CONSTRAINT PK_tmaproveedor PRIMARY KEY (IdProveedor),
    CONSTRAINT UQ_tmaproveedor_IdProveedorExternal UNIQUE (IdProveedorExternal),
    CONSTRAINT UQ_tmaproveedor_Ruc UNIQUE (Ruc),
    CONSTRAINT UQ_tmaproveedor_NombreProveedor UNIQUE (NombreProveedor)
);

-- ============================================================
-- TABLAS TRANSACCIONALES
-- ============================================================

-- rcocomprobante
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcocomprobante') AND type = 'U')
CREATE TABLE rcocomprobante (
    IdComprobante        INT           IDENTITY(1,1) NOT NULL,
    Folio                VARCHAR(20)   NOT NULL,
    RucReceptor          VARCHAR(11)   NOT NULL,
    RazonSocialReceptor  VARCHAR(200)  NOT NULL,
    TipoDocumento        VARCHAR(5)    NOT NULL,
    TipoSunat            VARCHAR(5)    NOT NULL,
    Serie                VARCHAR(10)   NOT NULL,
    Numero               VARCHAR(20)   NOT NULL,
    FechaEmision         DATETIME2     NOT NULL,
    FechaRecepcion       DATETIME2     NULL,
    Moneda               VARCHAR(5)    NOT NULL,
    TasaCambio           DECIMAL(10,4) NOT NULL DEFAULT 1,
    LugarPago            VARCHAR(50)   NULL,
    PlazoPago            VARCHAR(10)   NULL,
    FechaVencimiento     DATETIME2     NULL,
    RucBeneficiario      VARCHAR(11)   NULL,
    RazonSocialBenef     VARCHAR(200)  NULL,
    Observacion          VARCHAR(500)  NULL,
    OrdenCompra          VARCHAR(50)   NULL,
    FactMultiple         BIT           NOT NULL DEFAULT 0,
    TipoDocAsociado      VARCHAR(5)    NULL,
    SerieAsociado        VARCHAR(10)   NULL,
    NumeroAsociado       VARCHAR(20)   NULL,
    TieneDetraccion      BIT           NOT NULL DEFAULT 0,
    TipoDetraccion       VARCHAR(10)   NULL,
    PorcentajeDetraccion DECIMAL(5,2)  NULL,
    MontoDetraccion      DECIMAL(18,2) NOT NULL DEFAULT 0,
    ConstanciaDeposito   VARCHAR(20)   NULL,
    FechaDeposito        DATETIME2     NULL,
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
    EsDocumentoElectronico BIT         NOT NULL DEFAULT 1,
    AplicaIGV            VARCHAR(1)    NOT NULL DEFAULT 'N',
    RequiereDetraccion   VARCHAR(1)    NOT NULL DEFAULT 'N',
    RequiereAduana       VARCHAR(1)    NOT NULL DEFAULT 'N',
    TienePdf             BIT           NOT NULL DEFAULT 0,
    EdicionManual        BIT           NOT NULL DEFAULT 0,
    EstaDerivado         BIT           NOT NULL DEFAULT 0,
    CodigoEstado         VARCHAR(20)   NOT NULL DEFAULT 'NUEVO',
    EstadoSunat          VARCHAR(20)   NULL,
    RolDigitacion        VARCHAR(200)  NULL,
    FechaDigitacion      DATETIME2     NULL,
    RolAutorizacion      VARCHAR(200)  NULL,
    FechaAutorizacion    DATETIME2     NULL,
    RolAprobacion        VARCHAR(200)  NULL,
    FechaAprobacion      DATETIME2     NULL,
    RolAnulacion         VARCHAR(200)  NULL,
    FechaAnulacion       DATETIME2     NULL,
    Mensaje              VARCHAR(500)  NULL,
    Origen               VARCHAR(10)   NULL,
    SPO                  VARCHAR(50)   NULL,
    UsuarioReg           VARCHAR(50)   NOT NULL,
    FechaReg             DATETIME2     NOT NULL DEFAULT GETDATE(),
    UsuarioAct           VARCHAR(50)   NULL,
    FechaAct             DATETIME2     NULL,
    EsEmpleado           BIT           NOT NULL DEFAULT 0,
    EmpleadoCodigo       VARCHAR(20)   NULL,
    EmpleadoNombre       VARCHAR(200)  NULL,
    VoucherSyteline      INT           NULL,
    CONSTRAINT PK_rcocomprobante PRIMARY KEY (IdComprobante),
    CONSTRAINT UQ_rcocomprobante_Folio UNIQUE (Folio)
);

-- rcoimputacioncontable
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcoimputacioncontable') AND type = 'U')
CREATE TABLE rcoimputacioncontable (
    IdImputacionContable INT           IDENTITY(1,1) NOT NULL,
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
    FechaReg             DATETIME2     NOT NULL DEFAULT GETDATE(),
    UsuarioAct           VARCHAR(50)   NULL,
    FechaAct             DATETIME2     NULL,
    CONSTRAINT PK_rcoimputacioncontable PRIMARY KEY (IdImputacionContable),
    CONSTRAINT FK_rcoimputacioncontable_Folio
        FOREIGN KEY (Folio) REFERENCES rcocomprobante (Folio)
);

-- rcodocumentoelectronico
IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'rcodocumentoelectronico') AND type = 'U')
CREATE TABLE rcodocumentoelectronico (
    IdDocumento   INT            IDENTITY(1,1) NOT NULL,
    Folio         VARCHAR(20)    NOT NULL,
    TipoArchivo   VARCHAR(10)    NOT NULL,
    SubTipo       VARCHAR(30)    NOT NULL DEFAULT '',
    NombreArchivo VARCHAR(255)   NOT NULL,
    Contenido     VARBINARY(MAX) NOT NULL,
    FechaReg      DATETIME2      NOT NULL DEFAULT GETDATE(),
    UsuarioReg    VARCHAR(50)    NOT NULL,
    UsuarioAct    VARCHAR(50)    NULL,
    FechaAct      DATETIME2      NULL,
    CONSTRAINT PK_rcodocumentoelectronico PRIMARY KEY (IdDocumento)
);

CREATE INDEX IX_rcodocumentoelectronico_Folio
    ON rcodocumentoelectronico (Folio);

-- ============================================================
-- INSERTS CATÁLOGO — rcoestadocomprobante
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'NUEVO')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('NUEVO', 'Nuevo', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'REGISTRADO')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('REGISTRADO', 'Registrado', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'ENVIADO')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('ENVIADO', 'Enviado', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'AUTORIZADO')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('AUTORIZADO', 'Autorizado', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'APROBADO')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('APROBADO', 'Aprobado', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'ANULADO')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('ANULADO', 'Anulado', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'DERIVADO')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('DERIVADO', 'Derivado', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcoestadocomprobante WHERE Codigo = 'DERIVADO SYT')
    INSERT INTO rcoestadocomprobante (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('DERIVADO SYT', 'Derivado a Syteline', 1, 'SYSTEM', GETDATE());

-- ============================================================
-- INSERTS CATÁLOGO — rcotipodocumento
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'FAC')
    INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('FP', 'Facturas Proveedor', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'NCR')
    INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('VC', 'Vale Caja', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'NDB')
    INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('PV', 'Provisional', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodocumento WHERE Codigo = 'LIQ')
    INSERT INTO rcotipodocumento (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('VT', 'Pago Trabajadores', 1, 'SYSTEM', GETDATE());

-- ============================================================
-- INSERTS CATÁLOGO — rcotiposunat
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '01')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('01', 'Factura', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '07')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('07', 'Nota de Crédito', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '08')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('08', 'Nota de Débito', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '40')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('40', 'Recibo por Honorarios', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '03')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('03', 'Boleta de Venta', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '04')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('04', 'Liquidación de Compra', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = 'R1')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('R1', 'Recibo por Honorarios', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = '14')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('14', 'Recibos Servicios Públicos', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotiposunat WHERE Codigo = 'VALES')
    INSERT INTO rcotiposunat (Codigo, Descripcion, Activo, UsuarioReg, FechaReg)
    VALUES ('VALES', 'Vales', 1, 'SYSTEM', GETDATE());

-- ============================================================
-- INSERTS CATÁLOGO — rcomoneda
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM rcomoneda WHERE Codigo = 'PEN')
    INSERT INTO rcomoneda (Codigo, Descripcion, Simbolo, Activo, UsuarioReg, FechaReg)
    VALUES ('PEN', 'Sol Peruano', 'S/.', 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcomoneda WHERE Codigo = 'USD')
    INSERT INTO rcomoneda (Codigo, Descripcion, Simbolo, Activo, UsuarioReg, FechaReg)
    VALUES ('USD', 'Dólar Americano', '$', 1, 'SYSTEM', GETDATE());

-- ============================================================
-- INSERTS CATÁLOGO — rcotipodetraccion (SUNAT)
-- ============================================================

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '001')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('001', 'Azúcar y melaza de caña', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '003')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('003', 'Alcohol etílico', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '004')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('004', 'Recursos hidrobiológicos', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '005')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('005', 'Maíz amarillo duro', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '006')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('006', 'Algodón', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '007')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('007', 'Caña de azúcar', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '008')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('008', 'Madera', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '009')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('009', 'Arena y piedra', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '010')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('010', 'Residuos, subproductos, desechos, recortes y desperdicios', 15.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '011')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('011', 'Bienes gravados con el IGV por renuncia a la exoneración', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '012')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('012', 'Intermediación laboral y tercerización', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '014')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('014', 'Carnes y despojos comestibles', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '017')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('017', 'Harina, polvo y "pellets" de pescado', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '019')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('019', 'Arrendamiento de bienes', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '020')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('020', 'Mantenimiento y reparación de bienes muebles', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '021')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('021', 'Movimiento de carga', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '022')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('022', 'Otros servicios empresariales', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '023')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('023', 'Leche', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '024')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('024', 'Comisión mercantil', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '025')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('025', 'Fabricación de bienes por encargo', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '026')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('026', 'Servicio de transporte de personas', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '030')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('030', 'Contratos de construcción', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '031')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('031', 'Oro gravado con el IGV', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '032')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('032', 'Páprika y otros frutos de los géneros capsicum o pimienta', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '034')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('034', 'Minerales metálicos no auríferos', 10.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '035')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('035', 'Bienes exonerados del IGV', 1.50, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '036')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('036', 'Oro y demás minerales metálicos exonerados del IGV', 4.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '039')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('039', 'Minerales no metálicos', 12.00, 1, 'SYSTEM', GETDATE());

IF NOT EXISTS (SELECT 1 FROM rcotipodetraccion WHERE Codigo = '040')
    INSERT INTO rcotipodetraccion (Codigo, Descripcion, Porcentaje, Activo, UsuarioReg, FechaReg)
    VALUES ('040', 'Bien inmueble gravado con IGV', 4.00, 1, 'SYSTEM', GETDATE());

-- ============================================================
-- ALTER TABLE (columnas agregadas después de creación inicial)
-- ============================================================

-- VoucherSyteline en rcocomprobante
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('rcocomprobante') AND name = 'VoucherSyteline'
)
    ALTER TABLE rcocomprobante ADD VoucherSyteline INT NULL;

-- EsEmpleado en rcocomprobante
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('rcocomprobante') AND name = 'EsEmpleado'
)
    ALTER TABLE rcocomprobante ADD EsEmpleado BIT NOT NULL DEFAULT 0;

-- EmpleadoCodigo en rcocomprobante
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('rcocomprobante') AND name = 'EmpleadoCodigo'
)
    ALTER TABLE rcocomprobante ADD EmpleadoCodigo VARCHAR(20) NULL;

-- EmpleadoNombre en rcocomprobante
IF NOT EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('rcocomprobante') AND name = 'EmpleadoNombre'
)
    ALTER TABLE rcocomprobante ADD EmpleadoNombre VARCHAR(200) NULL;
