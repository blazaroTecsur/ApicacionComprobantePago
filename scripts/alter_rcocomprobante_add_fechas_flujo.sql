-- ============================================================
-- Migración: Agregar FechaDigitacion y FechaAutorizacion
-- Tabla   : rcocomprobante
-- Motivo  : Flujo de aprobación Digitador → Autorizador → Aprobador
-- ============================================================

ALTER TABLE rcocomprobante
    ADD COLUMN FechaDigitacion   DATETIME NULL AFTER RolDigitacion,
    ADD COLUMN FechaAutorizacion DATETIME NULL AFTER RolAutorizacion;
