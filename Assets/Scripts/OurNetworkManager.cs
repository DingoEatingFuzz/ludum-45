using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

class PathModel {
    public List<Vector3> path;
}

public class OurNetworkManager : NetworkManagerBehavior
{
    public GameObject lineTemplate;
    public bool Debugging = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetLevel() {
        if (this.Debugging) {
            this._resetLevel();
            this._setInkLevel(1);
        } else {
            this.networkObject.SendRpc(RPC_RESET_LEVEL, Receivers.All);
            this.networkObject.SendRpc(RPC_SET_INK_LEVEL, Receivers.All, 1);
        }
    }

    public void AddLine(List<Vector3> path) {
        // JSON encode path
        var model = new PathModel(){ path = path };
        var json = JsonUtility.ToJson(model);

        // send RPC
        if (this.Debugging) {
            this._sendPath(json);
        } else {
            Debug.Log($"Sending RPC... {json}");
            this.networkObject.SendRpc(RPC_SEND_PATH, Receivers.All, json);
        }
    }

    public void SetInkLevel(float level)
    {
        if (this.Debugging)
        {
            this._setInkLevel(level);
        } else
        {
            this.networkObject.SendRpc(RPC_SET_INK_LEVEL, Receivers.All, level);
        }

    }

    public override void resetLevel(RpcArgs args) {
        MainThreadManager.Run(() => {
            this._resetLevel();
        });
    }

    public override void sendPath(RpcArgs args) {
        MainThreadManager.Run(() => {
            var pathJson = args.GetNext<string>();
            Debug.Log($"Received RPC... {pathJson}");
            this._sendPath(pathJson);
        });
    }

    public override void victory(RpcArgs args) {

    }

    public override void setLevel(RpcArgs args) {

    }

    public override void setInkLevel(RpcArgs args) {
        MainThreadManager.Run(() =>
        {
            var level = args.GetNext<float>();
            this._setInkLevel(level);
        });

    }

    private void _sendPath(string json) {
        var model = JsonUtility.FromJson<PathModel>(json);

        var line = Instantiate(this.lineTemplate, new Vector3(0, 0, 0), new Quaternion());
        line.tag = "dynamic";

        var buildPath = line.GetComponent<BuildPath>();
        buildPath.Path = model.path;
    }

    private void _resetLevel() {
        Debug.Log("Deleting all the things!");
        var toDelete = GameObject.FindGameObjectsWithTag("dynamic");
        Debug.Log($"Deleting {toDelete.Length} Objects");
        foreach (var obj in toDelete)
        {
            Destroy(obj);
        }
    }

    private void _setInkLevel(float level)
    {
        FindObjectOfType<Painter>().setInkLevelPercent(level);
    }
}
