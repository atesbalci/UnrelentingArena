using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
    Menu, Pregame, Ingame, Scores, Shop
}

public class GameManager : MonoBehaviour {
    public const int PORT = 25002;
    private const float REFRESH_LENGTH = 10;
    private const int ROUND_LIMIT = 4;

    public HostData[] hostData { get; set; }
    public PlayerData playerData { get; set; }
    public int round { get; set; }
    public float remainingIntermissionDuration { get; set; }

    private GameState _state;
    public GameState state {
        get {
            return _state;
        }
        set {
            _state = value;
            GameObject.FindGameObjectWithTag("Stage").GetComponent<StageMainScript>().running = (state == GameState.Ingame);
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasNavigator>().RefreshUI();
        }
    }
    private const string gameName = "Warlock Map Like Isometric Realtime Multiplayer Game Testing";
    private Dictionary<NetworkPlayer, PlayerData> playerList;

    public void SetName(string name) {
        playerData.name = name;
    }

    void Start() {
        state = GameState.Menu;
        playerData = new PlayerData("");
    }

    [RPC]
    public void UpdateScore(NetworkPlayer player, int credits) {
        PlayerData data;
        if (playerList.TryGetValue(player, out data)) {
            data.addPoints(credits);
        }
    }

    public void StartServer() {
        Network.InitializeServer(8, PORT, false);
        MasterServer.RegisterHost(gameName, playerData.name + "'s Game");
    }

    public IEnumerator RefreshHosts() {
        MasterServer.RequestHostList(gameName);
        float timeEnd = Time.time + REFRESH_LENGTH;

        while (Time.time < timeEnd) {
            hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }
    }

    public void Initialize() {
        state = GameState.Pregame;
        playerData.Clear();
        playerList = new Dictionary<NetworkPlayer, PlayerData>();
        networkView.RPC("SendName", RPCMode.AllBuffered, Network.player, playerData.name);
        round = 0;
    }

    void OnConnectedToServer() {
        Initialize();
    }

    [RPC]
    public void SendName(NetworkPlayer player, string name) {
        if (player != Network.player)
            playerList.Add(player, new PlayerData(name));
        else
            playerList.Add(player, playerData);
    }

    void OnServerInitialized() {
        Initialize();
    }

    void OnDisconnectedFromServer(NetworkDisconnection info) {
        Clear();
        state = GameState.Menu;
    }

    public void Clear() {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            Destroy(gameObject);
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Skill"))
            Destroy(gameObject);
    }

    void OnPlayerDisconnected(NetworkPlayer player) {
        Network.RemoveRPCs(player);
        playerList.Remove(player);
        foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player")) {
            if (playerObject.GetComponent<PlayerScript>().player.owner == player) {
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
        hostPlayerObject.GetComponent<PlayerScript>().player.owner = Network.player;
        networkView.RPC("InitializePlayer", RPCMode.AllBuffered, hostPlayerObject.networkView.viewID, Network.player);
        foreach (NetworkPlayer networkPlayer in Network.connections) {
            x += 20;
            GameObject playerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
            playerObject.GetComponent<PlayerScript>().player.owner = networkPlayer;
            networkView.RPC("InitializePlayer", RPCMode.AllBuffered, playerObject.networkView.viewID, networkPlayer);
        }
        networkView.RPC("SetState", RPCMode.All, (int)GameState.Ingame);
        round++;
    }

    [RPC]
    public void InitializePlayer(NetworkViewID id, NetworkPlayer owner) {
        PlayerScript playerScript = NetworkView.Find(id).GetComponent<PlayerScript>();
        PlayerData data;
        if (playerList.TryGetValue(owner, out data)) {
            playerScript.player.name = data.name;
            playerScript.player.owner = owner;
            playerScript.player.skillSet = data.skillSet;
            playerScript.player.itemSet = data.itemSet;
        }
    }

    [RPC]
    public void UpgradeSkill(NetworkPlayer player, int skill) {
        PlayerData pd;
        if (playerList.TryGetValue(player, out pd)) {
            pd.skillSet.Upgrade((SkillType)skill);
        }
    }

    public LinkedList<PlayerData> ListPlayers() {
        LinkedList<PlayerData> result = new LinkedList<PlayerData>();
        foreach (NetworkPlayer player in playerList.Keys) {
            PlayerData data;
            if (playerList.TryGetValue(player, out data)) {
                result.AddFirst(new LinkedListNode<PlayerData>(data));
            }
        }
        SortScores(result.First);
        return result;
    }

    public void SortScores(LinkedListNode<PlayerData> head) {
        if (head == null)
            return;
        LinkedListNode<PlayerData> max = head;
        LinkedListNode<PlayerData> cur = head.Next;
        while (cur != null) {
            if (cur.Value.credits > max.Value.credits)
                max = cur;
            cur = cur.Next;
        }
        PlayerData tmp = head.Value;
        head.Value = max.Value;
        max.Value = tmp;
        SortScores(head.Next);
    }

    public void RefreshServers() {
        StartCoroutine("RefreshHosts");
    }

    [RPC]
    public void SetState(int newState) {
        state = (GameState)newState;
    }

    void Update() {
        if (Network.isServer) {
            int headCount = 0;
            if (state == GameState.Ingame) {
                foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player")) {
                    if (!playerObject.GetComponent<PlayerScript>().player.dead) {
                        headCount++;
                    }
                }
                if (headCount <= 0) {
                    Clear();
                    foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player")) {
                        PlayerData data;
                        int credits = 0;
                        NetworkPlayer np = playerObject.GetComponent<PlayerScript>().player.owner;
                        if (playerList.TryGetValue(np, out data)) {
                            credits = data.credits;
                        }
                        networkView.RPC("UpdateScore", RPCMode.AllBuffered, np, playerObject.GetComponent<PlayerScript>().player.score);
                    }
                    networkView.RPC("SetState", RPCMode.All, (int)GameState.Scores);
                    remainingIntermissionDuration = 30;
                }
            } else if (state == GameState.Scores || state == GameState.Shop) {
                if (remainingIntermissionDuration <= 0) {
                    if (round <= ROUND_LIMIT)
                        BeginGame();
                    else
                        Network.Disconnect();
                }
                remainingIntermissionDuration -= Time.deltaTime;
            }
        }
    }
}
