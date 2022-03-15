using UnityEngine;
using NativeWebSocket;

namespace FunkySheep.Network
{
  [AddComponentMenu("FunkySheep/Network/Network manager")]
  public class Manager : Types.Singleton<Manager>
  {
    public Connection connection;
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
    }

    private void onConnectionClose(WebSocketCloseCode code) {
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
  }
}
