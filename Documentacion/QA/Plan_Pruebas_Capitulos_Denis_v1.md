# YAKU — Plan de pruebas del capítulo comunitario (v1)

**Responsable:** Denis Prudencio  
**Proyecto:** `LuisanaNapan/Yaku`  
**Escena:** `Assets/_Yaku/Scenes/01_Community.unity`  
**Estado:** plan de pruebas; capítulos pendientes de verificación manual en esta fase.

## 1. Línea base reportada

En revisión previa el usuario informó **7/7 comprobaciones correctas**: apertura de Unity, movimiento, interacción con Elena, primera misión, escenario/materiales, interfaz y Console. Una captura de Scene mostró viviendas, vegetación, senderos, ribera, personajes y objetos de misión, con contadores de Console en cero. Esto NO demuestra aún la ejecución de la cadena completa de capítulos.

## 2. Secuencia de prueba de objetivos

Orden derivado de la lista `CommunityChapter.objectives` almacenada en `01_Community.unity`. La enumeración de alternativas comienza en 1 para el jugador.

| ID | Misión | Acción de prueba | Resultado esperado | Estado |
|---|---|---|---|---|
| C01 | El peligro invisible | Hablar con Elena en la posta | Se avanza al siguiente objetivo | Pendiente |
| C02 | El peligro invisible | Leer aviso del sendero | Se muestra información simulada | Pendiente |
| C03 | El peligro invisible | Comparar fuentes y elegir **opción 2** | Se acepta fuente verificada | Pendiente |
| C04 | El peligro invisible | Informar a Elena | Misión completada; inicia alimentación | Pendiente |
| C05 | Elegir con información | Hablar con Rosa | Se explica riesgo alimentario | Pendiente |
| C06 | Elegir con información | Leer ficha de alimentación | Se visualiza el aviso simulado | Pendiente |
| C07 | Elegir con información | Comparar lotes y elegir **opción 2** | Se acepta lote B documentado | Pendiente |
| C08 | Elegir con información | Justificar ante Rosa con **opción 2** | Se acepta la evidencia de la ficha | Pendiente |
| C09 | Atender y orientar | Hablar con Tomás del incidente | Se registran las dos necesidades | Pendiente |
| C10 | Atender y orientar | Comunicarlo a Elena | Se indica paquete de apoyo | Pendiente |
| C11 | Atender y orientar | Elegir el paquete de la posta, **opción 1** | Se acepta el material solicitado | Pendiente |
| C12 | Atender y orientar | Confirmar orientación con Elena, **opción 3** | Se recomienda consulta profesional | Pendiente |
| C13 | Una comunidad preparada | Coordinar entregas con Elena | Se activa reparto de tres paquetes | Pendiente |
| C14 | Una comunidad preparada | Recoger y entregar los tres paquetes | Indicador de entrega llega a 3/3 | Pendiente |
| C15 | Una comunidad preparada | Confirmar evidencia con Elena, **opción 1** | Se reconoce la verificación de la fuente | Pendiente |
| C16 | Cierre del capítulo | Escuchar a Elena | Se completa el capítulo y abre diario | Pendiente |

## 3. Pruebas complementarias

- Elegir deliberadamente una opción incorrecta en un punto de decisión: comprobar que aparece retroalimentación y que la misión permite reintentar sin quedar bloqueada.
- Abrir el diario con `J` durante el capítulo y al concluir; revisar las vistas `1` (resumen) y `2` (aprendizajes).
- Comprobar si el texto de los diálogos queda dentro del panel, especialmente en resoluciones diferentes.
- Verificar que el contador de suministros y el HUD se actualizan tras cada entrega.
- Revisar rutas de acceso, colisiones, marcadores de interacción y ausencia de bloqueos de personaje/cámara.
- Revisar Console después de cada misión para detectar errores rojos, mensajes repetidos y referencias faltantes.
- Confirmar que el material educativo mantiene diferenciadas las fuentes verificadas ficticias frente a los avisos reales de las autoridades sanitarias.

## 4. Registro de incidencias — plantilla

| ID | Fecha | Escena / objetivo | Pasos para reproducir | Resultado esperado | Resultado obtenido | Severidad | Captura | Estado |
|---|---|---|---|---|---|---|---|---|
| QA-001 | Por completar | — | — | — | — | — | — | Pendiente |

**Severidad sugerida:** alta = bloqueo o pérdida de progreso; media = misión/hud incorrectos pero se puede continuar; baja = detalle visual o de redacción.

## 5. Evidencias requeridas

Guardar capturas con nombres claros: `C03_Agua_Seleccion.png`, `C07_Alimentos_LoteB.png`, `C12_Salud_Orientacion.png`, `C14_Distribucion_3de3.png`, `C16_Diario_Final.png`. No considerarlas existentes hasta obtenerlas en Unity.

## 6. Guía para un commit de documentación

Cuando se agregue este plan junto con el guion propuesto y se revise que no se han modificado scripts o escenas ajenas, se puede usar:

`docs: mapear misiones y proponer guion alineado al prototipo`

Este commit documenta una propuesta y un plan de pruebas; **no debe afirmar que las misiones nuevas ya se probaron ni que los textos están integrados**.
