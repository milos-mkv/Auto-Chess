using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerNetworkData : IEquatable<PlayerNetworkData>, INetworkSerializable
{
    public ulong clientId;
    public FixedString32Bytes username;

    public bool Equals(PlayerNetworkData other)
    {
        return clientId == other.clientId && username.Equals(other.username);
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref username);
    }
}
