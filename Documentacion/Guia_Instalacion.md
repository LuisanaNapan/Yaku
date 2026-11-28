# Guía de instalación y ejecución — YAKU

## 1. Requisitos

- Computadora con Windows para la demostración prevista.
- Unity Hub.
- Unity Editor 6000.6.9f1.
- Git o GitHub Desktop.
- Espacio disponible para los recursos del proyecto.
- Conexión a Internet para descargar dependencias.

## 2. Repositorio

https://github.com/LuisanaNapan/Yaku

## 3. Clonación mediante Git

Abrir una terminal y ejecutar:

    git clone https://github.com/LuisanaNapan/Yaku.git
    cd Yaku
    git checkout main
    git pull origin main

La rama main contiene las contribuciones que han sido integradas por el equipo.

## 4. Apertura en Unity

1. Abrir Unity Hub.
2. Seleccionar la opción para añadir un proyecto desde el disco.
3. Localizar la carpeta clonada.
4. Seleccionar YakuUnity/Yaku.
5. Utilizar la versión de Unity indicada en ProjectSettings/ProjectVersion.txt.
6. Esperar la importación de recursos y dependencias.
7. Revisar la consola de Unity.

## 5. Ejecución

Localizar la escena:

`Assets/_Yaku/Scenes/01_Community.unity`

Abrirla y activar Play Mode para realizar una prueba del capítulo comunitario.

## 6. Verificación inicial

- Comprobar que el escenario se visualice correctamente.
- Revisar los controles del personaje.
- Interactuar con los NPC.
- Comprobar los objetivos y el inventario.
- Verificar los diálogos y audios disponibles en la versión instalada.

## 7. Generación del ejecutable

Utilizar las opciones de compilación de Unity para Windows y verificar las escenas incluidas en el perfil de compilación.

El proyecto incorpora el script de editor YakuWindowsBuild.cs como recurso relacionado con la generación de versiones Windows.

## 8. Solución de problemas

Si existen errores de importación, comprobar la versión del editor, las dependencias y los archivos descargados.

Si faltan audios, revisar que la rama instalada incluya la integración narrativa y sus respectivos archivos .meta.

## 9. Actualización del proyecto

Antes de actualizar, guardar o respaldar cualquier modificación local.

    git fetch origin
    git status
    git pull origin main

Ejecutar la actualización desde main y únicamente cuando los cambios locales estén protegidos.

**Elaboración documental:** Denis Prudencio.
