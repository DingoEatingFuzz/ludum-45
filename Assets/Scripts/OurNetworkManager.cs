using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

class PathModel {
    public List<Vector3> path;
}

[System.Serializable]
public class LevelRecord
{
    public GameObject LevelPrefab;
    public float MaxInk;
    public int id;
}

public class OurNetworkManager : NetworkManagerBehavior
{
    public GameObject lineTemplate;
    public bool Debugging = false;
    public List<LevelRecord> levels;
    private LevelRecord curLevel;
    private GameObject curLevelObj; 

    IEnumerator Countdown()
    {
        Debug.Log("Countdown started...");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Setting level...");
        this.SetLevel(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting countdown...");
        StartCoroutine(Countdown());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetLevel() {
        Debug.Log("RPC ResetLevel");
        if (this.Debugging) {
            this._resetLevel();
            this._setInkLevel(1);
        } else {
            this.networkObject.SendRpc(RPC_RESET_LEVEL, Receivers.All);
            this.networkObject.SendRpc(RPC_SET_INK_LEVEL, Receivers.All, 1.0f);
        }
    }

    public void AddLine(List<Vector3> path) {
        Debug.Log("RPC AddLine");
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
        Debug.Log("RPC SetInkLevel");
        if (this.Debugging)
        {
            this._setInkLevel(level);
        } else
        {
            this.networkObject.SendRpc(RPC_SET_INK_LEVEL, Receivers.All, level);
        }

    }

    public void SetLevel(int levelId)
    {
        Debug.Log($"RPC SetLevel ({levelId})");
        if (this.Debugging)
        {
            this._setLevel(levelId);
            this._setInkLevel(1);
        } else
        {
            this.networkObject.SendRpc(RPC_SET_LEVEL, Receivers.All, levelId);
            this.networkObject.SendRpc(RPC_SET_INK_LEVEL, Receivers.All, 1.0f);
        }
    }

    public void NextLevel()
    {
        if (curLevel != null)
        {
            this.SetLevel(curLevel.id + 1);
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
        Debug.Log("Outside main thread, in the setLevel");

        MainThreadManager.Run(() =>
        {
            Debug.Log("Are we ever here?????????");
            var levelId = args.GetNext<int>();
            this._setLevel(levelId);
        });
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

        FindObjectOfType<PlayerController>().ResetPlayerPosition();
    }

    private void _setInkLevel(float level)
    {
        FindObjectOfType<Painter>().setInkLevelPercent(level);
    }

    private void _setLevel(int levelId)
    {
        if (this.curLevelObj != null)
        {
            Destroy(this.curLevelObj);
        }

        this.curLevel = levels.Find(x => x.id == levelId);
        Debug.Log($"Current level: id: {curLevel.id}, {curLevel.MaxInk}");
        this.curLevelObj = Instantiate(curLevel.LevelPrefab, GameObject.Find("LevelZaddy").transform);
        this.curLevelObj.transform.Find("Character").GetComponent<PlayerController>().network = this;
        FindObjectOfType<Painter>().maxInk = this.curLevel.MaxInk;
        if (!Debugging)
        {
            if (!NetworkManager.Instance.IsServer)
            {
                this.curLevelObj.transform.Find("Character").gameObject.SetActive(false);
                var walls = GameObject.FindGameObjectsWithTag("wall");
                foreach (var wall in walls)
                {
                    wall.SetActive(false);
                }
            } else
            {
                FindObjectOfType<Painter>().readOnly = true;
            }
        }
    }

}
