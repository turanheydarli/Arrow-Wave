using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress_Bar : MonoBehaviour {

    public Image Progress_Fill;

    public void Show(){
        gameObject.SetActive(true);
        Set_Progress(0f);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Set_Progress(float ratio) {
        Progress_Fill.fillAmount = ratio;
    }

}
