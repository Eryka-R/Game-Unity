using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private RectTransform rect;
    private int currentPosition;

    private void Awake() {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Change position of the señection arrow
        if (Keyboard.current == null) return;
        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            ChangePosition(-1);
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            ChangePosition(1);
        }

        // Interact with opciones
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void ChangePosition(int _change){
        currentPosition += _change;

        if (_change != 0){
            SoundManager.instance.PlaySound(changeSound);
        }

        if(currentPosition < 0){
            currentPosition = options.Length - 1;
        } else if(currentPosition > options.Length - 1){
            currentPosition = 0;
        }

        rect.position = new Vector3(rect.position.x, options[currentPosition].position.y, 0);
    }

    private void Interact() {
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
        SoundManager.instance.PlaySound(interactSound);
    }
}
