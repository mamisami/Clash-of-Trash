using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Network discovery of a server
/// </summary>
public class CoT_NetworkDiscovery : NetworkDiscovery {

	/// <summary>
	/// If we have find a server, connect to it
	/// </summary>
	/// <param name="fromAddress"></param>
	/// <param name="data"></param>
	public override void OnReceivedBroadcast(string fromAddress, string data)
	{
		base.OnReceivedBroadcast (fromAddress, data);

		if (!NetworkManager.singleton.IsClientConnected()) {
			NetworkManager.singleton.networkAddress = fromAddress;
			NetworkManager.singleton.StartClient ();

			this.StopBroadcast ();
		}
	}
}