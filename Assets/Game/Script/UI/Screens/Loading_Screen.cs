using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading_Screen : MonoBehaviour, Screen_0_Callback
{

    public bool Start_Black = false;
    public float Fade_Duration = 0.25f;
    public bool Prevent_Touch = true; // while black
    private Image _Fade_Image;
    private Image Fade_Image{
        get{
            if(!_Fade_Image){
                _Fade_Image = GetComponent<Image>();
            }
            return _Fade_Image;
        }
    }

    void Awake(){
        Fade_Image.enabled = true;   

        if(Start_Black == true){
            Fade_Image.CrossFadeAlpha(1.0f,0f,false);
            Set_Prevent_Touch(true);
        }else{
            Fade_Image.CrossFadeAlpha(0.0f,0f,false);
            Set_Prevent_Touch(false);
        }
    }

    public void Show(){
        Fade_Image.CrossFadeAlpha(1.0f,Fade_Duration,false);
        Set_Prevent_Touch(true);
    }
    public void Show(float fade_duration){
        Fade_Image.CrossFadeAlpha(1.0f,fade_duration,false);
        Set_Prevent_Touch(true);
    }


    public void Hide(){
        Fade_Image.CrossFadeAlpha(0.0f,Fade_Duration,false);
        Set_Prevent_Touch(false);
    }
    public void Hide(float fade_duration){
        Fade_Image.CrossFadeAlpha(0.0f,fade_duration,false);
        Set_Prevent_Touch(false);
    }



    private void Set_Prevent_Touch(bool prevent){
        if(prevent == true){
            if(Prevent_Touch==true){
                Fade_Image.raycastTarget = true;
            }
        }else{
            Fade_Image.raycastTarget = false;
        }

    }

    
}
