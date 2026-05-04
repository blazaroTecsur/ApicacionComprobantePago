-- ============================================================
-- INSERT tmacodigounidad1 — codigos deduplicados (primera
-- ocurrencia por Codigo). Idempotente: no inserta si ya existe.
-- ============================================================

-- 1xxx — Gerencia / Administracion
INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1000', 'GERENCIA GENERAL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1000');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1100', 'AREA LEGAL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1100');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1101', 'SUPERVISION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1101');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1102', 'TALLER FLOTA LIVIANA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1102');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1103', 'TALLER FLOTA PESADA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1103');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1105', 'SSOMA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1105');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1107', 'SERVICIOS GENERALES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1107');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1108', 'MANTENIMIENTO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1108');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1109', 'OPERACIONES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1109');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1110', 'AUDITORIA INTERNA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1110');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1111', 'ATENCION DE SINIESTRO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1111');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1112', 'CONTABILIDAD', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1112');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1113', 'RRHH', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1113');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1114', 'AUDITORIA INTERNA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1114');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1115', 'SSOMA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1115');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1200', 'AUDITORIA INTERNA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1200');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1300', 'PREVENCION DE RIESGO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1300');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1301', 'SALUD OCUPACIONAL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1301');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '1800', 'MEJORA Y PROCESOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '1800');

-- 2xxx — Administracion y Finanzas
INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2000', 'GERENCIA DE ADMINISTRACION Y F', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2000');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2100', 'INFORMATICA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2100');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2110', 'CONTABILIDAD', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2110');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2120', 'COMUNICACIONES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2120');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2140', 'VIDEO VIGILANCIA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2140');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2200', 'CONTABILIDAD', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2200');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2300', 'FINANZAS Y PRESUPUESTOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2300');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2400', 'RECURSOS HUMANOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2400');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2410', 'CAPACITACION EXTERNA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2410');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2420', 'CAPACITACION INTERNA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2420');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2430', 'SEGURIDAD PATRIMONIAL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2430');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2500', 'SERVICIOS GENERALES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2500');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2700', 'ALQUILER VEHICULOS ELECTRICOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2700');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2710', 'ALQUILER DE VEHICULOS Y MAQUINARIAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2710');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '2800', 'PROYECTOS ERP', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '2800');

-- 3xxx — Comercial / Transporte
INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3000', 'GERENCIA COMERCIAL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3000');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3050', 'VENTA MATERIALES TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3050');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3100', 'SERVICIOS INDUSTRIALES TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3100');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3101', 'VENTA DE SERVICIOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3101');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3111', 'TECSUR CAMIONETAS 4X2 ,COMBI,AUTOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3111');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3115', 'TECSUR CAMIONES CANTER PORTER', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3115');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3116', 'HITACHI RAIL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3116');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3117', 'TORRE HOSPITALARIA COSAPI', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3117');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3118', 'HITACHI L2L4', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3118');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3124', 'TECSUR CAMIONETAS 4X4', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3124');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3126', 'TECSUR OTROS SERVICIOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3126');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3128', 'TECSUR CAMION TRACTO TERCERO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3128');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3129', 'TECSUR GRUAS TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3129');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3131', 'TECSUR POOL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3131');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3133', 'TECSUR GRUAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3133');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3134', 'TECSUR FLOTA LIVIANA TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3134');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3135', 'TECSUR CAMIONETAS TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3135');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3137', 'TECSUR CAMIONETAS ELECTRICAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3137');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3140', 'TECSUR GRUAS CORPORACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3140');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3141', 'TECSUR MANTENIMIENTO UNIDAD ELECTRICAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3141');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3142', 'TECSUR SINIESTRO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3142');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3143', 'TECSUR IMPLEMENTACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3143');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3144', 'TECSUR OTROS MANTENIMIENTO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3144');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3200', 'TRANSPORTE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3200');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3201', 'ELECTROMOVILIDAD', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3201');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3202', 'ESTACION DE CARGA R3', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3202');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3203', 'ESTACION CAÑETE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3203');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3204', 'ESTACION DE CARGA SAN BARTOLO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3204');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3205', 'ESTACION DE CARGA SIGLO XXI', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3205');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3206', 'ESTACION DE CARGA CHOSICA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3206');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3207', 'PROYECYO MAJES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3207');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3212', 'LDS BRAZOS HIDRAULICOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3212');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3213', 'LDS CAMIONETAS 4X2', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3213');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3214', 'LDS CAMIONETAS 4X4', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3214');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3215', 'LDS CAMIONETAS Y AUTOS POOL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3215');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3217', 'LDS GRUAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3217');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3218', 'LDS OTROS SERVICIOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3218');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3220', 'LDS GRUAS TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3220');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3221', 'LDS CAMION TRACTO TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3221');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3222', 'LDS CAMIONES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3222');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3234', 'LDS FLOTA LIVIANA TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3234');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3235', 'LDS CAMIONETAS TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3235');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3236', 'LDS BRAZOS AISLADO CORPORACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3236');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3237', 'LDS CAMIONETAS ELECTRICAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3237');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3238', 'LDS CAMIONETAS DIESEL FORD', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3238');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3240', 'LDS GRUAS CORPORACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3240');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3241', 'LDS CAMION PORTER DIESEL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3241');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3242', 'LDS SINIESTROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3242');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3243', 'LDS IMPLEMENTACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3243');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3244', 'LDS OTROS MANTENIMIENTOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3244');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3245', 'LDS CAMIONETA HIBRIDA 4X4', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3245');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3246', 'LDS BRAZO AISLADO HIDROLAVADOR TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3246');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3300', 'GESTION DE PROYECTOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3300');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3318', 'GCI CAMIONETAS 4X2', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3318');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3320', 'GCI CAMIONES CANTER, PORTER', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3320');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3321', 'GCI BRAZOS HIDRAULICOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3321');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3322', 'GCI OTROS SERVICIOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3322');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3325', 'GCI CAMIONETAS 4X4', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3325');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3327', 'GCI GRUAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3327');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3328', 'GCI GRUAS TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3328');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3329', 'GCI CAMION TRACTO TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3329');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3331', 'GCI POOL', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3331');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3335', 'GCI CAMIONETAS TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3335');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3337', 'GCI CAMIONETAS ELECTRICAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3337');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3338', 'GCI CAMIONETAS DIESEL FORD', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3338');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3339', 'GCI CAMIONES ELECTRICOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3339');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3340', 'GCI GRUAS 8TN', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3340');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3342', 'GCI SINIESTRO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3342');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3343', 'GCI IMPLEMENTACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3343');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3344', 'GCI OTROS MANTENIMIENTO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3344');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3400', 'MARKETING', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3400');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3410', 'TERCEROS GRUAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3410');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3430', 'TERCEROS OTROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3430');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3440', 'TERCEROS FLOTA PROPIA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3440');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3500', 'TRANSPORTE APIM', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3500');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3510', 'APIM MINIBUS EV', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3510');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3520', 'APIM SINIESTROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3520');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3530', 'APIM IMPLEMENTACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3530');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '3540', 'APIM OTROS MANTENIMIENTO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '3540');

