using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Small_Items : Level_Section {

    [Header("Drop_Small_Items")]

    public GameObject[] Drop_Object_List;
    public int Drop_Count = 20;
    public bool Randomize_Rotation = true;

    private List<GameObject> Objects;

    private float Drop_Height = 2.6f;
    private float Drop_Radius = 1.6f;

    // Section functions ------------------

    protected override void Start_Section() {


        Init_Objects();
        Show_UI();

        GDG.Input_Control.Enable_Input();
    }

    // protected override void End_Section() {
    //     base.End_Section();
    // }


    // Private functions -------------------



    // Pointer actions -----------------------------

    public override void On_Pointer_Down(Vector2 pos) {

    }

    public override void On_Drag(Vector2 pos) {

    }

    public override void On_Pointer_Up(Vector2 pos) {

    }

    // ---------------------------------------------


    // UI functions --------------------------------

    private void Show_UI() {
        // Tap to sprinkle call to action
        GDG.UI_Control.Game_Screen.Show_Start_Button(Drop_Objects);
    }


    // Gameplay actions ----------------------------

    private void Init_Objects() {

        Objects = new List<GameObject>();
        for(int i = 0; i < Drop_Count; i++) {
            int rnd = Random.Range(0, Drop_Object_List.Length);
            GameObject obj = Instantiate(Drop_Object_List[rnd], transform);
            Vector2 rnd_pos = Random.insideUnitCircle * Drop_Radius;
            float rnd_y = Random.Range(Drop_Height - 0.2f, Drop_Height + 0.2f);
            obj.transform.localPosition = new Vector3(rnd_pos.x, rnd_y, rnd_pos.y);
            if(Randomize_Rotation) {
                obj.transform.localRotation = Random.rotation;
            }
            obj.SetActive(false);
            Objects.Add(obj);
        }
    }


    private void Drop_Objects() {
        for(int i = 0; i < Objects.Count; i++) {
            Objects[i].SetActive(true);
        }
        Complete_Section();
        // GDG.Audio_Control.Play_Sprinkle();
        GDG.Audio_Control.Play_Haptic_Medium();
    }



    // --------------------------------------


    private void Complete_Section() {
        GDG.Input_Control.Disable_Input();
        StartCoroutine(Delay_Show_Final_View());
    }

    IEnumerator Delay_Show_Final_View() {
        yield return new WaitForSeconds(1f);
        // Confetti etc.
        End_Section();
    }


    // Util ------------------------------------



}
