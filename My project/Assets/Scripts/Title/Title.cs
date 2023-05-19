using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] RankingPopup rankingpopup;
    [SerializeField] InputField nickname;
    [SerializeField] InputField roomname;

    [SerializeField] PlayerData _playerDataPrefab = null; 
    [SerializeField] NetworkRunner _networkRunnerPrefab = null;

    private NetworkRunner _runner = null;

    // Update is called once per frame
    void Start()
    {
        nickname.placeholder.GetComponent<Text>().text = string.Format($"Player{Random.Range(0, 9999).ToString("0000")}");
    }

    public void OnClickHost()
    {
        SetPlayerData();
        StartGame(GameMode.AutoHostOrClient, roomname.text, 1);
    }
    public void OnClickGuest()
    {
        SetPlayerData();
        StartGame(GameMode.Client, roomname.text, 1);
    }
    public void OnClickRanking()
    {
        if (rankingpopup == null)
        {
            Debug.Log("RankingPupup is null");
            return;
        }

    }

    private void SetPlayerData()
    {
        var playerData = FindObjectOfType<PlayerData>();
        if (playerData == null)
        {
            playerData = Instantiate(_playerDataPrefab);
        }

        if (string.IsNullOrWhiteSpace(nickname.text))
        {
            playerData.SetNickName(nickname.placeholder.GetComponent<Text>().text);
        }
        else
        {
            playerData.SetNickName(nickname.text);
        }
    }
    private async void StartGame(GameMode mode, string roomName, int scenebuildindex)
    {
        _runner = FindObjectOfType<NetworkRunner>();
        if (_runner == null)
        {
            _runner = Instantiate(_networkRunnerPrefab);
            _runner.AddCallbacks();
        }

        // Let the Fusion Runner know that we will be providing user input
        _runner.ProvideInput = true;


        // GameMode.Host = Start a session with a specific name
        // GameMode.Client = Join a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            SceneManager = _runner.GetComponent<NetworkSceneManagerDefault>(),
            ObjectPool = _runner.GetComponent<NetworkPools>()
        }) ;
        _runner.SetActiveScene(scenebuildindex);
    }
}
