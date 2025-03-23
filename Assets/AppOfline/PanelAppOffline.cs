using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PanelAppOffline : MonoBehaviour
{
    public static PanelAppOffline Instance;

    public GameObject Panel;
    public bool DoCount;
    public float TimeCount,Count;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Start()
    {
        Panel.SetActive(false);
        yield return new WaitForSeconds(2);
        StartCoroutine(CheckRoutine());
    }

    public void Update()
    {
        if (DoCount)
        {
            if (Count <= TimeCount)
            {
                Count += Time.deltaTime;
            }
            else
            {
                DoCount = false;
                Count = 0;
                StartCoroutine(CheckRoutine());
            }
        }
    }

    IEnumerator CheckRoutine()
    {
        UnityWebRequest request = new UnityWebRequest("http://www.google.com/");
        yield return request.SendWebRequest();
        DoCount = true;
        if (string.IsNullOrEmpty(request.error))
        {
            Panel.SetActive(false);
            Debug.LogError("Off");
            Time.timeScale = 1;
        }
        else
        {
            Panel.SetActive(true);
            Debug.LogError("On");
            DoCount = false;
            Time.timeScale = 0;
        }
    }
}
