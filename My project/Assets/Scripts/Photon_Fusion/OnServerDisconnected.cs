using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asteroids.HostSimple
{
    public class OnServerDisconnected : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] private string _menuSceneName = "Title";

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            // Shuts down the local NetworkRunner when the client is disconnected from the server.
            GetComponent<NetworkRunner>().Shutdown();
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            // Can check if the Runner is being shutdown because of the Host Migration
            if (shutdownReason == ShutdownReason.HostMigration)
            {
                // ...
            }
            else
            {
                SceneManager.LoadScene(_menuSceneName);
                // Or a normal Shutdown
            }
            // When the local NetworkRunner has shut down, the menu scene is loaded.
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }
        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }
        public void OnConnectedToServer(NetworkRunner runner)
        {
        }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            // Step 2.1
            // Shutdown the current Runner, this will not be used anymore. Perform any prior setup and tear down of the old Runner

            // The new "ShutdownReason.HostMigration" can be used here to inform why it's being shut down in the "OnShutdown" callback
            await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

            // Step 2.2
            // Create a new Runner.
            var newRunner = Instantiate(GetComponent<NetworkRunner>());

            // setup the new runner...

            // Start the new Runner using the "HostMigrationToken" and pass a callback ref in "HostMigrationResume".
            StartGameResult result = await newRunner.StartGame(new StartGameArgs()
            {
                // SessionName = SessionName,              // ignored, peer never disconnects from the Photon Cloud
                // GameMode = gameMode,                    // ignored, Game Mode comes with the HostMigrationToken
                HostMigrationToken = hostMigrationToken,   // contains all necessary info to restart the Runner
                HostMigrationResume = HostMigrationResume, // this will be invoked to resume the simulation
                                                           // other args
            });

            // Check StartGameResult as usual
            if (result.Ok == false)
            {
                Debug.LogWarning(result.ShutdownReason);
            }
            else
            {
                Debug.Log("Done");
            }
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }
        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }
        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        void HostMigrationResume(NetworkRunner runner)
        {

            // Get a temporary reference for each NO from the old Host
            foreach (var resumeNO in runner.GetResumeSnapshotNetworkObjects())

                if (
                    // Extract any NetworkBehavior used to represent the position/rotation of the NetworkObject
                    // this can be either a NetworkTransform or a NetworkRigidBody, for example
                    resumeNO.TryGetBehaviour<NetworkPositionRotation>(out var posRot))
                {

                    runner.Spawn(resumeNO, position: posRot.ReadPosition(), rotation: posRot.ReadRotation(), onBeforeSpawned: (runner, newNO) =>
                    {
                        // One key aspects of the Host Migration is to have a simple way of restoring the old NetworkObjects state
                        // If all state of the old NetworkObject is all what is necessary, just call the NetworkObject.CopyStateFrom
                        newNO.CopyStateFrom(resumeNO);

                        // and/or

                        // If only partial State is necessary, it is possible to copy it only from specific NetworkBehaviours
                        if (resumeNO.TryGetBehaviour<NetworkBehaviour>(out var myCustomNetworkBehaviour))
                        {
                            newNO.GetComponent<NetworkBehaviour>().CopyStateFrom(myCustomNetworkBehaviour);
                        }
                    });
                }
        }
    }
}
