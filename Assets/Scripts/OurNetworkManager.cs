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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetLevel() {
        this.networkObject.SendRpc(RPC_RESET_LEVEL, Receivers.All);
    }

    public void AddLine(List<Vector3> path) {
        // JSON encode path
        var model = new PathModel(){ path = path };
        var json = JsonUtility.ToJson(model);
        Debug.Log($"Sending RPC... {json}");
        // send RPC
        this.networkObject.SendRpc(RPC_SEND_PATH, Receivers.All, json);
    }

    public override void resetLevel(RpcArgs args) {
        MainThreadManager.Run(() => {
            Debug.Log("Deleting all the things!");
            var toDelete = GameObject.FindGameObjectsWithTag("dynamic");
            Debug.Log($"Deleting {toDelete.Length} Objects");
            foreach (var obj in toDelete)
            {
                Destroy(obj);
            }
        });
    }

    public override void sendPath(RpcArgs args) {
        MainThreadManager.Run(() => {
            var pathJson = args.GetNext<string>();
            Debug.Log($"Received RPC... {pathJson}");
            var model = JsonUtility.FromJson<PathModel>(pathJson);

            var line = Instantiate(this.lineTemplate, new Vector3(0, 0, 0), new Quaternion());
            line.tag = "dynamic";

            var buildPath = line.GetComponent<BuildPath>();
            buildPath.Path = model.path;
        });
    }

    public override void victory(RpcArgs args) {

    }

    public override void setLevel(RpcArgs args) {

    }

    public override void setInkLevel(RpcArgs args) {

    }
}
