# Catálogo de voces narrativas — YAKU

## 1. Objetivo

Registrar los recursos de audio incorporados al capítulo comunitario y facilitar su mantenimiento, identificación y validación.

## 2. Herramientas utilizadas

- ElevenLabs: generación de voces.
- Audacity: edición, separación y exportación de grabaciones.
- Unity: importación, vinculación y reproducción.
- GitHub: almacenamiento y control de versiones.

## 3. Inventario de audios

| Personaje o categoría | Numeración | Cantidad |
|---|---|---:|
| Elena | Elena_01 a Elena_26 | 26 |
| Tomás | Tomas_01 a Tomas_07 | 7 |
| Rosa | Rosa_01 a Rosa_07 | 7 |
| Avisos y narrador | Avisos_01 a Avisos_09 | 9 |
| Total | | 49 |

Todos los recursos catalogados utilizan el formato WAV.

## 4. Ubicación

Los archivos están organizados en:

`YakuUnity/Yaku/Assets/_Yaku/Audio/Voices/`

Cada personaje dispone de una carpeta independiente.

## 5. Recursos narrativos asociados

- ChapterClosing
- DistributionPlan
- Elena
- FoodAdvisory
- FoodBriefing
- HealthGuidance
- HealthIncident
- RiverMonitoring
- Rosa
- Tomas
- WaterBriefing
- WaterReport

## 6. Componentes de integración

- DialogueController.cs: control de los diálogos y reproducción.
- DialogueData.cs: almacenamiento de información narrativa.
- YakuVoiceBinder.cs: vinculación de los recursos de audio.

## 7. Procedimiento de mantenimiento

1. Identificar la línea narrativa que requiere una modificación.
2. Generar o editar el audio correspondiente.
3. Conservar la nomenclatura establecida.
4. Importar el recurso en Unity.
5. Verificar su asociación con el diálogo correspondiente.
6. Comprobar la reproducción y sincronización.
7. Registrar los cambios en Git.

## 8. Criterios de calidad

- Voz comprensible.
- Volumen adecuado.
- Ausencia de silencios excesivos.
- Correspondencia entre voz y subtítulo.
- Identificación correcta del personaje.
- Conservación de referencias y archivos .meta.

## 9. Trazabilidad

Implementación de referencia: commit b6b0bd3.

Pull Request: https://github.com/LuisanaNapan/Yaku/pull/6

**Responsable de integración narrativa:** Denis Prudencio.
