using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    public bool started { get; set; }
    public Player player { get; set; }

    private string gameName = "Warlock Map Like Isometric Realtime Multiplayer Game";
    private float refreshRequestLength = 3;
    private HostData[] hostData;

    void Start() {
        started = false;
    }

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

    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    void OnPlayerConnected(NetworkPlayer player) {

    }

    void OnServerInitialized() {
    }

    public void beginGame() {
        int x = -20;
        int no = 1;
        foreach (NetworkPlayer player in Network.connections) {
            GameObject playerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
            networkView.RPC("initializePlayer", RPCMode.All, playerObject.networkView.viewID, player, no);
            x += 20;
            no++;
        }
        GameObject hostPlayerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
        networkView.RPC("initializePlayer", RPCMode.All, hostPlayerObject.networkView.viewID, Network.player, 0);
        networkView.RPC("setStarted", RPCMode.All, true);
    }

    [RPC]
    public void initializePlayer(NetworkViewID id, NetworkPlayer networkPlayer, int no) {
        Player newPlayer = new Player();
        NetworkView.Find(id).GetComponent<PlayerScript>().player = newPlayer;
        if (networkPlayer.Equals(Network.player)) {
            player = newPlayer;
        }
    }

    private void listPlayers() {
        int h = 200;
        foreach (NetworkPlayer player in Network.connections) {
            GUI.Label(new Rect(300, 100, 100, 30), player.externalIP);
            h += 50;
        }
    }

    [RPC]
    public void setStarted(bool b) {
        started = b;
    }

    void OnGUI() {
        if (Network.isServer || Network.isClient) {
            if (!started) {
                if (Network.isServer) {
                    if (GUI.Button(new Rect(100, 100, 100, 30), "Begin"))
                        beginGame();
                } else if (Network.isClient) {
                    GUI.Label(new Rect(100, 100, 100, 30), "Awaiting server to start the game");
                }
                listPlayers();
            }
        } else {
            if (GUI.Button(new Rect(100, 100, 100, 30), "Start Server")) {
                startServer();
            }

            if (GUI.Button(new Rect(100, 150, 100, 30), "Refresh Servers")) {
                StartCoroutine("refreshHosts");
            }

            if (hostData != null) {
                int h = 200;
                foreach (HostData hd in hostData) {
                    if (GUI.Button(new Rect(100, h, 100, 30), hd.gameName)) {
                        Network.Connect(hd);
                    }
                    h += 50;
                }
            }
        }
    }
}
