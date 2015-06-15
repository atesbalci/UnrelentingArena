using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
    Menu,
    Pregame,
    Ingame,
    Scores,
    Shop
}

public class GameManager : NetworkBehaviour {
    public static Color[] colors = { Color.red, Color.blue, Color.green, new Color(255 / 255f, 165 / 255f, 0 / 255f), Color.cyan, Color.yellow };

    public StageMainScript stage;
    public CanvasNavigator navigator;

    public static GameManager instance { get; private set; }

    private const string GAME_NAME = "Unrelenting Arena";
    private const float REFRESH_LENGTH = 10;
    private const int ROUND_LIMIT = 4;

    public HostData[] hostData { get; set; }
    public PlayerData playerData { get; set; }
    public int round { get; set; }
    public float remainingIntermissionDuration { get; set; }
    public Dictionary<int, PlayerData> playerList { get; private set; }

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
            stage.running = (state == GameState.Ingame);
            navigator.RefreshUI();
        }
    }

    public void SetName(string name) {
        playerData.name = name;
    }

    void Awake() {
        instance = this;
        state = GameState.Menu;
        playerData = new PlayerData(PlayerPrefs.GetString("name", "Player"));
        if (GameInput.instance == null)
            GameInput.instance = new GameInput();
    }

    [ClientRpc]
    public void RpcUpdateScore(int player, int credits) {
        PlayerData data;
        if (playerList.TryGetValue(player, out data)) {
            data.addPoints(credits);
        }
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
        playerList = new Dictionary<int, PlayerData>();
        //view.RPC("SendName", RPCMode.AllBuffered, Network.player, playerData.name);
        round = 0;
    }

    [Command]
    public void CmdSendName(int player, string name) {
        playerList.Add(player, new PlayerData(name));
    }

    public void Clear() {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
            Network.Destroy(gameObject);
    }

    public void BeginGame() {
        int i = 0;
        foreach (KeyValuePair<int, PlayerData> kvp in playerList) {
            //view.RPC("InstantiatePlayer", kvp.Key, i);
            i++;
        }
        //view.RPC("SetState", RPCMode.All, (int)GameState.Ingame);
    }

    [RPC]
    public void InstantiatePlayer(int index) {
        Vector3 spawnPoint = new Vector3(-20 + index * 10, 0, 0);
        GameObject playerObject = Network.Instantiate(Resources.Load("Player"), spawnPoint, Quaternion.identity, 0) as GameObject;
        //view.RPC("InitializePlayer", RPCMode.AllBuffered, playerObject.GetComponent<NetworkView>().viewID, Network.player, index);
    }

    [RPC]
    public void InitializePlayer(NetworkViewID id, int owner, int index) {
        PlayerScript playerScript = NetworkView.Find(id).GetComponent<PlayerScript>();
        PlayerData data;
        if (playerList.TryGetValue(owner, out data)) {
            playerScript.player.color = colors[index];
            playerScript.player.name = data.name;
            playerScript.player.owner = owner;
            playerScript.player.skillSet = data.skillSet;
            playerScript.player.itemSet = data.itemSet;
            data.currentPlayer = playerScript.player;
        }
        playerScript.Initialize();
    }

    [RPC]
    public void UpgradeSkill(int player, int skill) {
        PlayerData pd;
        if (playerList.TryGetValue(player, out pd)) {
            pd.skillPoints -= pd.skillSet.GetUpgradeCost((SkillType)skill);
            pd.skillSet.Upgrade((SkillType)skill);
        }
    }

    [RPC]
    public void BuyItem(int player, int item) {
        PlayerData pd;
        if (playerList.TryGetValue(player, out pd)) {
            pd.credits -= pd.itemSet.potentialItems[item].price;
            pd.itemSet.Get(item);
        }
    }

    public LinkedList<PlayerData> ListPlayers() {
        LinkedList<PlayerData> result = new LinkedList<PlayerData>();
        foreach (int player in playerList.Keys) {
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
        if (remainingIntermissionDuration > 0)
            remainingIntermissionDuration -= Time.deltaTime;
        if (isServer) {
            int headCount = 0;
            if (state == GameState.Ingame) {
                foreach (KeyValuePair<int, PlayerData> pd in playerList) {
                    if (!pd.Value.currentPlayer.dead) {
                        headCount++;
                    }
                }
                if (headCount <= 0) {
                    Clear();
                    foreach (KeyValuePair<int, PlayerData> pd in playerList) {
                        int np = pd.Value.currentPlayer.owner;
                        //view.RPC("UpdateScore", RPCMode.AllBuffered, np, pd.Value.currentPlayer.score + 200);
                    }
                    //view.RPC("SetState", RPCMode.All, (int)GameState.Scores);
                }
            } else if (state == GameState.Scores || state == GameState.Shop) {
                if (remainingIntermissionDuration <= 0) {
                    if (round <= ROUND_LIMIT)
                        BeginGame();
                    else
                        //Disconnect();
                        ;
                }
            }
        }
    }
}
