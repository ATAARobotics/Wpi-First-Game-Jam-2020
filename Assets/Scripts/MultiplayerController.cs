﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Text;
using Aws.GameLift.Realtime;
using Aws.GameLift.Realtime.Event;
using Aws.GameLift.Realtime.Types;

using System.IO;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Linq;

// This data structure is returned by the client service when a game match is found
[System.Serializable]
public class PlayerSessionObject
{
    public string PlayerSessionId;
    public string PlayerId;
    public string GameSessionId;
    public string FleetId;
    public string CreationTime;
    public string Status;
    public string IpAddress;
    public string Port;
}

public class MultiplayerController : MonoBehaviour
{

    const int MOVE_PLAYER_OP_CODE = 100;
    const int WINNER_DETERMINED_OP_CODE = 101;
    const int LOGICAL_PLAYER_OP_CODE = 102;

    const int SCENE_READY_OP_CODE = 200;
    const int LEVEL_CHANGE_OP_CODE = 201;

    // Start is called before the first frame update
    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        //ConnectToGameLiftServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Aws.GameLift.Realtime.Client _client;
    private int _logicalPlayerID = 0;
    private int _peerID = -1;   // invalid peer id

    public bool IsConnectedToServer { get; set; }

    public InputField codeInputField;

    public void SceneReady()
    {

    }

    public void NotifyMovePlayer(int playerId, float newPosition)
    {

    }

    public void NotifyWinnerDetermined(int winningPlayerId, int loosingPlayerId)
    {

    }

    public void NotifyChangeLevel()
    {

    }

    public void CreateAndJoinRoom()
    {

    }

    public void JoinRoom()
    {
        string code = codeInputField.text;
        ConnectToGameLiftServer(code);
    }

    private const string DEFAULT_ENDPOINT = "127.0.0.1";
    private const int DEFAULT_TCP_PORT = 3001;
    private const int DEFAULT_UDP_PORT = 8921;

    private int FindAvailableUDPPort(int firstPort, int lastPort)
    {
        var UDPEndPoints = IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners();
        List<int> usedPorts = new List<int>();
        usedPorts.AddRange(from n in UDPEndPoints where n.Port >= firstPort && n.Port <= lastPort select n.Port);
        usedPorts.Sort();
        for (int testPort = firstPort; testPort <= lastPort; ++testPort)

        {
            if (!usedPorts.Contains(testPort))
            {
                return testPort;
            }
        }
        return -1;
    }

    private IEnumerator ConnectToServer(string ipAddr, int port, string tokenUID)
    {
        ClientLogger.LogHandler = (x) => Debug.Log(x);
        ConnectionToken token = new ConnectionToken(tokenUID, null);

        ClientConfiguration clientConfiguration = ClientConfiguration.Default();

        _client = new Aws.GameLift.Realtime.Client(clientConfiguration);
        _client.ConnectionOpen += new EventHandler(OnOpenEvent);
        _client.ConnectionClose += new EventHandler(OnCloseEvent);
        _client.DataReceived += new EventHandler<DataReceivedEventArgs>(OnDataReceived);
        _client.ConnectionError += new EventHandler<Aws.GameLift.Realtime.Event.ErrorEventArgs>(OnConnectionErrorEvent);

        int UDPListenPort = FindAvailableUDPPort(DEFAULT_UDP_PORT, DEFAULT_UDP_PORT + 20);
        if (UDPListenPort == -1)
        {
            Debug.Log("Unable to find an open UDP listen port");
            yield break;
        }
        else
        {
            Debug.Log($"UDP listening on port: {UDPListenPort}");
        }

        Debug.Log($"[client] Attempting to connect to server ip: {ipAddr} TCP port: {port} Player Session ID: {tokenUID}");
        _client.Connect(string.IsNullOrEmpty(ipAddr) ? DEFAULT_ENDPOINT : ipAddr, port, UDPListenPort, token);

        while (true)
        {
            if (_client.ConnectedAndReady)
            {
                IsConnectedToServer = true;
                Debug.Log("[client] Connected to server");
                break;
            }
            yield return null;
        }
    }

    public void ActionConnectToServer(string ipAddr, int port, string tokenUID)
    {
        StartCoroutine(ConnectToServer(ipAddr, port, tokenUID));
    }

