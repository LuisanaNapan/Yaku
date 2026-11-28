# YAKU: SANGRE DE METAL
## Propuesta de guion integrado al prototipo — versión 1

**Responsable de propuesta:** Denis Prudencio  
**Estado:** Propuesta para revisión grupal; NO integrada en Unity  
**Base técnica:** repositorio `LuisanaNapan/Yaku`, escena `Assets/_Yaku/Scenes/01_Community.unity`  
**Ámbito:** tutorial y primer capítulo jugable (agua, alimentos, salud, distribución y cierre)

> Este documento distingue el contenido actual comprobado en los archivos del repositorio de las propuestas literarias. Ninguno de los diálogos propuestos se considera implementado hasta que el equipo lo apruebe y se actualicen los assets correspondientes.

## 1. Sinopsis narrativa del prototipo

En una pequeña comunidad amazónica inspirada en Huepetuhe, el río ya no ofrece la seguridad que antes parecía dar. La minería ha cambiado el paisaje y las familias temen que aquello que llevan a sus hogares —el agua y los alimentos— pueda perjudicar su salud. Yaku es un joven promotor comunitario que quiere ayudar, aunque todavía no entiende cómo diferenciar una precaución necesaria de un rumor.

Elena, promotora de salud, le encomienda una tarea sencilla: llevar suministros al vecino Tomás. Es el primer paso de una cadena de decisiones más complejas. Una familia necesita agua; Yaku debe consultar un aviso de monitoreo, comparar recipientes y elegir una fuente cuyo abastecimiento esté documentado. Más tarde, Rosa le pide que averigüe qué alimentos se pueden distribuir atendiendo a su procedencia y a los avisos disponibles. Cuando Tomás se hiere ordenando cajas, el joven comprende que atender una lesión superficial no es lo mismo que evaluar una posible exposición a metales pesados.

Al final, la comunidad reúne paquetes de agua segura, alimentos documentados y apoyo básico para distribuirlos según las necesidades. Yaku no descubre una cura milagrosa ni resuelve la contaminación de un día para otro. Descubre algo más honesto y valioso: la prevención requiere pruebas, profesionales de salud y colaboración. El río sigue allí, pero la comunidad ya no mira hacia otro lado.

**Idea dramática:** Yaku empieza creyendo que ayudar significa actuar rápido; termina comprendiendo que ayudar bien significa escuchar, comprobar y actuar en conjunto.

## 2. Lo que ya existe y lo que NO se debe inventar como implementado

**Existe en el prototipo:** primera entrega a Tomás; consulta de aviso de agua; selección de fuente; información sobre alimentos y lotes; orientación ante heridas y sospecha de exposición; distribución de tres paquetes; diario y cierre. Estas acciones están representadas en los archivos de código y la escena; deben validarse en una partida completa.

**No forma parte del primer capítulo implementado:** tormenta, rescate de Tomás en una estructura dañada, viaje por un depósito remoto, mecánica de plantas curativas, múltiples finales. Estos elementos pertenecen a una posible expansión narrativa y NO deben presentarse como jugables en la exposición actual.

## 3. Reglas para adaptar el guion a Unity

- La clase `DialogueData` muestra **un único nombre de hablante** por asset y una lista de líneas. Los textos propuestos debajo respetan ese formato: cada bloque corresponde a un solo hablante. Para alternar líneas entre Yaku y otro personaje en una misma conversación se requeriría una ampliación del sistema, que debe coordinarse con Alexandra.
- Mantener por ahora **Rosa**, nombre que figura en los assets y en la escena. El GDD anterior llama **María** a la conocedora de saberes tradicionales: el grupo debe decidir si se trata del mismo personaje y unificarlo.
- Conservar el orden actual de objetivos y sus interacciones. La propuesta solo cambia tono, continuidad y claridad de la historia.
- Los avisos de fuentes y lotes son **ficticios dentro de la simulación**. No se deben presentar como datos reales de potabilidad o seguridad alimentaria en Huepetuhe.
- No afirmar que hervir o filtrar sedimentos elimina el mercurio; tampoco que un remedio tradicional trata la intoxicación por mercurio.

## 4. Guion propuesto: prólogo y recorrido por misiones

### PRÓLOGO — «El primer encargo» (Tutorial)

**Intención dramática:** introducir a Yaku como alguien dispuesto a ayudar; establecer confianza en Elena y Tomás.  
**Gameplay actual:** hablar con Elena → recoger paquete en almacén → entregarlo a Tomás → abrir embarcadero y diario.  
**Asset actual:** `Narrative/Dialogues/Elena.asset`.

