using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetData : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Simulator.OnNewPlayer += OnPLayerAdded;
    }

    private void OnPLayerAdded(string arg1, string arg2, DateTime time)
    {
        Debug.Log("on send player");
    }



    IEnumerator Upload()
    {
        using (UnityWebRequest www = UnityWebRequest.Post("https://www.my-server.com/myapi", "{ \"field1\": 1, \"field2\": 2 }", "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}

