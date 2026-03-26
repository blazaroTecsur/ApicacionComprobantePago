-- Tabla para almacenar documentos electrónicos adjuntos a comprobantes
-- (XML, PDF, CDR extraídos de ZIP)
CREATE TABLE IF NOT EXISTS rcodocumentoelectronico (
    IdDocumento     INT AUTO_INCREMENT PRIMARY KEY,
    Folio           VARCHAR(20) NOT NULL,
    TipoArchivo     VARCHAR(10) NOT NULL COMMENT 'XML, PDF, CDR',
    NombreArchivo   VARCHAR(255) NOT NULL,
    Contenido       LONGBLOB NOT NULL,
    FechaReg        DATETIME NOT NULL,
    UsuarioReg      VARCHAR(50) NOT NULL,
    INDEX idx_folio (Folio)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
