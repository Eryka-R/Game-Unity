using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; //Sound or Music
    private TMP_Text txt;

    private void Awake() {
        txt = GetComponent<TMP_Text>();

        if (txt == null)
        {
            Debug.LogError($"No TMP_Text component found on {gameObject.name}");
        }
    }

    private void Update() {
        UpdateVolume();
    }

    private void UpdateVolume() {
        txt.text = $"{textIntro} {Mathf.RoundToInt(PlayerPrefs.GetFloat(volumeName) * 100)}%";
        // txt.text = $"{textIntro}: {(int)(PlayerPrefs.GetFloat(volumeName) * 100)}%" ;
    }
}

