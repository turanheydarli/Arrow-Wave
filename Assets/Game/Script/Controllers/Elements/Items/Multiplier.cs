using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using DG.Tweening;

public class Multiplier : Damageable
{
    [Min(1)] public int Multiply = 2;
    public GameObject Arrow_Prefab;
    public float Activation_Delay = 2f;
    public float Reactivation_Delay = 3f;

    private bool Hit = false;

    public TextMeshPro[] TMP;

    // -------------------------------

    private void Awake()
    {
        GDG.Game_Control.Add_On_Play_Listener(On_Play);
        Deactivate();
        for (int i = 0; i < TMP.Length; i++)
        {
            TMP[i].SetText(Multiply + "X");
        }
    }

    private void OnDestroy()
    {
        GDG.Game_Control.Remove_On_Play_Listener(On_Play);
    }

    private void On_Play()
    {
        StartCoroutine(Activation());
    }

    // -------------------------------

    private IEnumerator Activation()
    {
        yield return new WaitForSeconds(Activation_Delay);
        Activate();
    }

    private IEnumerator Reactivation()
    {
        yield return new WaitForSeconds(Reactivation_Delay);
        Activate();
    }

    private void Activate()
    {
        Hit = false;
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = true;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        transform.DOShakeScale(0.5f, 1);
        GDG.Audio_Control.Play_Pop();
    }

    private void Deactivate()
    {
        Collider[] colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        transform.localScale = Vector3.one;
    }

    // -------------------------------

    public override void Apply_Damage(Damage damage) {
        if(Hit) { return; }
        Hit = true;
        if(damage.Source is Arrow) {
            Arrow hit_arrow = (Arrow)damage.Source;
            GameObject prefab = Arrow_Prefab != null ? Arrow_Prefab : hit_arrow.gameObject;
        
            Vector3 arrowPosition = hit_arrow.transform.position;
            Quaternion arrowRotation = hit_arrow.transform.rotation;
            Vector3 arrowVelocity = hit_arrow.Restore_Velocity;
        
            Debug.Log($"Multiplier triggered by Arrow {hit_arrow.GetInstanceID()}, about to Destroy it.", this);

            hit_arrow.Destroy_On_Multiplier_Hit();
        
            int currentArrowCount = GDG.Player_Control.Bow_Control.Active_Arrows.Count;
            int maxArrows = GDG.Player_Control.Bow_Control.MaxActiveArrows;
            int availableSlots = maxArrows - currentArrowCount;
            
            if (availableSlots <= 0) {
                transform.DOShakeScale(0.5f, 0.5f, 10, 90, false);
                GDG.Audio_Control.Play_Haptic_Medium();
                
            }
            else {
                int arrowsToSpawn = Mathf.Min(Multiply, availableSlots);
                
                for(int i = 0; i < arrowsToSpawn; i++) {
                    GameObject arrow_obj = Instantiate(prefab, arrowPosition, arrowRotation, GDG.Level_Control.transform);
                    Randomize_Arrow_Position(arrow_obj);
                    Arrow arrow = arrow_obj.GetComponent<Arrow>();
                    Debug.Log("Spawned: "+ "  " + arrow.GetInstanceID() + " " + arrowVelocity, arrow);
                    arrow.Init(arrowVelocity);
                }
            
                GDG.Player_Control.Bow_Control.Arrange_Arrows();
            }
        
            Deactivate();
            StartCoroutine(Reactivation());
        }
    }
    private void Randomize_Arrow_Position(GameObject arrow_obj) {
        Vector3 pos = arrow_obj.transform.position;
        float radius = Mathf.Lerp(0.1f, 0.5f, Mathf.InverseLerp(2, 40, Multiply));
        Vector3 offset = new Vector3(
            Random.Range(-radius, radius),
            Random.Range(-radius, radius),
            Random.Range(-radius, radius)
        );
        arrow_obj.transform.position = pos + offset;
    }

    private void LateUpdate()
    {
        transform.LookAt(2f * transform.position - GDG.Camera_Control.Camera.transform.position);
    }
}