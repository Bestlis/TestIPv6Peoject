using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerIOClient;
using System;

public class StartConnection : MonoBehaviour
{
	public static StartConnection Instance { get; private set; }

	public event EventHandler<string> OnStatusChanged;

	Client cl;
	Connection con;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);
	}

	void Start()
    {
		ConnectServer();
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}

	void ConnectServer()
	{
		Debug.Log("Connecting to server...");
		OnStatusChanged?.Invoke(this, "Connecting to server...");

		PlayerIO.UseSecureApiRequests = true;

		PlayerIO.Authenticate("testipv6project-ps3f0ciutewzfi42eodywa", "Public", new Dictionary<string, string> { { "userId", "start_user" } }, null,
		delegate (Client cl)
		{

			Debug.Log("Connected to server!");
			OnStatusChanged?.Invoke(this, "Connected to server!");


			this.cl = cl;

			//if (true)
			//	cl.Multiplayer.DevelopmentServer = new ServerEndpoint("192.168.88.18", 8184);

			CreateRoom();
		},
		delegate (PlayerIOError er)
		{
			print("ConnectServer Error: " + er.ToString());
			OnStatusChanged?.Invoke(this, "ConnectServer Error: " + er.ToString());

			Invoke("ConnectServer", 1f);
		});
	}


	//connecting to select room
	void CreateRoom()
	{
		print("Connecting to room...");
		OnStatusChanged?.Invoke(this, "Connecting to room...");


		cl.Multiplayer.CreateJoinRoom("START_ROOM", "StartRoom", true, null, null,
		delegate (Connection con)
		{
			Debug.Log("Connected to room!");
			OnStatusChanged?.Invoke(this, "Connected to room!");


			this.con = con;
			this.con.OnMessage += new MessageReceivedEventHandler(con_OnMessage);
			this.con.OnDisconnect += new DisconnectEventHandler(con_OnDisconnect);
		},
		delegate (PlayerIOError er)
		{
			print("CreateRoom Error: " + er);
			OnStatusChanged?.Invoke(this, "CreateRoom Error: " + er);

			Invoke("CreateRoom", 1f);
		});
	}

	private void con_OnDisconnect(object sender, string message)
	{
		
	}

	private void con_OnMessage(object sender, Message e)
	{
		
	}

	private void OnDestroy()
	{
		DisconnectServer();
	}

	private void DisconnectServer()
	{
		if (con != null)
			con.Disconnect();
	}
}