    // calls our game service Lambda function to get connection info for the Realtime server
    private void ConnectToGameLiftServer(string gameCode)
    {
        Debug.Log("Reaching out to client service Lambda function");

        AWSConfigs.AWSRegion = "us-west-2"; // Your region here
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        // paste this in from the Amazon Cognito Identity Pool console
        CognitoAWSCredentials credentials = new CognitoAWSCredentials(
            "us-west-2:7f096143-ff98-4d63-88c6-52b174d31de8", // Identity pool ID
            RegionEndpoint.USWest2 // Region
        );

        AmazonLambdaClient client = new AmazonLambdaClient(credentials, RegionEndpoint.USWest2);
        InvokeRequest request = new InvokeRequest
        {
            FunctionName = "wpigamejam-ClientConnectToServer",
            //Payload = ("{\"gameCode\": \"asd123\"}"),
            //Payload = ("{gameCode:" + gameCode + "}"),
            Payload = ("{\"gameCode\": \"" + gameCode + "\"}"),
            InvocationType = InvocationType.RequestResponse
        };


        client.InvokeAsync(request,
            (response) =>
            {
                if (response.Exception == null)
                {
                    if (response.Response.StatusCode == 200)
                    {
                        var payload = Encoding.ASCII.GetString(response.Response.Payload.ToArray()) + "\n";
                        var playerSessionObj = JsonUtility.FromJson<PlayerSessionObject>(payload);

                        if (playerSessionObj.FleetId == null)
                        {
                            Debug.Log($"Error in Lambda: {payload}");
                        }
                        else
                        {
                            QForMainThread(ActionConnectToServer, playerSessionObj.IpAddress, Int32.Parse(playerSessionObj.Port), playerSessionObj.PlayerSessionId);
                        }
                    }
                }
                else
                {
                    Debug.LogError(response.Exception);
                }
            });
    }

    private void OnOpenEvent(object sender, EventArgs e)
    {
        Debug.Log("[server-sent] OnOpenEvent");
    }

    private void OnCloseEvent(object sender, EventArgs e)
    {
        Debug.Log("[server-sent] OnCloseEvent");
    }

    private void OnConnectionErrorEvent(object sender, Aws.GameLift.Realtime.Event.ErrorEventArgs e)
    {
        Debug.Log($"[client] Connection Error! : ");
    }

    private void OnDataReceived(object sender, DataReceivedEventArgs e)
    {
        string data = System.Text.Encoding.Default.GetString(e.Data);
        Debug.Log($"[server-sent] OnDataReceived - Sender: {e.Sender} OpCode: {e.OpCode} data: {data}");

        switch (e.OpCode)
        {
            case Constants.LOGIN_RESPONSE_OP_CODE:
                _peerID = e.Sender;
                Debug.Log($"[client] peer ID : {_peerID}");
                break;

            case LOGICAL_PLAYER_OP_CODE:
                {
                    int logicalPlayer = -1;
                    if (int.TryParse(data, out logicalPlayer))
                    {
                        if (logicalPlayer == 0 || logicalPlayer == 1)
                        {
                            _logicalPlayerID = logicalPlayer;
                            Debug.Log($"Logical player ID assigned: {_logicalPlayerID}");
                        }
                        else
                        {
                            Debug.LogWarning($"Server tried to assign a logical player out of range: {logicalPlayer}");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Unable to parse logicalPlayer!");
                    }
                    break;
                }

            case MOVE_PLAYER_OP_CODE:
                {
                    int logicalPlayer = -1;
                    float distance = -1;
                    // Move remote user
                    string[] parts = data.Split(':');
                    if (!int.TryParse(parts[0], out logicalPlayer))
                    {
                        Debug.LogWarning("Unable to parse logicalPlayer!");
                    }
                    if (!float.TryParse(parts[1], out distance))
                    {
                        Debug.LogWarning("Unable to parse distance!");
                    }
                    QForMainThread(NotifyMovePlayer, logicalPlayer, distance);
                    break;
                }

            case WINNER_DETERMINED_OP_CODE:
                {

                    // Player won
                    int winner = -1;
                    int loser = -1;
                    string[] parts = data.Split(':');
                    if (!int.TryParse(parts[0], out winner))
                    {
                        Debug.LogWarning("Unable to parse winner!");
                    }
                    if (!int.TryParse(parts[1], out loser))
                    {
                        Debug.LogWarning("Unable to parse loser!");
                    }
                    QForMainThread(NotifyWinnerDetermined, winner, loser);
                    break;
                }
        }
    }

    private Queue<Action> _mainThreadQueue = new Queue<Action>();

    private void QForMainThread(Action fn)
    {
        lock (_mainThreadQueue)
        {
            _mainThreadQueue.Enqueue(() => { fn(); });
        }
    }

    private void QForMainThread<T1>(Action<T1> fn, T1 p1)
    {
        lock (_mainThreadQueue)
        {
            _mainThreadQueue.Enqueue(() => { fn(p1); });
        }
    }

    private void QForMainThread<T1, T2>(Action<T1, T2> fn, T1 p1, T2 p2)
    {
        lock (_mainThreadQueue)
        {
            _mainThreadQueue.Enqueue(() => { fn(p1, p2); });
        }
    }

    private void QForMainThread<T1, T2, T3>(Action<T1, T2, T3> fn, T1 p1, T2 p2, T3 p3)
    {
        lock (_mainThreadQueue)
        {
            _mainThreadQueue.Enqueue(() => { fn(p1, p2, p3); });
        }
    }


    private void RunMainThreadQueueActions()
    {
        // as our server messages come in on their own thread
        // we need to queue them up and run them on the main thread
        // when the methods need to interact with Unity
        lock (_mainThreadQueue)
        {
            while (_mainThreadQueue.Count > 0)
            {
                _mainThreadQueue.Dequeue().Invoke();
            }
        }
    }








}
