using System.Text;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using FunkySheep.SimpleJSON;

namespace FunkySheep.Network
{
  [AddComponentMenu("FunkySheep/Network/Network manager")]
  public class Manager : Types.Singleton<Manager>
  {
    public Connection connection;
    public FunkySheep.Events.SimpleEvent onConnect;
    public FunkySheep.Events.SimpleEvent onDisconnect;
    public List<Services.Service> services = new List<Services.Service>();
    WebSocket webSocket;

    private void Start() {
      Connect();
    }

    private void Update() {
      #if !UNITY_WEBGL || UNITY_EDITOR
        webSocket.DispatchMessageQueue();
      #endif
    }
    
    public async void Connect()
    {
      webSocket = WebSocketFactory.CreateInstance(connection.address + ":" + connection.port);
      //  Binding the events
      webSocket.OnOpen += onConnectionOpen;
      webSocket.OnClose += onConnectionClose;
      webSocket.OnError += onConnectionError;
      webSocket.OnMessage += onMessage;
      await webSocket.Connect();
    }

    private void onConnectionOpen() {
      onConnect.Raise();
    }

    private void onConnectionClose(WebSocketCloseCode code) {
      onDisconnect.Raise();
    }

    private void onConnectionError(string errMsg) {
    }

    private void onMessage(byte[] msg)
    {
      string strMsg = Encoding.UTF8.GetString(msg);
      JSONNode msgObject = JSON.Parse(strMsg);
      string msgService = msgObject["service"];
      string msgRequest = msgObject["request"];
      
      /*services.FindAll(service => service.api == msgService)
        .ForEach(service => {
          service.lastRawMsg = msgObject;

          service.fields.ForEach(field => {
            if (field.apiName != "" && msgObject["data"][field.apiName] != null)
              field.variable.fromJSONNode(msgObject["data"][field.apiName]);
          });

          //  Raise the event
          if (service.onReception) {
            service.onReception.Raise();
          }
        });*/

        Debug.Log("Received message: " + strMsg + this);
    }

    async void OnApplicationQuit()
    {
      await webSocket.Close();
    }

    public void Send(string message)
    {
      webSocket.SendText(message);
    }
  }
}