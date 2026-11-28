# YAKU: Sangre de Metal — Mejoras de diálogo del capítulo «El peligro invisible»

**Autor de propuesta narrativa:** Denis Prudencio Ríos  
**Estado:** Propuesta preparada para prueba local, pendiente de revisión y aprobación del equipo.  
**Base técnica:** `LuisanaNapan/Yaku`, escena `Assets/_Yaku/Scenes/01_Community.unity` (proyecto ubicado en `YakuUnity/Yaku`).  
**Alcance:** cambios de texto en tres `DialogueData`; ninguna modificación de escenas, scripts ni misiones.

## Objetivo narrativo

Elena se expresa con urgencia y criterio profesional, sin sonar como un manual. El aviso conserva su naturaleza documental y explícitamente ficticia. El cierre reconoce el esfuerzo de Yaku sin presentar una solución mágica a la contaminación.

## Escena A: Elena encarga la inspección (`WaterBriefing.asset`)

**Contexto:** después del tutorial, Yaku conversa con Elena en la posta.  
**Intención:** hacer que la necesidad de agua de una familia impulse la misión.  
**Emoción:** preocupación contenida y confianza en el jugador.

**Interlocutor:** Elena

1. Yaku, una familia espera agua. No podemos adivinar si es segura.
2. El río parece el mismo de siempre, pero eso no es una prueba.
3. Revisa el aviso del sendero y observa el agua desde el mirador.
4. No te acerques a la orilla. Regresa con datos, no con suposiciones.


## Escena B — Cartel de monitoreo de la ribera

**Contexto:** el jugador lee el cartel junto al sendero, sin tocar el río. El aviso es información simulada, no un reporte real.

**Interlocutor:** Aviso de monitoreo · Simulación

1. SIMULACIÓN: el punto comunitario tiene una verificación vigente.
2. El agua del río no cuenta con resultados vigentes en esta misión.
3. El mercurio no puede identificarse a simple vista.
4. Hervir o filtrar sedimentos no garantiza eliminar el mercurio.
5. Para casos reales, consulta avisos de las autoridades competentes.


## Escena C — Regreso a la posta

**Contexto:** tras elegir la fuente correcta, Yaku vuelve a conversar con Elena.

**Interlocutor:** Elena

1. Volviste con el aviso y el origen del recipiente. Bien hecho.
2. En esta misión, la fuente comunitaria está verificada.
3. Registremos la procedencia antes de entregar el agua a la familia.
4. No resolvimos la contaminación. Pero evitamos decidir a ciegas.
5. Ante una duda real, seguiremos los avisos y pediremos apoyo.

## Relación con las mecánicas actuales

1. `WaterBriefing.asset` abre la primera conversación con Elena y permite continuar al siguiente objetivo cuando se cierra el diálogo.
2. `RiverMonitoring.asset` presenta el dato del aviso ficticio que el jugador necesita para la selección.
3. La elección entre fuentes sigue configurada en el componente `ChapterObjective` de la escena; **no se cambia** el índice de respuesta correcta ni el cuestionario.
4. `WaterReport.asset` aparece al regresar con Elena y cierra la misión.

## Límites técnicos considerados

- Cada `DialogueData` tiene un único campo `speaker` para toda la conversación. No se intercalan frases de Yaku dentro del mismo recurso.
- El panel de diálogo tiene tamaño fijo y usa texto de tamaño considerable. Se mantienen frases cortas y una idea por línea; hay que comprobar visualmente en Play que no haya cortes.
- Los recursos `.asset.meta` originales deben conservarse sin cambios: guardan los GUID usados por las escenas.
- No se modifican `CommunityChapter.cs`, `DialogueController.cs`, la escena ni otros archivos de Alexandra.
- Antes de integrarlo a `main`, el equipo debe aprobar la redacción y verificar el capítulo en Unity.

## Prueba de aceptación sugerida

- [ ] Abrir `01_Community` sin errores de Console.
- [ ] Completar el tutorial hasta activar «El peligro invisible».
- [ ] Hablar con Elena; comprobar que aparecen las cuatro líneas nuevas en orden.
- [ ] Leer el aviso y comprobar que sus cinco líneas se ven enteras.
- [ ] Seleccionar el recipiente de la fuente verificada, sin cambiar la respuesta correcta.
- [ ] Volver con Elena y comprobar las cinco líneas finales.
- [ ] Confirmar la misión completada y la activación de la misión de alimentos.
- [ ] Revisar GitHub Desktop: solo tres `.asset` y este documento; nunca archivos `.tif` grandes.

## Nota de rigor educativo

El agua transparente no implica ausencia de mercurio. La filtración de sedimentos y la ebullición no garantizan eliminar mercurio disuelto. La verificación de la fuente se refiere exclusivamente al aviso ficticio de esta misión; para situaciones reales deben utilizarse evaluaciones y avisos oficiales.

## Continuidad futura

El siguiente bloque narrativo puede abordar «Elegir con información», manteniendo el personaje Rosa provisionalmente hasta que el grupo acuerde si usará ese nombre o el de María del GDD.
