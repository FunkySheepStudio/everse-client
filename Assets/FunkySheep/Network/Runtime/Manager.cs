using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

namespace FunkySheep.Network
{
  [AddComponentMenu("FunkySheep/Network/Network manager")]
  public class Manager : Types.Singleton<Manager>
  {
    public Connection connection;
    public FunkySheep.Events.SimpleEvent onConnect;
    public FunkySheep.Events.SimpleEvent onDisconnect;
    [SerializeField]
    List<Services.Service> services = new List<Services.Service>();
    WebSocket webSocket;

    private void Start() {
      Connect();
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
    }

    async void OnApplicationQuit()
    {
      await webSocket.Close();
    }

    public void Regiser(Services.Service service)
    {
      if (!services.Contains(service))
      {
        services.Add(service);
      }
    }

    public void Send(string message)
    {
      webSocket.SendText(message);
    }
  }
}
