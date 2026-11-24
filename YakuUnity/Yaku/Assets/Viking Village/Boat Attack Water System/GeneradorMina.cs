using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GeneradorMina : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Herramientas/Crear Mina 3D")]
    public static void CrearMina()
    {
        GameObject minaRoot = new GameObject("Mina_Vikinga");
        minaRoot.transform.position = SceneView.lastActiveSceneView != null
            ? SceneView.lastActiveSceneView.pivot
            : Vector3.zero;

        float largoTunel = 15f;
        float ancho = 3.5f;
        float alto = 3.2f;

        // 1. Suelo, techo y paredes del tunel
        CrearBloque("Suelo_Mina", new Vector3(0, -0.25f, largoTunel / 2f), new Vector3(ancho + 1f, 0.5f, largoTunel), minaRoot.transform, new Color(0.25f, 0.23f, 0.22f));
        CrearBloque("Techo_Mina", new Vector3(0, alto + 0.25f, largoTunel / 2f), new Vector3(ancho + 1.5f, 0.8f, largoTunel), minaRoot.transform, new Color(0.22f, 0.20f, 0.19f));
        CrearBloque("Pared_Izquierda", new Vector3(-(ancho / 2f) - 0.5f, alto / 2f, largoTunel / 2f), new Vector3(1f, alto, largoTunel), minaRoot.transform, new Color(0.28f, 0.26f, 0.24f));
        CrearBloque("Pared_Derecha", new Vector3((ancho / 2f) + 0.5f, alto / 2f, largoTunel / 2f), new Vector3(1f, alto, largoTunel), minaRoot.transform, new Color(0.28f, 0.26f, 0.24f));
        CrearBloque("Pared_Fondo", new Vector3(0, alto / 2f, largoTunel), new Vector3(ancho + 2f, alto + 1f, 1f), minaRoot.transform, new Color(0.20f, 0.18f, 0.18f));

        // 2. Vigas de madera cada 3 metros + rieles
        Color colorMadera = new Color(0.32f, 0.18f, 0.08f);
        Color colorMetal = new Color(0.15f, 0.15f, 0.17f);

        for (float z = 0.5f; z < largoTunel - 1f; z += 3f)
        {
            GameObject arco = new GameObject($"Marco_Madera_{z}");
            arco.transform.SetParent(minaRoot.transform, false);
            arco.transform.localPosition = new Vector3(0, 0, z);

            CrearBloque("Poste_Izq", new Vector3(-(ancho / 2f) + 0.15f, alto / 2f, 0), new Vector3(0.3f, alto, 0.3f), arco.transform, colorMadera);
            CrearBloque("Poste_Der", new Vector3((ancho / 2f) - 0.15f, alto / 2f, 0), new Vector3(0.3f, alto, 0.3f), arco.transform, colorMadera);
            CrearBloque("Viga_Superior", new Vector3(0, alto - 0.15f, 0), new Vector3(ancho, 0.3f, 0.35f), arco.transform, colorMadera);
        }

        // 3. Rieles de vagoneta en el piso
        CrearBloque("Riel_Izq", new Vector3(-0.6f, 0.05f, largoTunel / 2f), new Vector3(0.08f, 0.1f, largoTunel - 0.5f), minaRoot.transform, colorMetal);
        CrearBloque("Riel_Der", new Vector3(0.6f, 0.05f, largoTunel / 2f), new Vector3(0.08f, 0.1f, largoTunel - 0.5f), minaRoot.transform, colorMetal);

        for (float z = 0.4f; z < largoTunel - 0.5f; z += 0.7f)
        {
            CrearBloque("Traviesa_Madera", new Vector3(0, 0.02f, z), new Vector3(1.6f, 0.05f, 0.18f), minaRoot.transform, colorMadera);
        }

        // 4. Luz calida interior
        GameObject luzObj = new GameObject("Luz_Interior_Mina");
        luzObj.transform.SetParent(minaRoot.transform, false);
        luzObj.transform.localPosition = new Vector3(0, alto - 0.6f, largoTunel * 0.4f);
        Light luz = luzObj.AddComponent<Light>();
        luz.type = LightType.Point;
        luz.color = new Color(1f, 0.65f, 0.25f);
        luz.range = 14f;
        luz.intensity = 3f;

        Selection.activeGameObject = minaRoot;
        Debug.Log("Mina 3D creada en la escena.");
    }

    private static void CrearBloque(string nombre, Vector3 posLocal, Vector3 escala, Transform padre, Color color)
    {
        GameObject cubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubo.name = nombre;
        cubo.transform.SetParent(padre, false);
        cubo.transform.localPosition = posLocal;
        cubo.transform.localScale = escala;

        Renderer rend = cubo.GetComponent<Renderer>();
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLit != null)
        {
            Material mat = new Material(urpLit);
            mat.color = color;
            rend.sharedMaterial = mat;
        }
    }
#endif
}