using UnityEngine;
using TMPro;

public static class UITextFileLoader
{
    private const string BASE_PATH = "Textos/";

    public static void CambiarTextoDesdeFichero(GameObject objetoPadre, TextoID idTexto)
    {
        string textoPorDefecto = "No hay texto disponible.";

        if (objetoPadre == null)
        {
            Debug.LogError("El objetoPadre es null.");
            return;
        }

        TMP_Text textoTMP = objetoPadre.GetComponent<TMP_Text>();
        if (textoTMP == null)
        {
            textoTMP = objetoPadre.GetComponentInChildren<TMP_Text>(true);
        }

        if (textoTMP == null)
        {
            Debug.LogError($"No se encontró ningún TMP_Text en {objetoPadre.name} ni en sus hijos.");
            return;
        }

        string ruta = BASE_PATH + idTexto.ToString();
        TextAsset fichero = Resources.Load<TextAsset>(ruta);

        if (fichero == null)
        {
            Debug.LogWarning($"No se encontró {ruta}. Usando texto por defecto.");
            textoTMP.text = textoPorDefecto;
            return;
        }

        if (string.IsNullOrWhiteSpace(fichero.text))
        {
            Debug.LogWarning($"El fichero {idTexto} está vacío. Usando texto por defecto.");
            textoTMP.text = textoPorDefecto;
            return;
        }

        textoTMP.text = fichero.text;
    }
}