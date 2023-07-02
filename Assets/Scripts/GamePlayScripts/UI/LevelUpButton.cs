using UnityEngine;
using UnityEngine.EventSystems;


public class LevelUpButton : MonoBehaviour
{
    public static string ClickedButtonName = "Nun";
    public void ButtonPressed()
    {
        ClickedButtonName = EventSystem.current.currentSelectedGameObject.name;
    }
}
