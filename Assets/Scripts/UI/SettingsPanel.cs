using UnityEngine;

public class SettingsPanel : MonoBehaviour 
{
    private bool isEnabled = false;

    public void EnablePanel()
    {
        isEnabled = !isEnabled;
        gameObject.SetActive(isEnabled);
    }
}