**Elena — propuesta de líneas para su diálogo inicial:**
1. «Yaku, me alegra verte. Estamos organizando provisiones para las familias y hoy necesitamos otra mano.»
2. «En el almacén hay un paquete reservado para Tomás. Recógelo y entrégaselo junto al sendero; así podremos continuar con las demás necesidades.»
3. «Parece un encargo pequeño, pero cada entrega cuenta cuando los recursos son limitados.»

**Tomás — propuesta para interacción general (`Tomas.asset`, sin interferir con la entrega programada):**
1. «Buenos días, Yaku. Vivo junto al sendero. Si hace falta trasladar provisiones, puedo colaborar.»
2. «Últimamente todos preguntamos lo mismo: ¿qué agua podemos llevar a casa? Elena sabrá orientarte mejor.»

**Transición narrativa sugerida (texto de presentación o diario, no implementado):** «Una entrega parece suficiente para empezar. Pero el verdadero problema es saber qué recursos son seguros.»

### MISIÓN 1 — «El peligro invisible» (Agua)

**Intención dramática:** de la incertidumbre a una decisión respaldada por información.  
**Gameplay actual:** hablar con Elena → leer aviso en ribera → comparar recipientes → informar a Elena.  
**Assets a revisar:** `WaterBriefing.asset`, `RiverMonitoring.asset`, `WaterReport.asset`. La decisión se configura en el objeto `Water Selection` de la escena.

**Elena — `WaterBriefing.asset`:**
1. «Una familia se ha quedado sin agua segura. No podemos entregarles cualquier recipiente solo porque se vea limpio.»
2. «Junto al sendero de la ribera encontrarás un aviso de monitoreo de esta simulación. Léelo antes de escoger.»
3. «Quédate en el sendero: no necesitas tocar ni entrar en el río para completar la revisión.»

**Aviso de monitoreo — `RiverMonitoring.asset`:**
1. «SIMULACIÓN: el punto de abastecimiento comunitario tiene verificación para esta misión. El recipiente procedente del río no cuenta con resultados vigentes.»
2. «La claridad del agua no permite determinar si contiene mercurio. Consulta información y orientaciones sanitarias verificadas.»

**Elena — `WaterReport.asset`:**
1. «Elegiste una fuente respaldada por el aviso. Esa decisión puede evitar una exposición innecesaria.»
2. «Anotemos el origen del recipiente y la fecha de la información. No basta con confiar en el aspecto del agua.»
3. «La contaminación no desaparecerá hoy, Yaku. Pero podemos proteger mejor a las familias mientras exigimos respuestas y atención.»

**Nota de diseño:** conservar la respuesta correcta ya configurada: **segunda alternativa, abastecimiento verificado**.

### MISIÓN 2 — «Elegir con información» (Alimentos)

**Intención dramática:** mostrar que la prevención también exige revisar qué comemos.  
**Gameplay actual:** hablar con Rosa → leer ficha → comparar tres lotes → justificar elección ante Rosa.  
**Assets:** `FoodBriefing.asset`, `FoodAdvisory.asset` y objetos `Food Selection`, `Food Reason`.

**Rosa — `FoodBriefing.asset`:**
1. «Necesito preparar comida para las familias. Trajeron varios lotes, pero no todos tienen información suficiente sobre su origen.»
2. «Dejé una ficha junto a mi casa. Léela antes de decidir qué llevaremos.»
3. «El mercurio puede acumularse en los peces y avanzar por la cadena alimentaria. Por eso importa conocer la procedencia y atender los avisos vigentes.»

**Ficha — `FoodAdvisory.asset`:**
1. «SIMULACIÓN: Lote A, origen sin documentar. Lote B, procedencia documentada y recomendación favorable en la ficha ficticia. Lote C, restricción vigente.»
2. «La apariencia, el olor y la cocción no permiten confirmar la ausencia de mercurio. En la vida real se consultan recomendaciones sanitarias actualizadas.»

**Rosa — propuesta para diálogo general (`Rosa.asset`):**
1. «Antes de compartir un alimento, debemos saber de dónde viene. No todo el riesgo se puede ver.»
2. «Traer provisiones ayuda; traer provisiones con información confiable ayuda mucho más.»

**Nota de diseño:** conservar la selección **lote B (segunda alternativa)** y la justificación **ficha vigente (segunda alternativa)**.

### MISIÓN 3 — «Atender y orientar» (Salud)

