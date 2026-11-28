# Registro de incidencias y riesgos — YAKU

## 1. Objetivo

Identificar aspectos técnicos y documentales que requieren seguimiento antes de la entrega final del videojuego.

## 2. Registro

| ID | Descripción | Prioridad | Estado inicial |
|---|---|---|---|
| INC-01 | Posible inconsistencia en la ubicación de Voices.meta | Alta | Por verificar |
| INC-02 | Diferencia entre María en documentación y Rosa en el prototipo | Media | Por coordinar |
| INC-03 | Conflictos reportados en archivos .meta del personaje Mercurio | Alta | Por coordinar |
| INC-04 | Compatibilidad de voces después de integrar ramas | Alta | Pendiente de prueba |
| INC-05 | Funcionamiento del ejecutable en otro equipo | Alta | Pendiente de prueba |
| INC-06 | Disponibilidad de evidencias visuales adicionales de QA | Media | Pendiente |
| INC-07 | Cambios locales de configuración de Unity | Media | Por revisar |

## 3. Acciones recomendadas

### Recursos de audio

Verificar la ubicación correcta de los archivos .meta, sus GUID y las referencias de los recursos narrativos.

### Coherencia narrativa

Acordar la nomenclatura definitiva de los personajes y actualizar los documentos correspondientes.

### Integración del escenario

Coordinar con los responsables de las ramas involucradas antes de resolver conflictos relacionados con texturas, materiales o modelos.

### Compatibilidad

Ejecutar las pruebas finales desde una copia limpia del repositorio.

### Evidencias

Registrar capturas de pantalla, videos y resultados de las pruebas funcionales.

## 4. Procedimiento de resolución

1. Identificar la incidencia.
2. Reproducirla cuando corresponda.
3. Registrar la versión afectada.
4. Aplicar una corrección controlada.
5. Ejecutar pruebas de regresión.
6. Actualizar el estado y registrar el commit de solución.

## 5. Consideraciones

Los riesgos registrados no representan necesariamente errores funcionales confirmados. El estado de cada incidencia debe actualizarse de acuerdo con las comprobaciones posteriores.

**Elaboración y seguimiento:** Denis Prudencio.
