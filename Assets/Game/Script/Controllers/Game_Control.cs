using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Control : MonoBehaviour, Pointer_Down_Listener, Pointer_Up_Listener, Drag_Listener
{

    public static int CurrentLevel => PlayerPrefs.GetInt("Current_Level_Index", 0);
    
    private int Save_Data_Version = 1; // Level orders changed
    public int Coins;
    public int Current_Level_Coins;
    private Scene Current_Level_Scene;
    public string Current_Scene_Name = ""; // also flag to determine game played once
    public int Current_Level_Index = 0;
    public Player_Control Player_Control;
    public Level_Control Level_Control;
    public bool Playing = false; // Playing or other features like home screen, upgrade screen, etc...
    public List<string> Levels;
    public Sprite[] Level_Icons;

    private Action Level_Ready_Callback;
    private Action Level_Completed_Callback;
    private Action Level_Failed_Callback;



    // Listeners
    private List<Action> On_Play_Listeners = new List<Action>();
    private List<Action> Update_Listeners = new List<Action>();
    private List<Action> Fixed_Update_Listeners = new List<Action>();
    private List<Action> Slow_Update_Listeners = new List<Action>();
    private float Slow_Update_Interval = 0.1f;



    private void Awake() {
        // Setup_Level_Info();
        SceneManager.sceneLoaded += On_Scene_Loaded;
        GDG.Input_Control.Add_Pointer_Down_Listener(this);
        GDG.Input_Control.Add_Pointer_Up_Listener(this);
        GDG.Input_Control.Add_Drag_Listener(this);

        StartCoroutine(Slow_Update());
    }


    // Save/Load Game Data ================================

    private void Load_Game_Data() {
        if(!Is_Save_Data_Valid()) return;
        Coins = PlayerPrefs.GetInt("Coins", 0);
        if(Current_Level_Index == 0) {
            Current_Level_Index = PlayerPrefs.GetInt("Current_Level_Index", 0);
        }

        Debug.Log("Game Data Loaded!");
    }

    public void Save_Game_Data() {
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.SetInt("Current_Level_Index", Current_Level_Index);

        PlayerPrefs.SetInt("Save_Data_Version", Save_Data_Version);
        Debug.Log("Game Data Saved!");
    }

    private bool Is_Save_Data_Valid() {
        int save_data_version = PlayerPrefs.GetInt("Save_Data_Version", 0);
        if(save_data_version == Save_Data_Version) {
            return true;
        } else {
            Debug.Log("Game data invalid!");
            return false;
        }
    }
    // ====================================================


    // Internal Load Level ====================================

    private void Load_Level() {
        Unset_Links(); // Unset links of previous level scene
        Current_Level_Coins = 0; // Reset current coins for current level
        int index = Get_Real_Level_Index(Current_Level_Index);
        Current_Scene_Name = Levels[index];
        SceneManager.LoadScene(Current_Scene_Name, LoadSceneMode.Additive); // Load level scene and start on scene loaded
    }

    private void On_Scene_Loaded(Scene scene, LoadSceneMode mode) {
        if(scene.name == gameObject.scene.name) return;
        if(Set_Links(scene)) { //Linked successfully
            Level_Control.Init_Level(Level_Completed, Level_Failed);
            Level_Ready_Callback?.Invoke(); // Callback Game_Section
            GDG.Analytics_Control.Send_Level_Start(); // Analytics
        } else {
            Debug.Log("Cannot link to level scene!");
        }
    }

    // Set links after scene loaded
    private bool Set_Links(Scene scene) {
        Current_Level_Scene = scene;
        GameObject[] root_objects = scene.GetRootGameObjects();
        for(int i = 0; i < root_objects.Length; i++) {
            if(root_objects[i].name == "Level_Control") {
                Level_Control = root_objects[i].GetComponent<Level_Control>();
                Player_Control = Level_Control.Player_Control;
                GDG.Level_Control = Level_Control;
                GDG.Player_Control = Player_Control;
                return true; // We all set
            }
        }

        return false;
    }

    private void Unset_Links() {
        Level_Control = null;
        Player_Control = null;
        GDG.Level_Control = null;
        GDG.Player_Control = null;
    }

    // Looping levels
    private int Get_Real_Level_Index(int level) {
        return level % Levels.Count;
    }

    // Level_Group_Control uses it
    public string Get_Current_Level_Scene_Name() {
        return Levels[Get_Real_Level_Index(Current_Level_Index)];
    }

    // ====================================================


    // MGC functions ======================================

    public void Load_Game_State() { // Called by MGC
        Load_Game_Data();
        GDG.UI_Control.Update_All();
    }

    public void Start_Level(Action level_ready_callback, Action level_completed_callback, Action level_failed_callback) { // Called by Game_Section
        Level_Ready_Callback = level_ready_callback;
        Level_Completed_Callback = level_completed_callback;
        Level_Failed_Callback = level_failed_callback;

        if(Current_Level_Scene.name != null) {
            StartCoroutine(Unload_Scene_Completely());
        }
        Load_Level();
    }

    IEnumerator Unload_Scene_Completely() {
        yield return SceneManager.UnloadSceneAsync(Current_Level_Scene);
        Resources.UnloadUnusedAssets();
    }

    public void Finish_Current_Level() { // Called by MGC
        Current_Level_Index++;
        Save_Game_Data();
    }


    // Called by Player_Control
    public void Level_Completed() {
        Playing = false;
        Level_Completed_Callback?.Invoke(); // Callback Game_Section
        GDG.Analytics_Control.Send_Level_Complete();
        GDG.Monetization_Control.Show_Interstitial();
    }

    // Called by Player_Control
    public void Level_Failed() {
        Playing = false;
        Level_Failed_Callback?.Invoke(); // Callback Game_Section
        GDG.Analytics_Control.Send_Level_Fail();
        GDG.Monetization_Control.Show_Interstitial();
    }

    // In-Game Functions ==================================

    public void Play_Level() {
        // GDG.UI_Control.Hide_Level_Start_Screen(); <<<***>>>
        Playing = true;
        On_Play(); // for listeners
    }

    public void Earn_Coins(int amount) {
        Coins += amount;
        Current_Level_Coins += amount;
        GDG.UI_Control.Update_All();
    }

    public bool Spend_Coins(int amount) {
        if(Coins < amount) {
            // GDG.UI_Control.Message_Popup_Control.Show_Mesage("NOT ENOUGH COINS!"); <<<***>>>
            return false;
        } else {
            Coins -= amount;
            GDG.UI_Control.Update_All();
            return true;
        }
    }

    public bool Spend_Coins(int amount, Action callback_after_ad_watch) {
        if(Coins < amount) {
            // GDG.UI_Control.Not_Enough_Coins_Popup_Control.Show_Offer(amount, callback_after_ad_watch); <<<***>>>
            return false;
        } else {
            Coins -= amount;
            GDG.UI_Control.Update_All();
            return true;
        }
    }



    public void On_Pointer_Down(Vector2 pos) {
        if(Playing) {
            Player_Control.On_Pointer_Down(pos);
        }
    }

    public void On_Pointer_Up(Vector2 pos) {
        if(Playing) {
            Player_Control.On_Pointer_Up(pos);
        }
    }

    public void On_Drag(Vector2 delta) {
        if(Playing) {
            Player_Control.On_Drag(delta);
        }
    }

    // On_Play -----------------------------

    public void Add_On_Play_Listener(Action callback) {
        if(!On_Play_Listeners.Contains(callback)) {
            On_Play_Listeners.Add(callback);
        }
    }
    public void Remove_On_Play_Listener(Action callback) {
        On_Play_Listeners.Remove(callback);
    }

    private void On_Play() {
        if(Playing) {
            for(int i = 0; i < On_Play_Listeners.Count; i++) {
                On_Play_Listeners[i].Invoke();
            }
        }
    }

    // Update ---------------------------------------

    public void Add_Update_Listener(Action callback) {
        if(!Update_Listeners.Contains(callback)) {
            Update_Listeners.Add(callback);
        }
    }
    public void Remove_Update_Listener(Action callback) {
        Update_Listeners.Remove(callback);
    }

    // ---

    public void Add_Fixed_Update_Listener(Action callback) {
        if(!Fixed_Update_Listeners.Contains(callback)) {
            Fixed_Update_Listeners.Add(callback);
        }
    }
    public void Remove_Fixed_Update_Listener(Action callback) {
        Fixed_Update_Listeners.Remove(callback);
    }

    // ---

    public void Add_Slow_Update_Listener(Action callback) {
        if(!Slow_Update_Listeners.Contains(callback)) {
            Slow_Update_Listeners.Add(callback);
        }
    }
    public void Remove_Slow_Update_Listener(Action callback) {
        Slow_Update_Listeners.Remove(callback);
    }

    // -------------

    private void Update() {
        if(Playing) {
            for(int i = 0; i < Update_Listeners.Count; i++) {
                Update_Listeners[i].Invoke();
            }
        }
    }

    private void FixedUpdate() {
        if(Playing) {
            for(int i = 0; i < Fixed_Update_Listeners.Count; i++) {
                Fixed_Update_Listeners[i].Invoke();
            }
        }
    }

    private IEnumerator Slow_Update() {
        while(true) {
            if(Playing) {
                for(int i = 0; i < Slow_Update_Listeners.Count; i++) {
                    Slow_Update_Listeners[i].Invoke();
                }
            }
            yield return new WaitForSeconds(Slow_Update_Interval);
        }
    }

}


