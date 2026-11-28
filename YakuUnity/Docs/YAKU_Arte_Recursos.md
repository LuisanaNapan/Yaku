# YAKU · Recursos de arte de la comunidad

## Atribuciones

| Recurso | Autor y fuente | Licencia | Uso |
| --- | --- | --- | --- |
| Nature Kit, selección de siete FBX | [Kenney](https://kenney.nl/assets/nature-kit) | [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/) | Palmeras, arbustos, plantas, pastos, flores y piedras. Las variantes se agruparon por zona con materiales URP existentes de YAKU. |

La selección incluye `tree_palmDetailedTall`, `tree_palmBend`, `plant_flatTall`, `plant_bushDetailed`, `rock_smallFlatC`, `grass_leafsLarge` y `flower_redC`. Los FBX se conservan en `Assets/_Yaku/Art/Environment/KenneyNature`. Los taludes, las terrazas mineras, la rampa, el mobiliario y las señales se modelaron para YAKU con geometría original y materiales del proyecto.

## Reemplazo de personajes

- **Yaku.** Conserva la raíz del jugador con `PlayerRig`, `CharacterController`, `CameraTarget` y sus componentes de entrada y misión. Coloca el nuevo modelo bajo `VisualRoot`. Configura su Avatar de importación como Humanoid y prepara un controlador compatible con los parámetros `GroundSpeed`, `VerticalSpeed` y `Grounded`. Los clips provisionales fueron hechos para sus pivotes y requieren retargeting o clips humanoides.
- **Elena, Tomás y Rosa.** Cambia los modelos dentro de `VisualRoot` y conserva las raíces con `NpcRig`, `NpcDialogue`, puntos de interacción y referencias de misión. Ajusta la posición visual de los pies y la altura de cada modelo.
