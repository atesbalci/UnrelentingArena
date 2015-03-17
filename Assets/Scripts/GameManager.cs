﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
    Menu,
    Pregame,
    Ingame,
    Scores,
    Shop
}

public class GameManager : MonoBehaviour {
    private const string GAME_NAME = "Warlock Map Like Isometric Realtime Multiplayer Game Testing";
    public const int PORT = 25002;
    private const float REFRESH_LENGTH = 10;
    private const int ROUND_LIMIT = 4;
    
    public HostData[] hostData { get; set; }
    public PlayerData playerData { get; set; }
    public int round { get; set; }
    public float remainingIntermissionDuration { get; set; }
    public KeyCode[] keys;

    private Color[] colors = { Color.red, Color.blue, Color.green, new Color(255 / 255f, 165 / 255f, 0 / 255f) };
    private Dictionary<NetworkPlayer, PlayerData> playerList;
    private GameState _state;
    public GameState state {
        get {
            return _state;
        }
        set {
            GameState previous = state;
            _state = value;
            if (previous != GameState.Shop && state == GameState.Scores)
                remainingIntermissionDuration = 30;
            else if (state == GameState.Ingame) {
                round++;
                remainingIntermissionDuration = 0;
            }
            GameObject.FindGameObjectWithTag("Stage").GetComponent<StageMainScript>().running = (state == GameState.Ingame);
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasNavigator>().RefreshUI();
        }
    }

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
        MasterServer.RegisterHost(GAME_NAME, playerData.name + "'s Game");
    }

    public IEnumerator RefreshHosts() {
        MasterServer.RequestHostList(GAME_NAME);
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
            Network.Destroy(gameObject);
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Skill"))
            Network.Destroy(gameObject);
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
        if (round == 0)
            networkView.RPC("SetColor", RPCMode.All, Network.player, 0);
        int i = 0;
        foreach (NetworkPlayer networkPlayer in Network.connections) {
            i++;
            x += 20;
            GameObject playerObject = Network.Instantiate(Resources.Load("Player"), new Vector3(x, 0, 0), new Quaternion(), 0) as GameObject;
            playerObject.GetComponent<PlayerScript>().player.owner = networkPlayer;
            networkView.RPC("InitializePlayer", RPCMode.AllBuffered, playerObject.networkView.viewID, networkPlayer);
            if (round == 0)
                networkView.RPC("SetColor", RPCMode.All, Network.player, i);
        }
        networkView.RPC("SetState", RPCMode.All, (int)GameState.Ingame);
    }

    [RPC]
    public void SetColor(NetworkPlayer player, int index) {
        PlayerData data;
        if (playerList.TryGetValue(player, out data)) {
            data.color = colors[index];
        }
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
            pd.skillPoints -= pd.skillSet.GetUpgradeCost((SkillType)skill);
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
        if(remainingIntermissionDuration > 0)
            remainingIntermissionDuration -= Time.deltaTime;
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
                        NetworkPlayer np = playerObject.GetComponent<PlayerScript>().player.owner;
                        networkView.RPC("UpdateScore", RPCMode.AllBuffered, np, playerObject.GetComponent<PlayerScript>().player.score + 200);
                    }
                    networkView.RPC("SetState", RPCMode.All, (int)GameState.Scores);
                }
            } else if (state == GameState.Scores || state == GameState.Shop) {
                if (remainingIntermissionDuration <= 0) {
                    if (round <= ROUND_LIMIT)
                        BeginGame();
                    else
                        Network.Disconnect();
                }
            }
        }
    }
}
