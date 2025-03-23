using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section_Control : MonoBehaviour
{
    public Section Loader_Section;
    public Section Home_Section;
    public Section Game_Section;
    public Section Level_Complete_Section;
    public Section Level_Failed_Section;

    public void Start_Entry_Section(){
        Loader_Section.Start_Section();
    }
    
}
