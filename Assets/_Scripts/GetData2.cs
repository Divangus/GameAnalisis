using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    private string _url;

    void OnEnable()
    {
        Simulator.OnNewPlayer += OnPlayerAdded;
    }

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= OnPlayerAdded;
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
        }

       
    }
}
