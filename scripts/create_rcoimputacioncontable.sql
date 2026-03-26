-- ============================================================
-- Tabla: rcoimputacioncontable
-- Esquema simplificado para exportación Syteline
-- ============================================================

CREATE TABLE IF NOT EXISTS rcoimputacioncontable (
  IdImputacionContable INT           AUTO_INCREMENT PRIMARY KEY,
  Folio                VARCHAR(20)   NOT NULL,
  Secuencia            INT           NOT NULL,
  AliasCuenta          VARCHAR(50)   NULL,
  CuentaContable       VARCHAR(50)   NULL,
  DescripcionCuenta    VARCHAR(255)  NULL,
  Monto                DECIMAL(18,2) NOT NULL DEFAULT 0.00,
  Descripcion          VARCHAR(255)  NULL,
  Proyecto             VARCHAR(50)   NULL,
  CodUnidad1Cuenta     VARCHAR(50)   NULL,
  CodUnidad3Cuenta     VARCHAR(50)   NULL,
  CodUnidad4Cuenta     VARCHAR(50)   NULL,
  UsuarioReg           VARCHAR(50)   NOT NULL,
  FechaReg             DATETIME      NOT NULL,
  UsuarioAct           VARCHAR(50)   NULL,
  FechaAct             DATETIME      NULL,
  INDEX idx_folio (Folio)
);

-- ============================================================
-- Corrección: asegurar AUTO_INCREMENT en rcocomprobante
-- (necesario para que el campo Comprobante no salga vacío
--  en las exportaciones de Cabecera y Distribución Syteline)
-- ============================================================

ALTER TABLE rcocomprobante
  MODIFY COLUMN IdComprobante INT NOT NULL AUTO_INCREMENT;
