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
    public string serverUrl;
    public string serverUrlItem;
    public string serverUrlSessionsEnd;
    public string serverUrlSessions;

    private int userID = 0;
    private int sessionID = 0;



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
        public int user_id;
        public string Start;
    }

    public class Item
    {
       public int Item_ID;
       public string buyDateTime;
       public int session_id;
       //session id o player id
    }

    public class EndSessionData
    {
        //public int user_id;
        public string End;
        public int session_id;
    }

    void OnEnable()
    {
        Simulator.OnNewPlayer += OnPlayerAdded;
        Simulator.OnNewSession += OnSessionsAdded;
        Simulator.OnBuyItem += OnItemBought;
        Simulator.OnEndSession += OnSessionsEnded;
    }
    

    private void OnDisable()
    {
        Simulator.OnNewPlayer -= OnPlayerAdded;
        Simulator.OnNewSession -= OnSessionsAdded;
        Simulator.OnBuyItem -= OnItemBought;
        Simulator.OnEndSession -= OnSessionsEnded;

    }

    private void OnSessionsAdded(DateTime Start)
    {
        SessionData sessionData = new SessionData
        {
            user_id = userID,
            Start = Start.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string jsonData = JsonUtility.ToJson(sessionData);

        // Send the JSON data to the server
        StartCoroutine(UploadSession(jsonData));
    }

    private void OnSessionsEnded(DateTime End)
    {
        EndSessionData sessionData = new EndSessionData // to do
        {
            session_id = sessionID,
            End = End.ToString("yyyy-MM-dd HH:mm:ss")
        };

        string jsonData = JsonUtility.ToJson(sessionData);

        // Send the JSON data to the server
        StartCoroutine(SessionEndData(jsonData));
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

        Debug.Log("Received userID: " + userID);
    }

    private void OnItemBought(int Item_ID, DateTime buyDateTime)
    {
        Debug.Log("Item added");

        // Create a UserData object and fill it with the player's data
        Item user = new Item
        {
            Item_ID = Item_ID,
            buyDateTime = buyDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            session_id = sessionID
        };

        // Convert the UserData object to JSON
        string jsonData = JsonUtility.ToJson(user);

        // Send the JSON data to the server
        StartCoroutine(UploadItem(jsonData));
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
            //Debug.Log("Form upload complete!");
             //Debug.Log("Data uploaded successfully!");
                Debug.Log(www.downloadHandler.text);
            if (int.TryParse(www.downloadHandler.text, out userID))
            {
                Debug.Log("Received id: " + userID);
            }
            else
            {
                Debug.LogError("Failed to parse id from the response: " + www.downloadHandler.text);
            }
            Debug.Log(jsonData);
            CallbackEvents.OnAddPlayerCallback?.Invoke(8);
        }

       
    }
    IEnumerator UploadSession(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(serverUrlSessions, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!");
            //Debug.Log("Data uploaded successfully!");
            Debug.Log(www.downloadHandler.text);
            if (int.TryParse(www.downloadHandler.text, out sessionID))
            {
                Debug.Log("Received id: " + sessionID);
            }
            else
            {
                Debug.LogError("Failed to parse id from the response: " + www.downloadHandler.text);
            }
            Debug.Log(jsonData);
            CallbackEvents.OnNewSessionCallback?.Invoke(8);
        }


    }

    IEnumerator UploadItem(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(serverUrlItem, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!");
            //Debug.Log("Data uploaded successfully!");
            Debug.Log(www.downloadHandler.text);
            Debug.Log(jsonData);
            CallbackEvents.OnItemBuyCallback?.Invoke();
        }


    }

    IEnumerator SessionEndData(string jsonData)
    {
        WWWForm form = new WWWForm();
        form.AddField("jsonData", jsonData);


        UnityWebRequest www = UnityWebRequest.Post(serverUrlSessionsEnd, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!");
            //Debug.Log("Data uploaded successfully!");
            Debug.Log(www.downloadHandler.text);
            Debug.Log(jsonData);
            CallbackEvents.OnEndSessionCallback?.Invoke(8);
        }


    }

}
