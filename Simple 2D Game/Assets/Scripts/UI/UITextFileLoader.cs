using UnityEngine;
using TMPro;
using System.Collections.Generic;

public static class UITextFileLoader
{
    private const string BASE_PATH = "Textos/";

    public static void ChangeTextFromFile(GameObject objetoPadre, TextoID idTexto)
    {
        string textoPorDefecto = "There is no text available.";

        if (objetoPadre == null)
        {
            Debug.LogError("The objetoPadre is null.");
            return;
        }

        TMP_Text textoTMP = objetoPadre.GetComponent<TMP_Text>();
        if (textoTMP == null)
        {
            textoTMP = objetoPadre.GetComponentInChildren<TMP_Text>(true);
        }

        if (textoTMP == null)
        {
            Debug.LogError($"No TMP_Text found on {objetoPadre.name} or its children.");
            return;
        }

        string ruta = BASE_PATH + idTexto.ToString();
        TextAsset fichero = Resources.Load<TextAsset>(ruta);

        if (fichero == null)
        {
            Debug.LogWarning($"File not found: {ruta}. Using default text.");
            textoTMP.text = textoPorDefecto;
            return;
        }

        if (string.IsNullOrWhiteSpace(fichero.text))
        {
            Debug.LogWarning($"The file {idTexto} is empty. Using default text.");
            textoTMP.text = textoPorDefecto;
            return;
        }

        textoTMP.text = fichero.text;
    }



    public static string[] LoadLinesFromFile(TextoID idTexto)
    {
        string ruta = BASE_PATH + idTexto.ToString();
        TextAsset fichero = Resources.Load<TextAsset>(ruta);

        if (fichero == null)
        {
            Debug.LogWarning($"File not found: {ruta}. Returning default text.");
            return new string[] { "Narrator|There is no text available." };
        }

        if (string.IsNullOrWhiteSpace(fichero.text))
        {
            Debug.LogWarning($"The file {idTexto} is empty. Returning default text.");
            return new string[] { "Narrator|There is no text available." };
        }

        string[] lineas = fichero.text.Split('\n');
        List<string> resultado = new List<string>();

        foreach (string linea in lineas)
        {
            string limpia = linea.Trim();

            if (string.IsNullOrEmpty(limpia))
                continue;

            resultado.Add(limpia);
        }

        return resultado.ToArray();
    }

}