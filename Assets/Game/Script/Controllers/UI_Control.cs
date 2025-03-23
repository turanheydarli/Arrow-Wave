using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    public Screen_Touch Screen_Touch;
    public Loading_Screen Loading_Screen;
    public Home_Screen Home_Screen;
    public Game_Screen Game_Screen;
    public Level_Completed_Screen Level_Completed_Screen;
    public Level_Failed_Screen Level_Failed_Screen;
    public Text Coins_Indicator;
    public Tutorial_Elements Tutorial_Elements;


    public void Play_Confetti_Level_Part(){
        // Confetti_Level_Particles.Play();
    }
    
    // Game Screen ----------------------------------------
    public void Update_All(){
        Game_Screen.Update_Screen();
    }

}
