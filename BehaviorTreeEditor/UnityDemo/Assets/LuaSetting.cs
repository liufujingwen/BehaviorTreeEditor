using R7BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaSetting : MonoBehaviour
{
    private string m_LuaPrefix;
    private LuaEnv m_LuaEnv = new LuaEnv();

    // Use this for initialization
    void Start()
    {
        m_LuaPrefix = (Application.dataPath + "/LuaScript").Replace("\\", "/");
        m_LuaEnv.AddLoader(CustomLoader);
        LuaNodeProxy.LuaEnv = m_LuaEnv;

        string luaFile = m_LuaPrefix + "/RequireFile.lua.txt";
        byte[] bytes = File.ReadAllBytes(luaFile);
        m_LuaEnv.DoString(bytes, luaFile);
    }

    private byte[] CustomLoader(ref string filepath)
    {
        byte[] bytes = null;
        string path = Path.Combine(m_LuaPrefix, filepath);
        if (File.Exists(path))
        {
            bytes = File.ReadAllBytes(path);
        }
        return bytes;
    }

    private void Update()
    {
        if (m_LuaEnv != null)
        {
            m_LuaEnv.Tick();
        }
    }
}
