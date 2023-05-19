using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

namespace Asteroids.HostSimple
{
    public class LocalInputPoller : MonoBehaviour, INetworkRunnerCallbacks
    {
        private const string AXIS_MOUSE_X = "Mouse X";
        private const string AXIS_MOUSE_Y = "Mouse Y";
        private const string BUTTON_FIRE1 = "Fire1";
        private const string BUTTON_FIRE2 = "Fire2";

        // The INetworkRunnerCallbacks of this LocalInputPoller are automatically detected
        // because the script is located on the same object as the NetworkRunner and
        // NetworkRunnerCallbacks scripts.

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            if (runner.GetPlayerObject(runner.LocalPlayer) == null)
                return;
            PlaneInput localInput = new PlaneInput();
            Player p = runner.GetPlayerObject(runner.LocalPlayer).GetComponent<Player>();
            Vector3 vec = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - p.myPlane.transform.position);
            Vector3 view = new Vector3(vec.x, 0, vec.z).normalized;
            localInput.ViewVec = view;
            localInput.Buttons.Set(PlaneButtons.Fire1, Input.GetButton(BUTTON_FIRE1));
            localInput.Buttons.Set(PlaneButtons.Fire2, Input.GetButton(BUTTON_FIRE2));

            input.Set(localInput);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
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

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
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
    }
}
