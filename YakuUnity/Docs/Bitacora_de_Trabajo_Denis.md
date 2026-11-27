# Bitacora retrospectiva de contribuciones — Denis

**Elaborada:** 2026-11-28. Este documento registra trabajo previo, no pretende que los commits se hayan creado antes.

| Fecha real del trabajo | Actividad | Evidencia verificable | Estado |
|---|---|---|---|
| [Completar solo con fecha conocida] | Elaboracion del guion y plan de QA | Documento/archivo/captura o chat del equipo | [Completar] |
| 2026-11-28 | Integracion de voces a DialogueData y DialogueController | `b6b0bd3`: 49 audios y 12 recursos de dialogo | Publicado en feature/denis-voces-narrativa |

No rellenar fechas aproximadas como si estuvieran verificadas. Si una fecha es estimada, marcarla explicitamente como tal.

## Revision final de voces — 28 de noviembre de 2026

Responsable de la contribucion: Denis Prudencio. Referencia academica: bloque 4 (27/11/2026); esta revision y sus commits corresponden al 28/11/2026, sin cambiar fechas anteriores.

- `3a8e59d` — registrar `Assets/_Yaku/Audio/Voices.meta` con el GUID ya utilizado por Unity y retirar el metadata de la carpeta vacia `Assets/Audio/Voices`. No se modificaron los GUID de los audios.
- `f71d143` — pausar la voz cuando el juego esta pausado y reanudarla al continuar; detenerla al cerrar el dialogo.
- Auditoria en Unity: 49 WAV importados con duracion positiva, 49 subtitulos y 49 referencias unicas validas en 12 DialogueData. Las 49 asignaciones coinciden con el mapa de YakuVoiceBinder. Git registra los 49 WAV y sus 49 archivos .meta.
- Prueba dirigida en Play Mode: apertura, avance y cierre de los 12 dialogos y 49 lineas mediante invocacion de sus metodos; texto y clip coincidentes, recuperacion del movimiento al cerrar, pausa y reanudacion correctas, audio detenido al cerrar. Compilacion y consola final sin errores. No se repitio el recorrido completo de misiones ni una escucha manual de los 49 audios.
- Se conservaron los cambios locales de proyectos generados, paquetes y configuracion. No se incluyeron en los commits de voces.
- Se inspecciono y conservo `stash@{0}` de GitHub Desktop: proyectos generados, solucion y tres texturas grandes. No se aplico ni elimino.

Destino de publicacion: `feature/denis-voces-narrativa`, PR #6 hacia main. Esta revision no fusiona el PR ni modifica main.
