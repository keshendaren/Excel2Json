using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;


/*
 需要自己定义最基本的三个类型, 之所以不写这里, 是方便自己扩展

// 二元组
public class Vec2
{
    public int x;
    public int y;
};

// 三元组
public class Vec3
{
    public int x;
    public int y;
    public int z;

};

// 配置基类
public class CfgBase
{
    public int cid;
}

 */




/// <summary>
/// 配置管理器
/// </summary>
public class CfgMgr {
    public static CfgMgr Instance = new CfgMgr();

    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool m_isInit = false;

    // 所有配置
    public static Dictionary<System.Type, CfgMapBase> AllCfgs;


    ////////////////////////////////////CfgTools Map Define///////////////////////////////////////////////////
	public static CfgMap<ConstCfg> ConstCfgMap = new CfgMap<ConstCfg>();

	public static CfgMap<ENConstCfg> ENConstCfgMap = new CfgMap<ENConstCfg>();

	public static CfgMap<ENTest1Cfg> ENTest1CfgMap = new CfgMap<ENTest1Cfg>();

	public static CfgMap<ENTest2Cfg> ENTest2CfgMap = new CfgMap<ENTest2Cfg>();

	public static CfgMap<ENTest3Cfg> ENTest3CfgMap = new CfgMap<ENTest3Cfg>();

	public static CfgMap<Test1Cfg> Test1CfgMap = new CfgMap<Test1Cfg>();

	public static CfgMap<Test2Cfg> Test2CfgMap = new CfgMap<Test2Cfg>();

	public static CfgMap<Test3Cfg> Test3CfgMap = new CfgMap<Test3Cfg>();
////////////////////////////////////CfgTools Map Define///////////////////////////////////////////////////

    // 初始化
    public void Init() {
        if (m_isInit)
            return;
        m_isInit = true;

        //AllCfgs = new Dictionary<System.Type, Dictionary<int, CfgBase>>();
        LoadAll();
    }


// 加载所有配置
private void LoadAll() {

		////////////////////////////////////CfgTools Map Load///////////////////////////////////////////////////
		ConstCfgMap.LoadCfg();
		AllCfgs.Add(typeof(ConstCfg), ConstCfgMap);

		ENConstCfgMap.LoadCfg();
		AllCfgs.Add(typeof(ENConstCfg), ENConstCfgMap);

		ENTest1CfgMap.LoadCfg();
		AllCfgs.Add(typeof(ENTest1Cfg), ENTest1CfgMap);

		ENTest2CfgMap.LoadCfg();
		AllCfgs.Add(typeof(ENTest2Cfg), ENTest2CfgMap);

		ENTest3CfgMap.LoadCfg();
		AllCfgs.Add(typeof(ENTest3Cfg), ENTest3CfgMap);

		Test1CfgMap.LoadCfg();
		AllCfgs.Add(typeof(Test1Cfg), Test1CfgMap);

		Test2CfgMap.LoadCfg();
		AllCfgs.Add(typeof(Test2Cfg), Test2CfgMap);

		Test3CfgMap.LoadCfg();
		AllCfgs.Add(typeof(Test3Cfg), Test3CfgMap);
////////////////////////////////////CfgTools Map Load///////////////////////////////////////////////////
        Debug.Log("CfgMgr.LoadAll() Finished");
	}


	//// 加载配置, 存到Dictionary;
	//private Dictionary<int, CfgBase> LoadCfg<T>() where T : CfgBase
 //   {
 //       Dictionary<int, CfgBase> _Dic = new Dictionary<int, CfgBase>();

 //       string fileName = typeof(T).Name;
	//	string jsonStr = ResourcesManager.Loader.LoadTextAsset_Cfg(fileName).text;
 //       _Dic.Clear();

 //       try
 //       {
 //           JsonData jsonData = JsonMapper.ToObject(jsonStr);
 //           for (int i = 0; i < jsonData.Count; i++)
 //           {
 //               string idStr = jsonData[i]["cid"].ToString().Trim();
 //               int cid = Convert.ToInt32(idStr);
 //               T cfg = JsonMapper.ToObject<T>(jsonData[i].ToJson());
 //               _Dic.Add(cid, cfg);
 //           }
 //       }
 //       catch (Exception ex)
 //       {
 //           Debug.LogError(fileName + "  LoadCfg Error, ex:" + ex.ToString());
 //       }

 //       AllCfgs.Add(typeof(T), _Dic);
 //       return _Dic;
 //   }

	//// 根据Key 获取配置项
	//public static T Get<T>(int _cid, Dictionary<int, CfgBase> _Dic, bool _isShowErr = true) where T:CfgBase {
	//	if(_Dic.TryGetValue(_cid, out var cfg))
 //           return cfg as T;

	//	if(_isShowErr)
	//		string.Format("配置缺失. 表:{0}, cid:{1}", typeof(T).Name, _cid);

	//	return null;
	//}

	// 通用获取配置. *:有一次类型查找过程 如果在乎效率的话 建议直接从map获取.
	public static T Get<T>(int _cid, bool _isShowErr = true) where T : CfgBase
    {
        //AllCfgs.TryGetValue(typeof(T), out var cfgMap);
        //      if(!cfgMap.TryGetValue(_cid, out var cfg ))
        //{
        //          if (_isShowErr)
        //              string.Format("配置缺失. 表:{0}, cid:{1}", typeof(T).Name, _cid);
        //          return null;
        //}
        //return cfg as T;

        if (!AllCfgs.TryGetValue(typeof(T), out var _cfgMap))
		{
            if (_isShowErr)
				string.Format("配置缺失. 表:{0}, cid:{1}", typeof(T).Name, _cid);
			return null;
        }

        CfgMap<T> cfgMap = _cfgMap as CfgMap<T>;
        return cfgMap.Get(_cid, _isShowErr);
    }

//////////////////////////////////////CfgTools Map Get///////////////////////////////////////////////////

//	public static LvCfg GetLvCfg(int _cid, bool _isShowErr = true)
//	{
//		return Get<LvCfg>(_cid, LvCfgMap, _isShowErr);
//	}

//	public static LingGenAttrCfg GetLingGenAttrCfg(int _cid, bool _isShowErr = true)
//	{
//		return Get<LingGenAttrCfg>(_cid, LingGenAttrCfgMap, _isShowErr);
//	}

//	public static LingGenCfg GetLingGenCfg(int _cid, bool _isShowErr = true)
//	{
//		return Get<LingGenCfg>(_cid, LingGenCfgMap, _isShowErr);
//	}

//	public static AttrCfg GetAttrCfg(int _cid, bool _isShowErr = true)
//	{
//		return Get<AttrCfg>(_cid, AttrCfgMap, _isShowErr);
//	}

//	public static FTest1Cfg GetFTest1Cfg(int _cid, bool _isShowErr = true)
//	{
//		return Get<FTest1Cfg>(_cid, FTest1CfgMap, _isShowErr);
//	}

//	public static FTest2Cfg GetFTest2Cfg(int _cid, bool _isShowErr = true)
//	{
//		return Get<FTest2Cfg>(_cid, FTest2CfgMap, _isShowErr);
//	}

//	public static FTest3Cfg GetFTest3Cfg(int _cid, bool _isShowErr = true)
//	{
//		return Get<FTest3Cfg>(_cid, FTest3CfgMap, _isShowErr);
//	}
//////////////////////////////////////CfgTools Map Get///////////////////////////////////////////////////

}


