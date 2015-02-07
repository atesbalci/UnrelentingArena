using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public bool started { get; set; }
    public Player player { get; set; }
    public string name { get; set; }
    public int credits { get; set; }

    private const string gameName = "Warlock Map Like Isometric Realtime Multiplayer Game";
    private float refreshRequestLength = 3;
    private HostData[] hostData;

    void Start() {
        started = false;
        name = "PlayerNameGoesHere";
    }

    public void startServer() {
        Network.InitializeServer(8, 25002, false);
        MasterServer.RegisterHost(gameName, "Test", "");
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
        credits = 0;
        name = "OtherPlayerNameGoesHere";
    }

    void OnServerInitialized() {
        credits = 0;
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player")) {
            if (playerObject.GetComponent<PlayerServersideScript>().networkPlayer == player) {
                Network.Destroy(playerObject);
                break;
            }
        }
    }

    void OnPlayerConnected(NetworkPlayer player) {
        if (started) {
            Network.CloseConnection(player, true);
        }
    }

    public void beginGame() {
        int x = -20;
        int no = 0;
        GameObject hostPlayerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
        hostPlayerObject.AddComponent<PlayerServersideScript>().networkPlayer = Network.player;
        initializePlayer(hostPlayerObject.networkView.viewID, no);
        foreach (NetworkPlayer player in Network.connections) {
            x += 20;
            no++;
            GameObject playerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
            playerObject.AddComponent<PlayerServersideScript>().networkPlayer = player;
            networkView.RPC("initializePlayer", player, playerObject.networkView.viewID, no);
        }
        networkView.RPC("setStarted", RPCMode.All, true);
    }

    [RPC]
    public void initializePlayer(NetworkViewID id, int no) {
        Player newPlayer = new Player();
        NetworkView.Find(id).GetComponent<PlayerScript>().player = newPlayer;
        player = newPlayer;
        player.name = name;
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
                    GUI.Label(new Rect(100, 100, 100, 50), "Awaiting server to start the game");
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
