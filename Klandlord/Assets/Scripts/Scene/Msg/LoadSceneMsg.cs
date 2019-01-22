using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LoadSceneMsg
{
    public int SceneBuildInex;
    public string SceneBuildName;
    public Action OnSceneLoad;

    public LoadSceneMsg()
    {
        SceneBuildInex = -1;
        SceneBuildName = null;
        OnSceneLoad = null;
    }

    public LoadSceneMsg(string name, Action loaded)
    {
        SceneBuildInex = -1;
        SceneBuildName = name;
        OnSceneLoad = loaded;
    }

    public LoadSceneMsg(int index, Action loaded)
    {
        SceneBuildInex = index;
        SceneBuildName = null;
        OnSceneLoad = loaded;
    }


}

