// 配置结构体
// 此文件由配置工具导出

#pragma once
#include <vector>
#include <string>
#include "ConfigBase.h"

using namespace std;



/// <summary>
/// 1-常量表.xlsx.ConstCfg
/// </summary>
struct ConstCfg : CfgBase
{
	ConstCfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 关键字
	/// </summary>
	string key;

	/// <summary>
	/// 整数值
	/// </summary>
	int32 val;

	/// <summary>
	/// 整数列表
	/// </summary>
	vector<int> valAry;

	/// <summary>
	/// 字符串值
	/// </summary>
	string strVal;

};


/// <summary>
/// 101-ConstTable.xlsx.ENConstCfg
/// </summary>
struct ENConstCfg : CfgBase
{
	ENConstCfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 关键字
	/// </summary>
	string key;

	/// <summary>
	/// 整数值
	/// </summary>
	int32 val;

	/// <summary>
	/// 整数列表
	/// </summary>
	vector<int> valAry;

	/// <summary>
	/// 字符串值
	/// </summary>
	string strVal;

};


/// <summary>
/// 102-Test1Table.xlsx.ENTest1Cfg
/// </summary>
struct ENTest1Cfg : CfgBase
{
	ENTest1Cfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 编号
	/// </summary>
	int32 id;

	/// <summary>
	/// 名称
	/// </summary>
	string name;

	/// <summary>
	/// 坐标
	/// </summary>
	Vec2 pos;

	/// <summary>
	/// 代表色
	/// </summary>
	Vec3 color;

	/// <summary>
	/// 坐标2
	/// </summary>
	Vec3 pos2;

	/// <summary>
	/// 坐标2
	/// </summary>
	vector<int> npcList;

	/// <summary>
	/// 旗帜
	/// </summary>
	int32 flag;

};


/// <summary>
/// 103-Test2Table.xlsx.ENTest2Cfg
/// </summary>
struct ENTest2Cfg : CfgBase
{
	ENTest2Cfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 编号
	/// </summary>
	int32 id;

	/// <summary>
	/// 名称
	/// </summary>
	string name;

	/// <summary>
	/// 功能
	/// </summary>
	vector<int> function;

	/// <summary>
	/// 建筑
	/// </summary>
	vector<vector<int>> building;

	/// <summary>
	/// NPC
	/// </summary>
	vector<Vec3> npc;

	/// <summary>
	/// 建筑
	/// </summary>
	vector<vector<int>> buildingRole;

	/// <summary>
	/// 建筑
	/// </summary>
	vector<Vec2> buildingRole2;

};


/// <summary>
/// 104-Test3Table.xlsx.ENTest3Cfg
/// </summary>
struct ENTest3Cfg : CfgBase
{
	ENTest3Cfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 编号
	/// </summary>
	int32 id;

	/// <summary>
	/// 名称
	/// </summary>
	string name;

	/// <summary>
	/// 血
	/// </summary>
	int32 hp;

	/// <summary>
	/// 攻击
	/// </summary>
	int32 attack;

	/// <summary>
	/// 防御
	/// </summary>
	int32 defence;

	/// <summary>
	/// 战力
	/// </summary>
	int32 actPoints;

	/// <summary>
	/// 身法
	/// </summary>
	int32 actSpeed;

	/// <summary>
	/// 命中
	/// </summary>
	int32 hit;

	/// <summary>
	/// 闪避
	/// </summary>
	int32 dodge;

	/// <summary>
	/// 暴击
	/// </summary>
	int32 critical;

};


/// <summary>
/// 2-测试表1.xlsx.Test1Cfg
/// </summary>
struct Test1Cfg : CfgBase
{
	Test1Cfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 编号
	/// </summary>
	int32 id;

	/// <summary>
	/// 名称
	/// </summary>
	string name;

	/// <summary>
	/// 坐标
	/// </summary>
	Vec2 pos;

	/// <summary>
	/// 代表色
	/// </summary>
	Vec3 color;

	/// <summary>
	/// 坐标2
	/// </summary>
	Vec3 pos2;

	/// <summary>
	/// 坐标2
	/// </summary>
	vector<int> npcList;

	/// <summary>
	/// 旗帜
	/// </summary>
	int32 flag;

};


/// <summary>
/// 3-测试表2.xlsx.Test2Cfg
/// </summary>
struct Test2Cfg : CfgBase
{
	Test2Cfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 编号
	/// </summary>
	int32 id;

	/// <summary>
	/// 名称
	/// </summary>
	string name;

	/// <summary>
	/// 功能
	/// </summary>
	vector<int> function;

	/// <summary>
	/// 建筑
	/// </summary>
	vector<vector<int>> building;

	/// <summary>
	/// NPC
	/// </summary>
	vector<Vec3> npc;

	/// <summary>
	/// 建筑
	/// </summary>
	vector<vector<int>> buildingRole;

	/// <summary>
	/// 建筑
	/// </summary>
	vector<Vec2> buildingRole2;

};


/// <summary>
/// 4-测试表3.xlsx.Test3Cfg
/// </summary>
struct Test3Cfg : CfgBase
{
	Test3Cfg(int _cid) :CfgBase(_cid) {}
	virtual int Parse(json& _jObj);

	/// <summary>
	/// 编号
	/// </summary>
	int32 id;

	/// <summary>
	/// 名称
	/// </summary>
	string name;

	/// <summary>
	/// 血
	/// </summary>
	int32 hp;

	/// <summary>
	/// 攻击
	/// </summary>
	int32 attack;

	/// <summary>
	/// 防御
	/// </summary>
	int32 defence;

	/// <summary>
	/// 战力
	/// </summary>
	int32 actPoints;

	/// <summary>
	/// 身法
	/// </summary>
	int32 actSpeed;

	/// <summary>
	/// 命中
	/// </summary>
	int32 hit;

	/// <summary>
	/// 闪避
	/// </summary>
	int32 dodge;

	/// <summary>
	/// 暴击
	/// </summary>
	int32 critical;

};