-- 4xxx — Operaciones
INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4000', 'GERENCIA DE OPERACIO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4000');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4040', 'PROYECTOS Y SISTEMAS DE UTILIZACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4040');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4050', 'GESTION DE PROYECTOS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4050');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4060', 'INGENIERIA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4060');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4220', 'EMERGENCIA Y ALUMBRADO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4220');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4230', 'MANTENIMIENTO DISTRIBUCION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4230');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4235', 'MANTENIMIENTO MEDIA TENSION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4235');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4300', 'OBRAS MEDIA TENSION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4300');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4301', 'OBRAS BAJA TENSION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4301');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4440', 'OPERACIONES CAÑETE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4440');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4620', 'OTROS SERVICIOS LDS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4620');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4700', 'CONTROL DE OBRAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4700');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4800', 'EJECUCION DE OBRAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4800');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4805', 'MANTENIMIENTO AP', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4805');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4808', 'PROYECTOS MT', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4808');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4900', 'SET PACHACUTEC - DIRECCION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4900');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4904', 'PROYECTOS TRANSMISION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4904');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '4905', 'MANTENIMIENTO TRANSMISION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '4905');

-- 5xxx — Logistica / Ventas / Servicios de campo
INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5000', 'GERENCIA DE LOGISTICA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5000');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5100', 'COMPRAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5100');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5200', 'ALMACENES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5200');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5210', 'ALMACENES LDS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5210');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5240', 'MEDIDORES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5240');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5320', 'RECLAMOS AP LIMA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5320');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5330', 'RECLAMOS BT LIMA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5330');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5333', 'VERIFICACION REDES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5333');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5340', 'EMERGENCIA LIMA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5340');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5360', 'FUGAS A TIERRA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5360');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5361', 'REFORMA MANTENIMIENTO SUBTERRANEA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5361');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5370', 'MANTENIMIENTO DE MEDIA TENSION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5370');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5371', 'PROYECTOS BAJA TENSION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5371');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5372', 'MTO PAT / MTO RETENIDAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5372');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5373', 'PEM', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5373');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5376', 'SED', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5376');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5377', 'PODA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5377');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5378', 'LAVADO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5378');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5380', 'CONEXIONES DE RUTINA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5380');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5390', 'PROYECTOS MEDIA TENSIÓN', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5390');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5400', 'METROLOGIA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5400');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5410', 'CONTROL DE CALIDAD', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5410');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5420', 'MANTENIMIENTO Y PROYECTOS MT', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5420');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5430', 'RECLAMOS BT - CAÑETE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5430');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5440', 'EMERGENCIA - CAÑETE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5440');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5450', 'MANTENIMIENTO Y PROYECTOS BT', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5450');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5500', 'VENTAS LUZ DEL SUR', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5500');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5511', 'CAMBIO DE POSTES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5511');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5530', 'MATERIALES CONTRATISTAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5530');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5540', 'DESMEDRO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5540');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5600', 'VENTAS LOS ANDES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5600');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5620', 'VENTA GCI', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5620');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5700', 'RECICLAJE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5700');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5710', 'MATERIALES OBSOLETOS RECICLAJE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5710');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5720', 'LUMINARIAS REHABILITADAS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5720');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5730', 'MUESTREO Y REMEDIACION', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5730');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5740', 'VENTA MATERIAL RECICLADO', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5740');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5760', 'TRANSFORMADORES', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5760');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '5780', 'RAEE', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '5780');

-- 6xxx — Nivel empresa
INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '6000', 'NIVEL EMPRESA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '6000');

-- 7xxx — Servicios / Oficina tecnica
INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '7400', 'SERVICIOS TERCEROS', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '7400');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '7500', 'OFICINA TECNICA', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '7500');

INSERT INTO tmacodigounidad1 (Codigo, Descripcion, Empresa, Activo, UsuarioReg, FechaReg)
SELECT '7600', 'VENTA TECSUR', '', 1, 'SISTEMA', GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM tmacodigounidad1 WHERE Codigo = '7600');
