using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
    Menu, Pregame, Ingame, Scores, Intermission
}

public class GameManager : MonoBehaviour {
    public string playerName { get; set; }
    public GameState state { get; set; }
    public Player player { get; set; }
    public int credits { get; set; }

    private const string gameName = "Warlock Map Like Isometric Realtime Multiplayer Game";
    private float refreshRequestLength = 3;
    private HostData[] hostData;
    private Dictionary<NetworkPlayer, string> nameList;

    void Start() {
        state = GameState.Menu;
        playerName = "";
    }

    public void StartServer() {
        Network.InitializeServer(8, 25002, false);
        MasterServer.RegisterHost(gameName, "Test");
    }

    public IEnumerator RefreshHosts() {
        MasterServer.RequestHostList(gameName);
        float timeEnd = Time.time + refreshRequestLength;

        while (Time.time < timeEnd) {
            hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }
    }

    public void Initialize() {
        state = GameState.Pregame;
        credits = 0;
        nameList = new Dictionary<NetworkPlayer, string>();
        networkView.RPC("SendName", RPCMode.AllBuffered, Network.player, playerName);
    }

    void OnConnectedToServer() {
        Initialize();
    }

    [RPC]
    public void SendName(NetworkPlayer player, string name) {
        nameList.Add(player, name);
    }

    void OnServerInitialized() {
        Initialize();
    }

    void OnDisconnectedFromServer(NetworkDisconnection info) {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            Destroy(gameObject);
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Skill"))
            Destroy(gameObject);
        state = GameState.Menu;
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        nameList.Remove(player);
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player")) {
            if (playerObject.GetComponent<PlayerServersideScript>().networkPlayer == player) {
                Network.Destroy(playerObject);
                break;
            }
        }
    }

    void OnPlayerConnected(NetworkPlayer player) {
        if (state != GameState.Pregame) {
            Network.CloseConnection(player, true);
        }
    }

    public void BeginGame() {
        int x = -20;
        GameObject hostPlayerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
        hostPlayerObject.AddComponent<PlayerServersideScript>().networkPlayer = Network.player;
        networkView.RPC("InitializePlayer", RPCMode.AllBuffered, hostPlayerObject.networkView.viewID, Network.player);
        foreach (NetworkPlayer networkPlayer in Network.connections) {
            x += 20;
            GameObject playerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
            playerObject.AddComponent<PlayerServersideScript>().networkPlayer = networkPlayer;
            networkView.RPC("InitializePlayer", RPCMode.AllBuffered, playerObject.networkView.viewID, networkPlayer);
        }
        networkView.RPC("SetState", RPCMode.All, (int)GameState.Ingame);
    }

    [RPC]
    public void InitializePlayer(NetworkViewID id, NetworkPlayer owner) {
        Player newPlayer = new Player();
        NetworkView.Find(id).GetComponent<PlayerScript>().player = newPlayer;
        if (owner == Network.player) {
            player = newPlayer;
            player.name = playerName;
        } else {
            string name;
            if (nameList.TryGetValue(owner, out name))
                newPlayer.name = name;
        }
    }

    private void ListPlayers() {
        int h = 100;
        foreach (NetworkPlayer player in nameList.Keys) {
            string name;
            if (nameList.TryGetValue(player, out name)) {
                GUI.Label(new Rect(300, h, 200, 30), name);
                h += 50;
            }
        }
    }

    [RPC]
    public void SetState(int newState) {
        state = (GameState)newState;
    }

    void OnGUI() {
        switch (state) {
            case GameState.Menu: {
                    playerName = GUI.TextField(new Rect(100, 50, 100, 30), playerName);
                    if (GUI.Button(new Rect(100, 100, 100, 30), "Start Server")) {
                        StartServer();
                    }

                    if (GUI.Button(new Rect(100, 150, 100, 30), "Refresh Servers")) {
                        StartCoroutine("RefreshHosts");
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
                    break;
                }
            case GameState.Pregame: {
                    if (Network.isServer) {
                        if (GUI.Button(new Rect(100, 100, 100, 30), "Begin"))
                            BeginGame();
                    } else if (Network.isClient) {
                        GUI.Label(new Rect(100, 100, 100, 50), "Awaiting server to start the game");
                    }
                    ListPlayers();
                    break;
                }
            case GameState.Ingame: {
                    break;
                }
            case GameState.Scores: {
                    break;
                }
            case GameState.Intermission: {
                    break;
                }
        }
    }
}