**Intención dramática:** mostrar el límite entre ayudar y diagnosticar sin preparación.  
**Gameplay actual:** conversar con Tomás sobre una herida y otra consulta → informar a Elena → seleccionar apoyo de la posta → confirmar orientación profesional.  
**Assets:** `HealthIncident.asset`, `HealthGuidance.asset` y objetivos `Health Supplies`, `Health Referral`.

**Tomás — `HealthIncident.asset`:**
1. «Yaku, me corté el brazo al acomodar unas cajas. No parece grave, pero necesito que me acompañes a la posta.»
2. «Además, otra familia está preocupada por una posible exposición. No sabemos qué ocurrió y necesitamos orientación.»

**Elena — `HealthGuidance.asset`:**
1. «Gracias por acompañar a Tomás. Me encargaré de valorar su herida y te indicaré el paquete de apoyo que necesitamos.»
2. «La consulta de la otra familia es diferente. Anotemos lo que observaron, cuándo ocurrió y con qué estuvieron en contacto.»
3. «Una sospecha de exposición a mercurio necesita evaluación profesional. No puede diagnosticarse solo mirando los síntomas.»

**Nota de diseño:** conservar la decisión de escoger **el paquete básico señalado por la posta (primera opción)** y **buscar orientación profesional (tercera opción)**.

### MISIÓN 4 — «Una comunidad preparada» (Distribución)

**Intención dramática:** pasar de decisiones individuales a organización y solidaridad.  
**Gameplay actual:** coordinar con Elena → repartir tres paquetes → confirmar que las elecciones se basaron en evidencia.  
**Assets:** `DistributionPlan.asset` y objetivos `Distribution Route`, `Distribution Confirmation`.

**Elena — `DistributionPlan.asset`:**
1. «El almacén ha reservado tres paquetes. Tomás necesita dos bidones del punto verificado; Rosa recibirá el alimento documentado.»
2. «También debemos llevar un paquete básico a la posta. Antes de salir, revisa a quién corresponde cada entrega.»
3. «No se trata de repartir a ciegas: necesitamos tener en cuenta las necesidades y registrar qué recibió cada familia.»

**Nota de diseño:** al confirmar, la respuesta correcta es **la fuente verificada según el aviso ficticio (primera opción)**.

### EPÍLOGO — «Cierre del capítulo»

**Intención dramática:** final esperanzador sin fingir que la contaminación ha desaparecido.  
**Gameplay actual:** escuchar el reconocimiento de Elena y revisar diario.  
**Asset:** `ChapterClosing.asset`.

**Elena — `ChapterClosing.asset`:**
1. «Hoy hiciste más que llevar paquetes, Yaku. Escuchaste a tus vecinos, buscaste información y ayudaste a tomar decisiones responsables.»
2. «El problema no termina aquí. El río todavía necesita vigilancia, las familias necesitan atención y la comunidad debe seguir organizada.»
3. «Pero ahora sabemos algo importante: nadie tiene por qué enfrentar el peligro en soledad.»
4. «Dejé registradas tus observaciones en el diario. Volvamos a consultarlas cuando tengamos que decidir de nuevo.»

**Última frase sugerida para pantalla final (requiere UI adicional, no implementada):**  
«A veces proteger la vida no empieza con una respuesta. Empieza con la decisión de comprobarla.»

## 5. Prioridades de integración

1. **Aprobación grupal:** confirmar nombres, tono de las conversaciones, fuentes médicas y el alcance del prototipo.
2. **Compatibilidad:** Alexandra confirma si es posible ampliar la cantidad de líneas sin generar una experiencia pesada o incompatibilidades.
3. **Edición controlada:** actualizar solo `DialogueData` seleccionados y sus `.meta` cuando realmente corresponda; editar los `.asset` mediante el Inspector de Unity, no manualmente por fuera.
4. **Prueba:** recorrer todas las misiones, verificar que cada conversación avance y que el texto no se desborde en pantalla.
5. **Git:** integrar mediante commit separado y revisión, no mezclado con modificaciones de escenas que estén editando las compañeras.

## 6. Dependencias y solicitudes al equipo

- **Alexandra:** confirmar el límite práctico de líneas, el uso de `DialogueData`, el orden de los objetivos y el estado de la implementación.
- **Rosa (integrante de diseño):** confirmar nombres oficiales de NPC y recorridos; decidir si el personaje Rosa se unifica con María del GDD.
- **Luisana:** revisar espacio de los cuadros de diálogo, tipografía, retratos y uso de sonido ambiental.
- **Denis:** entregar y revisar esta propuesta, verificar consistencia con ODS 3, preparar evidencias de QA y coordinar cambios narrativos.

**Fuente de implementación para revisión grupal:** https://github.com/LuisanaNapan/Yaku
