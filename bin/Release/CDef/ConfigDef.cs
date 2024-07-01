using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 1-常量表.xlsx.ConstCfg
/// </summary>
public class ConstCfg : CfgBase
{
	/// <summary>
	/// 关键字
	/// </summary>
	public string key{get;set;}

	/// <summary>
	/// 整数值
	/// </summary>
	public int val{get;set;}

	/// <summary>
	/// 整数列表
	/// </summary>
	public List<int> valAry{get;set;}

	/// <summary>
	/// 字符串值
	/// </summary>
	public string strVal{get;set;}

	/// <summary>
	/// 描述
	/// </summary>
	public string desc{get;set;}

}

/// <summary>
/// 101-ConstTable.xlsx.ENConstCfg
/// </summary>
public class ENConstCfg : CfgBase
{
	/// <summary>
	/// 关键字
	/// </summary>
	public string key{get;set;}

	/// <summary>
	/// 整数值
	/// </summary>
	public int val{get;set;}

	/// <summary>
	/// 整数列表
	/// </summary>
	public List<int> valAry{get;set;}

	/// <summary>
	/// 字符串值
	/// </summary>
	public string strVal{get;set;}

	/// <summary>
	/// 描述
	/// </summary>
	public string desc{get;set;}

}

/// <summary>
/// 102-Test1Table.xlsx.ENTest1Cfg
/// </summary>
public class ENTest1Cfg : CfgBase
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id{get;set;}

	/// <summary>
	/// 名称
	/// </summary>
	public string name{get;set;}

	/// <summary>
	/// 坐标
	/// </summary>
	public Vec2 pos{get;set;}

	/// <summary>
	/// 代表色
	/// </summary>
	public Vec3 color{get;set;}

	/// <summary>
	/// 坐标2
	/// </summary>
	public Vec3 pos2{get;set;}

	/// <summary>
	/// 坐标2
	/// </summary>
	public List<int> npcList{get;set;}

	/// <summary>
	/// 旗帜
	/// </summary>
	public int flag{get;set;}

	/// <summary>
	/// 描述
	/// </summary>
	public string desc{get;set;}

	/// <summary>
	/// 名字列表
	/// </summary>
	public List<string> nameList{get;set;}

}

/// <summary>
/// 103-Test2Table.xlsx.ENTest2Cfg
/// </summary>
public class ENTest2Cfg : CfgBase
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id{get;set;}

	/// <summary>
	/// 名称
	/// </summary>
	public string name{get;set;}

	/// <summary>
	/// 功能
	/// </summary>
	public List<int> function{get;set;}

	/// <summary>
	/// 建筑
	/// </summary>
	public List<List<int>> building{get;set;}

	/// <summary>
	/// NPC
	/// </summary>
	public List<Vec3> npc{get;set;}

	/// <summary>
	/// 建筑
	/// </summary>
	public List<List<int>> buildingRole{get;set;}

	/// <summary>
	/// 建筑
	/// </summary>
	public List<Vec2> buildingRole2{get;set;}

}

/// <summary>
/// 104-Test3Table.xlsx.ENTest3Cfg
/// </summary>
public class ENTest3Cfg : CfgBase
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id{get;set;}

	/// <summary>
	/// 名称
	/// </summary>
	public string name{get;set;}

	/// <summary>
	/// 资源编号
	/// </summary>
	public string assetName{get;set;}

	/// <summary>
	/// 血
	/// </summary>
	public int hp{get;set;}

	/// <summary>
	/// 攻击
	/// </summary>
	public int attack{get;set;}

	/// <summary>
	/// 防御
	/// </summary>
	public int defence{get;set;}

	/// <summary>
	/// 战力
	/// </summary>
	public int actPoints{get;set;}

	/// <summary>
	/// 身法
	/// </summary>
	public int actSpeed{get;set;}

	/// <summary>
	/// 命中
	/// </summary>
	public int hit{get;set;}

	/// <summary>
	/// 闪避
	/// </summary>
	public int dodge{get;set;}

	/// <summary>
	/// 暴击
	/// </summary>
	public int critical{get;set;}

}

/// <summary>
/// 2-测试表1.xlsx.Test1Cfg
/// </summary>
public class Test1Cfg : CfgBase
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id{get;set;}

	/// <summary>
	/// 名称
	/// </summary>
	public string name{get;set;}

	/// <summary>
	/// 坐标
	/// </summary>
	public Vec2 pos{get;set;}

	/// <summary>
	/// 代表色
	/// </summary>
	public Vec3 color{get;set;}

	/// <summary>
	/// 坐标2
	/// </summary>
	public Vec3 pos2{get;set;}

	/// <summary>
	/// 坐标2
	/// </summary>
	public List<int> npcList{get;set;}

	/// <summary>
	/// 旗帜
	/// </summary>
	public int flag{get;set;}

	/// <summary>
	/// 描述
	/// </summary>
	public string desc{get;set;}

	/// <summary>
	/// 名字列表
	/// </summary>
	public List<string> nameList{get;set;}

}

/// <summary>
/// 3-测试表2.xlsx.Test2Cfg
/// </summary>
public class Test2Cfg : CfgBase
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id{get;set;}

	/// <summary>
	/// 名称
	/// </summary>
	public string name{get;set;}

	/// <summary>
	/// 功能
	/// </summary>
	public List<int> function{get;set;}

	/// <summary>
	/// 建筑
	/// </summary>
	public List<List<int>> building{get;set;}

	/// <summary>
	/// NPC
	/// </summary>
	public List<Vec3> npc{get;set;}

	/// <summary>
	/// 建筑
	/// </summary>
	public List<List<int>> buildingRole{get;set;}

	/// <summary>
	/// 建筑
	/// </summary>
	public List<Vec2> buildingRole2{get;set;}

}

/// <summary>
/// 4-测试表3.xlsx.Test3Cfg
/// </summary>
public class Test3Cfg : CfgBase
{
	/// <summary>
	/// 编号
	/// </summary>
	public int id{get;set;}

	/// <summary>
	/// 名称
	/// </summary>
	public string name{get;set;}

	/// <summary>
	/// 资源编号
	/// </summary>
	public string assetName{get;set;}

	/// <summary>
	/// 血
	/// </summary>
	public int hp{get;set;}

	/// <summary>
	/// 攻击
	/// </summary>
	public int attack{get;set;}

	/// <summary>
	/// 防御
	/// </summary>
	public int defence{get;set;}

	/// <summary>
	/// 战力
	/// </summary>
	public int actPoints{get;set;}

	/// <summary>
	/// 身法
	/// </summary>
	public int actSpeed{get;set;}

	/// <summary>
	/// 命中
	/// </summary>
	public int hit{get;set;}

	/// <summary>
	/// 闪避
	/// </summary>
	public int dodge{get;set;}

	/// <summary>
	/// 暴击
	/// </summary>
	public int critical{get;set;}

}

