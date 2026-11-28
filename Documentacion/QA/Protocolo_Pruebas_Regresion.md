# Protocolo de pruebas de regresión

## 1. Objetivo

Verificar que la incorporación de voces, subtítulos y modificaciones narrativas no afecte las funcionalidades previamente implementadas en YAKU.

## 2. Alcance

- Sistema de diálogos.
- Reproducción de voces.
- Interfaz.
- Inventario.
- Misiones comunitarias.
- Menú de pausa.
- Integración final.

## 3. Casos de prueba

| ID | Prueba | Resultado esperado |
|---|---|---|
| REG-01 | Iniciar una partida | Carga correcta del escenario |
| REG-02 | Conversar con Elena | Diálogo y voz correspondientes |
| REG-03 | Conversar con Tomás | Reproducción correcta |
| REG-04 | Conversar con Rosa | Reproducción correcta |
| REG-05 | Avanzar una línea narrativa | Actualización del subtítulo y audio |
| REG-06 | Cerrar un diálogo | Finalización de la voz y recuperación del control |
| REG-07 | Pausar durante una conversación | Comportamiento coherente del diálogo |
| REG-08 | Reanudar la partida | Recuperación correcta del estado |
| REG-09 | Recoger suministros | Actualización del inventario |
| REG-10 | Entregar suministros | Actualización del objetivo |
| REG-11 | Consultar el diario | Visualización de información |
| REG-12 | Completar el capítulo | Activación del cierre narrativo |
| REG-13 | Reiniciar la partida | Ausencia de estados residuales |
| REG-14 | Clonar el repositorio | Disponibilidad de los recursos de audio |
| REG-15 | Ejecutar la versión Windows | Funcionamiento de las mecánicas principales |

## 4. Clasificación de resultados

- Aprobado: comportamiento conforme al resultado esperado.
- Fallido: comportamiento diferente al esperado.
- Bloqueado: imposibilidad de ejecutar la prueba.
- Pendiente: prueba todavía no realizada.

## 5. Clasificación de incidencias

- Crítica: impide ejecutar el videojuego.
- Alta: impide completar una misión.
- Media: afecta una funcionalidad sin bloquear el recorrido.
- Baja: problema visual, sonoro o documental menor.

## 6. Registro de evidencias

Cada ejecución debe registrar:

- Fecha.
- Versión o hash evaluado.
- Identificador del caso.
- Resultado obtenido.
- Observaciones.
- Captura o video cuando corresponda.

## 7. Criterio de aceptación

La versión candidata debe permitir completar el recorrido principal sin incidencias críticas ni bloqueantes.

## 8. Estado

Este documento define el procedimiento de validación. Los resultados deberán registrarse después de ejecutar las pruebas.

**Elaboración:** Denis Prudencio.
