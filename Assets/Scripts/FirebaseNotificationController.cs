using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseNotificationController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenRecieved;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageRecieved;

    }

    public void OnTokenRecieved(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Recieved registration: " + token.Token);
    }

    public void OnMessageRecieved(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Recieved a new message from: " + e.Message.From);
    }

}
