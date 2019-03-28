using UnityEngine;
using UnityEngine.UI;

public delegate void Callback();
public class NetworkManager : Photon.PunBehaviour {

    public static NetworkManager instance;
	public const string version = "1.0";
    public Text status;
    private Callback onConnected;
    private Callback onConnectionFailed;
    private Callback onRoomJoined;

    void Awake() {
        instance = this;
    }

    public void Connect(Callback onConnected, Callback onConnectionFailed) {
        this.onConnected = onConnected;
        this.onConnectionFailed = onConnectionFailed;
        PhotonNetwork.ConnectUsingSettings(version);
    }

    void Update() {
        status.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    public override void OnConnectionFail(DisconnectCause cause) {
        onConnectionFailed();
        onConnectionFailed = null;
    }

    public override void OnJoinedLobby() {
        onConnected();
        onConnected = null;
    }

    public override void OnJoinedRoom() {
        onRoomJoined();
        onRoomJoined = null;
    }

    public void JoinOrCreateRoom(Callback onRoomJoined) {
        RoomOptions roomOptions = new RoomOptions() {IsVisible = true, MaxPlayers = 4};
        PhotonNetwork.JoinOrCreateRoom("TEST_ROOM", roomOptions, TypedLobby.Default);
        this.onRoomJoined = onRoomJoined;
    }
}
