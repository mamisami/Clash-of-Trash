using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CoT_NetworkDiscovery : NetworkDiscovery {

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