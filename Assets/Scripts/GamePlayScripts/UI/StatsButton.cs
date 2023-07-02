
using UnityEngine;
using UnityEngine.EventSystems;

public class StatsButton : MonoBehaviour
{
    [SerializeField] GameObject PlayerStats;
    public void ButtonPressed()
    {
        PlayerStats.SetActive(!PlayerStats.activeSelf);
    }
}
