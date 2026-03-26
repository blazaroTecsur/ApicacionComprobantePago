-- Tabla para almacenar documentos electrónicos adjuntos a comprobantes
-- (XML, PDF, CDR, MSG, EML, etc.)
CREATE TABLE IF NOT EXISTS rcodocumentoelectronico (
    IdDocumento     INT AUTO_INCREMENT PRIMARY KEY,
    Folio           VARCHAR(20) NOT NULL,
    TipoArchivo     VARCHAR(10) NOT NULL COMMENT 'XML, PDF, MSG, EML',
    SubTipo         VARCHAR(30) NOT NULL DEFAULT '' COMMENT 'XML_SUNAT, XML_CDR, REPRESENTACION_IMPRESA, CORREO_ORIGEN, etc.',
    NombreArchivo   VARCHAR(255) NOT NULL,
    Contenido       LONGBLOB NOT NULL,
    FechaReg        DATETIME NOT NULL,
    UsuarioReg      VARCHAR(50) NOT NULL,
    INDEX idx_folio (Folio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Si la tabla ya existe, agregar la columna SubTipo:
-- ALTER TABLE rcodocumentoelectronico
--   ADD COLUMN SubTipo VARCHAR(30) NOT NULL DEFAULT ''
--   COMMENT 'XML_SUNAT, XML_CDR, REPRESENTACION_IMPRESA, CORREO_ORIGEN, etc.'
--   AFTER TipoArchivo;
