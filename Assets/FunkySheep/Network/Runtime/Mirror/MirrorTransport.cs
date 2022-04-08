using System;
using System.Net;
using System.Security.Authentication;
using UnityEngine;
using UnityEngine.Serialization;
using Mirror;
using System.Linq;

namespace FunkySheep.Network
{
    public class MirrorTransport : Mirror.Transport
    {
      [Tooltip("Protect against allocation attacks by keeping the max message size small. Otherwise an attacker might send multiple fake packets with 2GB headers, causing the server to run out of memory after allocating multiple large packets.")]
      public int maxMessageSize = 16 * 1024;

      public override bool Available()
      {
        return true;
      }

      public override bool ClientConnected()
      {
        return true;
      }

      public override void ClientConnect(string address)
      {
        OnClientConnected.Invoke();
      }

      public override void ClientConnect(Uri uri)
      {
      }

      public override void ClientSend(ArraySegment<byte> segment, int channelId = Channels.Reliable)
      {
        FunkySheep.Network.Manager.Instance.webSocket.Send(segment.ToArray());
      }

      public override void ClientDisconnect()
      {

      }

      public override Uri ServerUri()
      {
        UriBuilder builder = new UriBuilder
        {
          Host = FunkySheep.Network.Manager.Instance.connection.address,
          Port = FunkySheep.Network.Manager.Instance.connection.port
        };

        return builder.Uri;
      }

      public override bool ServerActive()
      {
        return true;
      }

      public override void ServerStart()
      {

      }

      public override void ServerSend(int connectionId, ArraySegment<byte> segment, int channelId = Channels.Reliable)
      {

      }

      public override void ServerDisconnect(int connectionId)
      {

      }

      public override string ServerGetClientAddress(int connectionId)
      {
        return "";
      }

      public override void ServerStop()
      {

      }

      public override int GetMaxPacketSize(int channelId = Channels.Reliable)
      {
        return maxMessageSize;
      }

      public override void Shutdown()
      {

      }
    }
}
