# YAKU: Sangre de Metal — Resultados de pruebas funcionales

**Proyecto:** YAKU: Sangre de Metal  
**Responsable de ejecución:** Denis Prudencio  
**Fecha de registro:** 27/11/2026 (Perú)  
**Repositorio:** https://github.com/LuisanaNapan/Yaku  
**Escena evaluada:** `Assets/_Yaku/Scenes/01_Community.unity`  
**Editor de Unity:** `6000.6.9f1 (8c45277fb9c5)`  
**Tipo de pruebas:** manuales, funcionales y exploratorias en equipo local  
**Estado del documento:** registro inicial basado en la comprobación comunicada por quien realizó las pruebas.

## 1. Objetivo

Comprobar que el recorrido del capítulo comunitario se puede ejecutar y avanzar por sus misiones, incluidos los diálogos, las interacciones, los objetivos y el cierre, sin identificar fallas bloqueantes durante esta revisión.

## 2. Alcance y criterio

Los resultados «Aprobado» de la tabla son **observaciones reportadas por el evaluador durante una ejecución local**, no resultados de pruebas automatizadas ni una certificación de ausencia de defectos. El diagnóstico de apertura y la primera misión se confirmaron por separado; posteriormente el evaluador indicó que las restantes misiones estaban funcionando correctamente.

## 3. Resultados observados

| ID | Caso probado | Resultado comunicado | Observación |
| --- | --- | --- | --- |
| QA-00 | Apertura de `01_Community`, visualización del entorno y de la interfaz | Aprobado | Escena, materiales y controles observados como funcionales en revisión inicial. |
| QA-01 | Tutorial «El primer encargo»: hablar con Elena, recoger paquete y entregarlo a Tomás | Aprobado | El evaluador confirmó que pudo completarlo. |
| QA-02 | «El peligro invisible»: consultar aviso de monitoreo y elegir fuente de agua verificada | Aprobado | El evaluador confirmó explícitamente su finalización. |
| QA-03 | «Elegir con información»: consultar procedencia y seleccionar alimento acorde con la ficha | Aprobado (reportado) | Confirmación general de funcionamiento de las misiones. |
| QA-04 | «Atender y orientar»: solicitar orientación sanitaria y elegir apoyo básico | Aprobado (reportado) | Confirmación general de funcionamiento de las misiones. |
| QA-05 | «Una comunidad preparada»: coordinar y completar distribución comunitaria | Aprobado (reportado) | Confirmación general de funcionamiento de las misiones. |
| QA-06 | «Cierre del capítulo»: escuchar el diálogo final y acceder al diario | Aprobado (reportado) | Confirmación general de funcionamiento de las misiones. |

**Resultado global de la ronda:** recorrido principal reportado como funcional; **no se comunicaron errores bloqueantes** en estas pruebas. No se afirma que el juego esté libre de errores o que todos los casos extremos hayan sido cubiertos.

## 4. Verificaciones pendientes de documentar

Estas pruebas complementarias **no se marcan como aprobadas sin un registro de ejecución específico**:

- Responder incorrectamente a una decisión y comprobar que es posible reintentar sin bloquear el progreso.
- Abrir y cerrar el diario con `J` durante diferentes etapas y después de completar el capítulo.
- Verificar la evolución del inventario con mochila llena o al recoger objetos repetidos.
- Probar colisiones, límites de escenario, caída del jugador y objetos fuera de alcance.
- Revisar interfaz y legibilidad en diferentes resoluciones.
- Confirmar el comportamiento después de salir de Play y volver a iniciarlo.
- Guardar capturas o video del final del capítulo y de los estados relevantes como evidencia adicional.

## 5. Evidencias y trazabilidad

- **Evidencia inicial:** captura del editor con la escena `01_Community`, jerarquía, escenario y Console, compartida en la revisión del equipo (adjuntar al repositorio si se dispone del archivo).
- **Evidencia de las misiones:** validación manual comunicada por Denis; capturas adicionales de cada misión pendientes de adjuntar.
- **Código y escena:** repositorio `LuisanaNapan/Yaku`, rama base `main` en la fecha de revisión; los resultados dependen de los cambios efectivos presentes en la copia local probada.

## 6. Hallazgos y recomendaciones

1. La escena inicial y el recorrido principal constituyen una base funcional adecuada para la siguiente fase de trabajo narrativo y de ambientación.
2. Conviene mantener las mejoras de guion y documentación separadas de las escenas y scripts de programación para evitar conflictos con otras integrantes.
3. La nomenclatura del GDD menciona a **María**, mientras el prototipo utiliza una NPC llamada **Rosa**. El equipo debe acordar si se mantiene o se cambia el nombre antes de ajustar diálogos definitivos.
4. Los recursos externos y archivos de Unity que ya aparecen modificados localmente deben revisarse antes de seleccionarlos para un commit; el presente informe no requiere incluirlos.

## 7. Próximos pasos

1. Adjuntar evidencias visuales adicionales cuando estén disponibles.
2. Proponer una revisión narrativa ajustada a los capítulos efectivamente jugables.
3. Validar con el equipo el nombre y rol definitivo de los NPC.
4. Realizar pruebas de regresión tras integrar mejoras de narración o ambientación.

---

*Registro de QA elaborado como documentación de trabajo grupal. No se realizaron cambios de código ni de escena al preparar este informe.*
