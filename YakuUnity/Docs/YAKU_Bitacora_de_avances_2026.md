# Bitácora de avances de YAKU: Guardianes de la Vida

La fecha de cada bloque identifica su semana académica. La columna de implementación registra el día en que se realizó el trabajo, en hora de Lima. Los avances pendientes se completarán con su hash, su fecha real y las pruebas realizadas cuando se registren. Las pruebas propuestas para esos avances se indican como pendientes.

Rama de trabajo `feature/programacion-yaku`. Respaldo local del primer avance `respaldo/yaku-commit-1`. Documento de referencia [GDD de YAKU](YAKU_GDD_Completo_2026.md).

## Bloque 1 · Semana académica del 7 de noviembre de 2026

| Avance | Funcionalidad | Hash | Fecha real de implementación | Pruebas realizadas o pendientes |
| --- | --- | --- | --- | --- |
| 1 | Estructura técnica y escena base `01_Community`. Poblado, ribera, cámara fija de presentación, prefab de Yaku con modelo provisional separado y puntos de interacción. | [`85f2c4585c614e67da35ac8ead0fa397da24f8e7`](https://github.com/LuisanaNapan/Yaku/commit/85f2c4585c614e67da35ac8ead0fa397da24f8e7) | 27 de noviembre de 2026 | Se abrió la escena mediante Unity MCP y se confirmó en Play Mode la presencia de Yaku, su `CharacterController` y la cámara. Se comprobó que sustituir un objeto en `VisualRoot` conservaba el controlador y el objetivo de cámara. La última consulta de consola devolvió cero errores y advertencias. |
| 2 | Movimiento WASD relativo a la cámara y giro de Yaku hacia la dirección de avance. | Pendiente | Pendiente | Pendiente. Probar las cuatro direcciones, verificar la velocidad diagonal y comprobar que Yaku se detiene al soltar las teclas. |
| 3 | Carrera con Shift izquierdo, salto con Espacio, gravedad y detección de suelo. Animaciones corporales provisionales para reposo, marcha, carrera, salto, caída y aterrizaje. | Commit que incorpora esta bitácora | 27 de noviembre de 2026 | Pasaron 12 casos de movimiento y tres comprobaciones de animación en Play Mode mediante Unity MCP. Se verificaron WASD, diagonales, cámara rotada, salto de 1,049 m, rechazo de Espacio en el aire, caída desde 0,65 m, balanceo alternado, flexión al aterrizar y regreso a reposo. La consola devolvió cero errores y advertencias. |
| 4 | Cámara en tercera persona con seguimiento y órbita mediante el ratón. | Pendiente | Pendiente | Pendiente. Girar la cámara, comprobar sus límites verticales y repetir el movimiento con la cámara rotada. |
| 5 | Colisiones del personaje y de la cámara y pruebas de movimiento. | Pendiente | Pendiente | Pendiente. Recorrer paredes, rampas, escalones y bordes, y ejecutar las pruebas de regresión de movimiento. |

El commit 1 conserva las fechas de autor y de committer registradas en Git. Ambas son `2026-11-27T10:13:57-05:00`.

### Avance 3 · Validación funcional y animaciones provisionales

Se aprobó el avance 3 con carrera, salto, gravedad y animaciones corporales provisionales. El desarrollo se realizó el 27 de noviembre de 2026, en hora de Lima, y corresponde a la semana académica del 7 de noviembre de 2026. La publicación se revisará por separado.

- El modelo provisional usa pivotes en pelvis, torso, hombros, codos, caderas, rodillas y tobillos. El `Animator Controller` mezcla `Idle`, `Walk` y `Run`, y transiciona entre `Jump`, `Fall`, `Land` y la locomoción.
- El Animator toma la velocidad real, la velocidad vertical y el contacto con el suelo del `CharacterController`. El movimiento del jugador sigue a cargo de `PlayerMovement`.
- Queda pendiente el modelo 3D definitivo con rig humanoide. Podrá montarse bajo `VisualRoot` con su propio controlador de animación, conservando `PlayerRig` y el controlador de movimiento.

## Bloque 2 · Semana académica del 14 de noviembre de 2026

| Avance | Funcionalidad | Hash | Fecha real de implementación | Pruebas realizadas o pendientes |
| --- | --- | --- | --- | --- |
| 6 | Detección de objetos interactivos y aviso contextual de **E**. | Pendiente | Pendiente | Pendiente. Entrar y salir del alcance y comprobar cuál de dos objetos cercanos recibe la interacción. |
| 7 | Diálogos configurables con Elena, Tomás y Rosa. | Pendiente | Pendiente | Pendiente. Hablar con los tres personajes, elegir una respuesta y comprobar que se recuperan los controles al cerrar el diálogo. |
| 8 | Inventario de seis espacios, límites por pila y objetos recolectables. | Pendiente | Pendiente | Pendiente. Llenar el inventario y comprobar que un objeto rechazado sigue disponible en la escena. |
| 9 | Validación de entregas por destinatario, objeto, cantidad y procedencia. | Pendiente | Pendiente | Pendiente. Probar un destinatario incorrecto, un objeto ausente y una entrega repetida. Verificar el consumo de inventario solo en la entrega válida. |
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
