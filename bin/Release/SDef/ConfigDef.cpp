// 配置结构体从Json解析
// 此文件由配置工具导出

#include "ConfigDef.h"



int ConstCfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		key = _jObj["key"].get<string>();
		val = _jObj["val"].get<int32>();
		valAry = _jObj["valAry"].get<vector<int>>();
		strVal = _jObj["strVal"].get<string>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}



int ENConstCfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		key = _jObj["key"].get<string>();
		val = _jObj["val"].get<int32>();
		valAry = _jObj["valAry"].get<vector<int>>();
		strVal = _jObj["strVal"].get<string>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}



int ENTest1Cfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		id = _jObj["id"].get<int32>();
		name = _jObj["name"].get<string>();
		pos = _jObj["pos"].get<Vec2>();
		color = _jObj["color"].get<Vec3>();
		pos2 = _jObj["pos2"].get<Vec3>();
		npcList = _jObj["npcList"].get<vector<int>>();
		flag = _jObj["flag"].get<int32>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}



int ENTest2Cfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		id = _jObj["id"].get<int32>();
		name = _jObj["name"].get<string>();
		function = _jObj["function"].get<vector<int>>();
		building = _jObj["building"].get<vector<vector<int>>>();
		npc = _jObj["npc"].get<vector<Vec3>>();
		buildingRole = _jObj["buildingRole"].get<vector<vector<int>>>();
		buildingRole2 = _jObj["buildingRole2"].get<vector<Vec2>>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}



int ENTest3Cfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		id = _jObj["id"].get<int32>();
		name = _jObj["name"].get<string>();
		hp = _jObj["hp"].get<int32>();
		attack = _jObj["attack"].get<int32>();
		defence = _jObj["defence"].get<int32>();
		actPoints = _jObj["actPoints"].get<int32>();
		actSpeed = _jObj["actSpeed"].get<int32>();
		hit = _jObj["hit"].get<int32>();
		dodge = _jObj["dodge"].get<int32>();
		critical = _jObj["critical"].get<int32>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}



int Test1Cfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		id = _jObj["id"].get<int32>();
		name = _jObj["name"].get<string>();
		pos = _jObj["pos"].get<Vec2>();
		color = _jObj["color"].get<Vec3>();
		pos2 = _jObj["pos2"].get<Vec3>();
		npcList = _jObj["npcList"].get<vector<int>>();
		flag = _jObj["flag"].get<int32>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}



int Test2Cfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		id = _jObj["id"].get<int32>();
		name = _jObj["name"].get<string>();
		function = _jObj["function"].get<vector<int>>();
		building = _jObj["building"].get<vector<vector<int>>>();
		npc = _jObj["npc"].get<vector<Vec3>>();
		buildingRole = _jObj["buildingRole"].get<vector<vector<int>>>();
		buildingRole2 = _jObj["buildingRole2"].get<vector<Vec2>>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}



int Test3Cfg::Parse(json& _jObj){
	if (_jObj.is_null())
		return -1;

	try {
		id = _jObj["id"].get<int32>();
		name = _jObj["name"].get<string>();
		hp = _jObj["hp"].get<int32>();
		attack = _jObj["attack"].get<int32>();
		defence = _jObj["defence"].get<int32>();
		actPoints = _jObj["actPoints"].get<int32>();
		actSpeed = _jObj["actSpeed"].get<int32>();
		hit = _jObj["hit"].get<int32>();
		dodge = _jObj["dodge"].get<int32>();
		critical = _jObj["critical"].get<int32>();
	}
	catch (const std::exception& e) {
		LogErr("FuncCfg::Parse, cid:%d, e:%s", cid, e.what());
		return -2;
	}

	return 0;
}


