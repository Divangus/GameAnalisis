using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetData : MonoBehaviour
{
    // Start is called before the first frame update

    struct _user
    {
        string name;
        string country;
        DateTime dateTime;

        public _user(string name, string country, DateTime dateTime)
        {
            this.name = name; 
            this.country = country;
            this.dateTime = dateTime;
        }
    }
    private string _url;

    void OnEnable()
    {
        Simulator.OnNewPlayer += OnPLayerAdded;
    }

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= OnPLayerAdded;
    }

    private void OnPLayerAdded(string name, string country, DateTime dateTime)
    {
        Debug.Log("on send player");

        _user User = new _user(name, country, dateTime);

    }



    IEnumerator Upload()
    {
        using (UnityWebRequest www = UnityWebRequest.Post(_url, "{ \"field1\": 1, \"field2\": 2 }", "application/json"))
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

