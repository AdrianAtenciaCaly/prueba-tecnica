-- ============================================================================
-- BaseDatos.sql
-- Script de creación de esquema para la prueba técnica de microservicios.
-- Se crean DOS bases de datos independientes (una por cada microservicio),
-- reflejando que cada servicio es dueño exclusivo de sus propios datos
-- (principio de "Database per Service" en arquitecturas de microservicios).
-- Motor: PostgreSQL 16
-- ============================================================================

-- ============================================================================
-- BASE DE DATOS: clientes_db  (usada por ClientesService)
-- ============================================================================
CREATE DATABASE clientes_db;

\connect clientes_db

CREATE TABLE clientes (
    id                UUID PRIMARY KEY,
    nombre            VARCHAR(150) NOT NULL,
    genero            VARCHAR(20)  NOT NULL,
    edad              INT          NOT NULL CHECK (edad BETWEEN 0 AND 120),
    identificacion    VARCHAR(30)  NOT NULL,
    direccion         VARCHAR(250) NOT NULL,
    telefono          VARCHAR(20)  NOT NULL,
    cliente_id        VARCHAR(50)  NOT NULL,
    contrasena_hash   TEXT         NOT NULL,
    estado            BOOLEAN      NOT NULL DEFAULT TRUE
);

CREATE UNIQUE INDEX ux_clientes_cliente_id     ON clientes (cliente_id);
CREATE UNIQUE INDEX ux_clientes_identificacion ON clientes (identificacion);

-- Datos de ejemplo según el "Caso de Uso 1: Creación de Usuarios" del enunciado
INSERT INTO clientes (id, nombre, genero, edad, identificacion, direccion, telefono, cliente_id, contrasena_hash, estado) VALUES
    (gen_random_uuid(), 'Jose Lema',           'Masculino', 35, '1023456781', 'Otavalo sn y principal', '098254785', 'CLI001', '$2a$11$examplehasheddatanotrealbcrypt0000000000000000000000', TRUE),
    (gen_random_uuid(), 'Marianela Montalvo',  'Femenino',  32, '1023456782', 'Amazonas y NNUU',        '097548965', 'CLI002', '$2a$11$examplehasheddatanotrealbcrypt0000000000000000000001', TRUE),
    (gen_random_uuid(), 'Juan Osorio',         'Masculino', 40, '1023456783', '13 junio y Equinoccial', '098874587', 'CLI003', '$2a$11$examplehasheddatanotrealbcrypt0000000000000000000002', TRUE);

-- Nota: los hashes de ejemplo NO son contraseñas reales utilizables; al crear clientes vía la API,
-- ClientesService genera el hash real con BCrypt a partir de la contraseña en texto plano recibida.


-- ============================================================================
-- BASE DE DATOS: cuentas_db  (usada por CuentasService)
-- ============================================================================
CREATE DATABASE cuentas_db;

\connect cuentas_db

-- Read-model local de Cliente, alimentado de forma asíncrona vía eventos (RabbitMQ/MassTransit)
-- publicados por ClientesService. No es una copia de la tabla clientes de clientes_db: cada
-- microservicio mantiene su propia base y solo lo que necesita del otro dominio.
CREATE TABLE clientes_referencia (
    cliente_id     VARCHAR(50) PRIMARY KEY,
    nombre         VARCHAR(150) NOT NULL,
    estado         BOOLEAN      NOT NULL DEFAULT TRUE,
    actualizado_en TIMESTAMP    NOT NULL DEFAULT now()
);

CREATE TABLE cuentas (
    id             UUID PRIMARY KEY,
    numero_cuenta  VARCHAR(20)   NOT NULL,
    tipo_cuenta    VARCHAR(20)   NOT NULL CHECK (tipo_cuenta IN ('Ahorros', 'Corriente')),
    saldo_inicial  NUMERIC(18,2) NOT NULL CHECK (saldo_inicial >= 0),
    saldo_actual   NUMERIC(18,2) NOT NULL,
    estado         BOOLEAN       NOT NULL DEFAULT TRUE,
    cliente_id     VARCHAR(50)   NOT NULL
);

CREATE UNIQUE INDEX ux_cuentas_numero_cuenta ON cuentas (numero_cuenta);
CREATE INDEX ix_cuentas_cliente_id ON cuentas (cliente_id);

CREATE TABLE movimientos (
    id               UUID PRIMARY KEY,
    fecha            TIMESTAMP     NOT NULL,
    tipo_movimiento  VARCHAR(20)   NOT NULL CHECK (tipo_movimiento IN ('Deposito', 'Retiro')),
    valor            NUMERIC(18,2) NOT NULL CHECK (valor > 0),
    saldo            NUMERIC(18,2) NOT NULL,
    cuenta_id        UUID          NOT NULL REFERENCES cuentas (id) ON DELETE CASCADE
);

CREATE INDEX ix_movimientos_cuenta_id ON movimientos (cuenta_id);
CREATE INDEX ix_movimientos_fecha     ON movimientos (fecha);

-- Read-model de clientes, reflejando los mismos 3 clientes de ejemplo (normalmente llegarían solos vía eventos)
INSERT INTO clientes_referencia (cliente_id, nombre, estado) VALUES
    ('CLI001', 'Jose Lema', TRUE),
    ('CLI002', 'Marianela Montalvo', TRUE),
    ('CLI003', 'Juan Osorio', TRUE);

-- Datos de ejemplo según "Caso de Uso 2 y 3: Creación de Cuentas de Usuario" del enunciado
INSERT INTO cuentas (id, numero_cuenta, tipo_cuenta, saldo_inicial, saldo_actual, estado, cliente_id) VALUES
    (gen_random_uuid(), '478758', 'Ahorros',   2000, 2000, TRUE, 'CLI001'),
    (gen_random_uuid(), '225487', 'Corriente', 100,  100,  TRUE, 'CLI002'),
    (gen_random_uuid(), '495878', 'Ahorros',   0,    0,    TRUE, 'CLI002'),
    (gen_random_uuid(), '496825', 'Ahorros',   540,  540,  TRUE, 'CLI002'),
    (gen_random_uuid(), '585545', 'Corriente', 1000, 1000, TRUE, 'CLI001');

-- Nota: los movimientos del "Caso de Uso 4" del enunciado se generan a través del endpoint
-- POST /movimientos (para respetar la lógica de negocio de actualización de saldo en la capa de dominio,
-- en vez de insertarlos directamente por SQL, que saltaría las validaciones de F2/F3).
