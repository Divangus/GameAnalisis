using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Networking;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class GetData2 : MonoBehaviour
{
    public string serverUrl; // Set this in the Unity Inspector

    // Define a public class for serializing user data
    [Serializable]
    public class UserData
    {
        public string name;
        public string country;
        public string gender;
        public int age;
        public string dateTime;
    }

    public class SessionData
    {
        public int sessions_id;
        public string dateTime;
    }


    private string _url;

    void OnEnable()
    {
        Simulator.OnNewPlayer += OnPlayerAdded;
        Simulator.OnNewSession += OnSessionsAdded;
  
        
    }
    

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= OnPlayerAdded;
        Simulator.OnNewSession -= OnSessionsAdded;

    }

    private void OnSessionsAdded(DateTime dateTime)
    {
        SessionData user = new SessionData
        {
          
            dateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string jsonData = JsonUtility.ToJson(user);

        // Send the JSON data to the server
        StartCoroutine(UploadSession(jsonData));
    }

    private void OnPlayerAdded(string name, string country, string gender, int age, DateTime dateTime)
    {
        Debug.Log("Player added");

        // Create a UserData object and fill it with the player's data
        UserData user = new UserData
        {
            name = name,
            country = country,
            gender = gender,
            age = age,
            dateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss")
        };

        // Convert the UserData object to JSON
        string jsonData = JsonUtility.ToJson(user);

        // Send the JSON data to the server
        StartCoroutine(Upload(jsonData));
    }

    IEnumerator Upload(string jsonData)
    {
         WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);

        
        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
             Debug.Log("Data uploaded successfully!");
                Debug.Log(www.downloadHandler.text);
                Debug.Log(jsonData);
            CallbackEvents.OnAddPlayerCallback?.Invoke(8);
        }

       
    }
    IEnumerator UploadSession(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(serverUrl, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            Debug.Log("Data uploaded successfully!");
            Debug.Log(www.downloadHandler.text);
            Debug.Log(jsonData);
            CallbackEvents.OnNewSessionCallback?.Invoke(8);
        }


    }

}
