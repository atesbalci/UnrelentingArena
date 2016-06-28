using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState {
	Menu,
	Pregame,
	Ingame,
	Intermission
}

public class GameManager : MonoBehaviour {
	public StageMainScript stage;
	public CanvasNavigator navigator;
	public GameObject playerPrefab;
	public TooltipScript tooltip;
	public Color[] colors;
	public GameObject[] skills;
	public CountdownScript countdown;
	public GameObject spawnPrefab;

	public static GameManager instance { get; private set; }
	public const int PORT = 25002;
	public const float PLATFORM_RADIUS = 34;

	private const string GAME_NAME = "Unrelenting Arena Testing";
	private const float REFRESH_LENGTH = 10;
	private const int ROUND_LIMIT = 4;

	public HostData[] hostData { get; set; }
	public PlayerData playerData { get; set; }
	public int round { get; set; }
	public float remainingIntermissionDuration { get; set; }
	public Dictionary<NetworkPlayer, PlayerData> playerList { get; private set; }
	public int headCount { get; set; }
	public bool locked { get; set; }

	private NetworkView view;
	private GameState _state;
	public GameState state {
		get {
			return _state;
		}
		set {
			GameState previous = state;
			_state = value;
			if (state == GameState.Intermission)
				remainingIntermissionDuration = 30;
			else if (state == GameState.Ingame) {
				round++;
				remainingIntermissionDuration = 0;
			}
			tooltip.gameObject.SetActive(false);
			stage.running = (state == GameState.Ingame);
			navigator.RefreshUI();
		}
	}

	public void SetName(string name) {
		playerData.name = name;
	}

	void Awake() {
		instance = this;
		view = GetComponent<NetworkView>();
		state = GameState.Menu;
		playerData = new PlayerData(PlayerPrefs.GetString("name", "Player"));
		if (GameInput.instance == null)
			GameInput.instance = new GameInput();
	}

	[RPC]
	public void UpdateScore(NetworkPlayer player, int credits) {
		PlayerData data;
		if (playerList.TryGetValue(player, out data)) {
			data.AddPoints(credits);
		}
	}

	public void StartServer() {
		Network.InitializeServer(8, PORT, !Network.HavePublicAddress());
		MasterServer.RegisterHost(GAME_NAME, playerData.name + "'s Game");
        Debug.Log(Network.TestConnection());
        Debug.Log(Network.TestConnectionNAT());
        Debug.Log(Network.HavePublicAddress());
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
		view.RPC("SendName", RPCMode.AllBuffered, Network.player, playerData.name);
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

	public void Disconnect() {
		Network.Disconnect();
	}

	void OnPlayerConnected(NetworkPlayer player) {
		if (state != GameState.Pregame) {
			Network.CloseConnection(player, true);
		}
	}

	public void BeginGame() {
		StartCoroutine("BeginGameRoutine");
	}

	IEnumerator BeginGameRoutine() {
		locked = true;
		view.RPC("SetLocked", RPCMode.All, locked);
		view.RPC("SetState", RPCMode.All, (int)GameState.Ingame);
		int i = 0;
		float angle = (2 * Mathf.PI) / playerList.Count;
		foreach (KeyValuePair<NetworkPlayer, PlayerData> kvp in playerList) {
			Vector3 point = new Vector3(0 + 6 * Mathf.Cos(angle * i), 0, 6 * Mathf.Sin(angle * i));
			Network.Instantiate(spawnPrefab, point, Quaternion.identity, 0);
			yield return new WaitForSeconds(1f);
			if (kvp.Key == Network.player)
				InstantiatePlayer(i, point);
			else
				view.RPC("InstantiatePlayer", kvp.Key, i, point);
			i++;
		}
		for (i = 3; i >= 0; i--) {
			view.RPC("Countdown", RPCMode.All, i);
			if (i > 0)
				yield return new WaitForSeconds(1);
		}
		locked = false;
		view.RPC("SetLocked", RPCMode.All, locked);
	}

	[RPC]
	public void Countdown(int i) {
		countdown.Countdown(i);
	}

	[RPC]
	public void SetLocked(bool l) {
		locked = l;
	}

	[RPC]
	public void InstantiatePlayer(int index, Vector3 point) {
		GameObject playerObject = Network.Instantiate(playerPrefab, point, Quaternion.identity, 0) as GameObject;
		view.RPC("InitializePlayer", RPCMode.AllBuffered, playerObject.GetComponent<NetworkView>().viewID, Network.player, index);
	}

	[RPC]
	public void InitializePlayer(NetworkViewID id, NetworkPlayer owner, int index) {
		PlayerScript playerScript = NetworkView.Find(id).GetComponent<PlayerScript>();
		PlayerData data = playerList[owner];
		playerScript.player.color = colors[index];
		playerScript.player.name = data.name;
		playerScript.player.owner = owner;
		playerScript.player.skillSet = data.skillSet;
		data.currentPlayer = playerScript.player;
		playerScript.Initialize();
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
			if (cur.Value.score > max.Value.score)
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
		if (Network.isServer) {
			headCount = 0;
			if (state == GameState.Ingame) {
				foreach (KeyValuePair<NetworkPlayer, PlayerData> pd in playerList) {
					if (!locked && !pd.Value.currentPlayer.dead) {
						headCount++;
					}
				}
				if (!locked && headCount <= 0) {
					Clear();
					foreach (KeyValuePair<NetworkPlayer, PlayerData> pd in playerList) {
						NetworkPlayer np = pd.Value.currentPlayer.owner;
						view.RPC("UpdateScore", RPCMode.AllBuffered, np, pd.Value.currentPlayer.score + 200);
					}
					view.RPC("SetState", RPCMode.All, (int)GameState.Intermission);
				}
			} else if (state == GameState.Intermission) {
				if (remainingIntermissionDuration <= 0) {
					if (round <= ROUND_LIMIT)
						BeginGame();
					else
						Disconnect();
				}
			}
		}
	}
}
