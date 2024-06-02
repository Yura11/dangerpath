using UnityEngine;
using UnityEngine.UI;

public class volume2 : MonoBehaviour
{
    public Button muteButton;
    public Button unmuteButton;
   

    private void Start()
    {
        if (menu.Buttonstate!=true)
        {
            ToggleMute();
        }
        else if(menu.Buttonstate != false)
        {
            ToggleunMute();
        }
    }

    public void ToggleunMute()
    {
        menu.Buttonstate = true;
        AudioListener.volume = 1f;
        muteButton.gameObject.SetActive(true);
        unmuteButton.gameObject.SetActive(false);
    }

    public void ToggleMute()
    {
        menu.Buttonstate = false;
        AudioListener.volume = 0f;
        muteButton.gameObject.SetActive(false);
        unmuteButton.gameObject.SetActive(true);
    }
}


