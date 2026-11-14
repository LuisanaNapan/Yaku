# Bitácora de avances de YAKU: Guardianes de la Vida

La fecha de cada bloque identifica su semana académica. La columna de implementación registra el día en que se realizó el trabajo, en hora de Lima. Los avances pendientes se completarán con su hash, su fecha real y las pruebas realizadas cuando se registren. Las pruebas propuestas para esos avances se indican como pendientes.

Rama de trabajo `feature/programacion-yaku`. Respaldo local del primer avance `respaldo/yaku-commit-1`. Documento de referencia [GDD de YAKU](YAKU_GDD_Completo_2026.md).

## Bloque 1 · Semana académica del 7 de noviembre de 2026

| Avance | Funcionalidad | Hash | Fecha real de implementación | Pruebas realizadas o pendientes |
| --- | --- | --- | --- | --- |
| 1 | Estructura técnica y escena base `01_Community`. Poblado, ribera, cámara fija de presentación, prefab de Yaku con modelo provisional separado y puntos de interacción. | [`85f2c4585c614e67da35ac8ead0fa397da24f8e7`](https://github.com/LuisanaNapan/Yaku/commit/85f2c4585c614e67da35ac8ead0fa397da24f8e7) | 27 de noviembre de 2026 | Se abrió la escena mediante Unity MCP y se confirmó en Play Mode la presencia de Yaku, su `CharacterController` y la cámara. Se comprobó que sustituir un objeto en `VisualRoot` conservaba el controlador y el objetivo de cámara. La última consulta de consola devolvió cero errores y advertencias. |
| 2 | Movimiento WASD relativo a la cámara y giro de Yaku hacia la dirección de avance. | Pendiente | Pendiente | Pendiente. Probar las cuatro direcciones, verificar la velocidad diagonal y comprobar que Yaku se detiene al soltar las teclas. |
| 3 | Carrera con Shift izquierdo, salto con Espacio, gravedad y detección de suelo. Animaciones corporales provisionales para reposo, marcha, carrera, salto, caída y aterrizaje. | Commit que incorpora esta bitácora | 27 de noviembre de 2026 | Pasaron 12 casos de movimiento y tres comprobaciones de animación en Play Mode mediante Unity MCP. Se verificaron WASD, diagonales, cámara rotada, salto de 1,049 m, rechazo de Espacio en el aire, caída desde 0,65 m, balanceo alternado, flexión al aterrizar y regreso a reposo. La consola devolvió cero errores y advertencias. |
| 4 | Cámara en tercera persona con seguimiento de `CameraTarget`, órbita con `Player/Look`, encuadre y suavizado configurables. **Esc** libera el cursor y el clic izquierdo en la vista de juego lo recupera. | Pendiente de commit | 27 de noviembre de 2026 | Pasaron 11 comprobaciones en Play Mode mediante Unity MCP. Se verificaron órbita de 360°, inclinación de −12° a 65°, WASD tras girar 90°, seguimiento al caminar, correr, saltar y aterrizar, y liberación y recuperación del cursor. Se revisaron capturas de `01_Community`. La consola devolvió cero errores y advertencias. |
| 5 | Colisiones del personaje y de la cámara. Ajuste del `CharacterController`, colisiones en techos y troncos de palmera, detección de obstáculos con `SphereCast` y recuperación gradual de la distancia de cámara. | Commit que registra el avance 5 | 27 de noviembre de 2026 | Pasaron 39 comprobaciones en Play Mode mediante Unity MCP. Se verificaron paredes, troncos, vivienda, escalones, pendientes, bordes, sendero, salto junto a una pared, órbita, recuperación de cámara, WASD relativo y las seis animaciones. La consola devolvió cero errores y advertencias. |

El commit 1 conserva las fechas de autor y de committer registradas en Git. Ambas son `2026-11-27T10:13:57-05:00`.

### Avance 3 · Validación funcional y animaciones provisionales

Se aprobó el avance 3 con carrera, salto, gravedad y animaciones corporales provisionales. El desarrollo se realizó el 27 de noviembre de 2026, en hora de Lima, y corresponde a la semana académica del 7 de noviembre de 2026. La publicación se revisará por separado.

- El modelo provisional usa pivotes en pelvis, torso, hombros, codos, caderas, rodillas y tobillos. El `Animator Controller` mezcla `Idle`, `Walk` y `Run`, y transiciona entre `Jump`, `Fall`, `Land` y la locomoción.
- El Animator toma la velocidad real, la velocidad vertical y el contacto con el suelo del `CharacterController`. El movimiento del jugador sigue a cargo de `PlayerMovement`.
- Queda pendiente el modelo 3D definitivo con rig humanoide. Podrá montarse bajo `VisualRoot` con su propio controlador de animación, conservando `PlayerRig` y el controlador de movimiento.

## Bloque 2 · Semana académica del 14 de noviembre de 2026

| Avance | Funcionalidad | Hash | Fecha real de implementación | Pruebas realizadas o pendientes |
| --- | --- | --- | --- | --- |
| 6 | Detección de objetos interactivos y aviso contextual de **E**. | Ver historial de la rama | 27 de noviembre de 2026 | Play Mode mediante Unity MCP. Cinco comprobaciones aprobadas de alcance, aviso, selección por proximidad y pulsaciones de E. Sin errores ni advertencias. |
| 7 | Diálogos configurables con Elena, Tomás y Rosa. | Ver historial de la rama | 27 de noviembre de 2026 | 15 comprobaciones en Play Mode mediante Unity MCP. Referencias, aviso, apertura, avance, cierre y recuperación del movimiento con Elena, Tomás y Rosa. Sin errores ni advertencias. |
| 8 | Inventario de seis espacios, límites por pila y objetos recolectables. | Ver historial de la rama | 27 de noviembre de 2026 | Nueve comprobaciones aprobadas en Unity MCP, incluidos seis espacios, apilado, retiro, rechazo por capacidad, cantidades inválidas y HUD con icono. Consola limpia. |
| 9 | Validación de entregas por destinatario, objeto, cantidad y procedencia. | Ver historial de la rama | 27 de noviembre de 2026 | Cinco comprobaciones en Play Mode. Recogida con E, inventario lleno, entrega sin recursos, descuento correcto y prevención del descuento repetido. Consola limpia. |
| 10 | Progresión de misiones, objetivo activo en el HUD y tutorial «El primer encargo». | Pendiente | Pendiente | Pendiente. Completar el tutorial sin ayuda externa y verificar que la ribera se habilita al finalizar. |

## Bloque 3 · Semana académica del 21 de noviembre de 2026

| Avance | Funcionalidad | Hash | Fecha real de implementación | Pruebas realizadas o pendientes |
| --- | --- | --- | --- | --- |
| 11 | Misión de agua «El peligro invisible», con aviso de monitoreo, decisión y entrega. | Pendiente | Pendiente | Pendiente. Elegir primero una fuente sin verificar, recibir la explicación y completar la misión tras corregir la decisión. |
| 12 | Misión de alimentos «Elegir con información», con ficha, tres opciones y justificación. | Pendiente | Pendiente | Pendiente. Comparar una elección sin respaldo con una informada y verificar que la recompensa se concede una sola vez. |
| 13 | Secuencia guiada de orientación sanitaria con Tomás y Elena. | Pendiente | Pendiente | Pendiente. Recorrer el incidente cotidiano y la sospecha de exposición, y verificar el diálogo y el objetivo correspondiente. |
| 14 | Distribución comunitaria de tres paquetes y confirmación final de Elena. | Pendiente | Pendiente | Pendiente. Probar entregas incompletas, incorrectas y repetidas. Confirmar el cierre tras las tres entregas válidas. |
| 15 | Diario de objetivos y aprendizajes, y resumen final de decisiones y entregas. | Pendiente | Pendiente | Pendiente. Consultar el diario con **J** durante las misiones y cotejar el resumen con una partida que incluya un error y su corrección. |

## Bloque 4 · Semana académica del 28 de noviembre de 2026

| Avance | Funcionalidad | Hash | Fecha real de implementación | Pruebas realizadas o pendientes |
| --- | --- | --- | --- | --- |
| 16 | Menú principal, controles, créditos, pausa con Esc y retorno al menú. | Pendiente | Pendiente | Pendiente. Iniciar, pausar, reanudar y reiniciar una partida, comprobando el cursor y los controles. |
| 17 | Legibilidad del HUD, contraste, etiquetas y navegación por teclado. | Pendiente | Pendiente | Pendiente. Revisar el HUD a 1920×1080 y 1366×768 y completar una interacción usando el teclado. |
| 18 | Ambiente, efectos de audio y control de volumen. | Pendiente | Pendiente | Pendiente. Comprobar reproducción, nivel de volumen y ausencia de audio duplicado al cambiar de escena. |
| 19 | Pruebas integradas de progresión, inventario y reintentos, y perfilado del recorrido. | Pendiente | Pendiente | Pendiente. Ejecutar las regresiones y medir el rendimiento y la duración de una partida en un equipo académico. |
| 20 | Compilación Windows y validación final del prototipo. | Pendiente | Pendiente | Pendiente. Completar una partida fuera del editor y verificar misiones, cierre, controles, interfaz y audio. |
