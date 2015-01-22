using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    private string gameName = "Warlock Map Like Isometric Realtime Multiplayer Game";
    private float refreshRequestLength = 3;
    private HostData[] hostData;

    public void startServer() {
        Network.InitializeServer(8, 25002, false);
        MasterServer.RegisterHost(gameName, "Test", "a game");
    }

    public IEnumerator refreshHosts() {
        MasterServer.RequestHostList(gameName);
        float timeEnd = Time.time + refreshRequestLength;

        while (Time.time < timeEnd) {
            hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }
    }

    void OnConnectedToServer() {
        Network.Instantiate(Resources.Load("Player"), new Vector3(0, 0, 0), new Quaternion(), 0);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    void OnServerInitialized() {
        OnConnectedToServer();
    }

    void OnGUI() {
        if (Network.isClient || Network.isServer)
            return;

        if (GUI.Button(new Rect(100, 100, 100, 30), "Start Server")) {
            startServer();
        }

        if (GUI.Button(new Rect(100, 150, 100, 30), "Refresh Servers")) {
            StartCoroutine("refreshHosts");
        }

        if (hostData != null) {
            float h = 200;
            foreach (HostData hd in hostData) {
                if (GUI.Button(new Rect(100, h, 100, 30), hd.gameName)) {
                    Network.Connect(hd);
                }
                h += 50;
            }
        }
    }
}
