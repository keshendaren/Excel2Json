#pragma once

#include "../../Framework/Global.h"
#include "ConfigDef.h"


class CfgMgr : public Singleton<CfgMgr>
{
public:

////////////////////////////////////CfgTools Map Define///////////////////////////////////////////////////
	CfgMap<ConstCfg> mConstCfgs;

	CfgMap<ENConstCfg> mENConstCfgs;

	CfgMap<ENTest1Cfg> mENTest1Cfgs;

	CfgMap<ENTest2Cfg> mENTest2Cfgs;

	CfgMap<ENTest3Cfg> mENTest3Cfgs;

	CfgMap<Test1Cfg> mTest1Cfgs;

	CfgMap<Test2Cfg> mTest2Cfgs;

	CfgMap<Test3Cfg> mTest3Cfgs;
////////////////////////////////////CfgTools Map Define///////////////////////////////////////////////////
	
	// 初始化 加载所有配置
	void Init()
	{
////////////////////////////////////CfgTools Map Load///////////////////////////////////////////////////
		mConstCfgs.LoadCfg("ConstCfg");

		mENConstCfgs.LoadCfg("ENConstCfg");

		mENTest1Cfgs.LoadCfg("ENTest1Cfg");

		mENTest2Cfgs.LoadCfg("ENTest2Cfg");

		mENTest3Cfgs.LoadCfg("ENTest3Cfg");

		mTest1Cfgs.LoadCfg("Test1Cfg");

		mTest2Cfgs.LoadCfg("Test2Cfg");

		mTest3Cfgs.LoadCfg("Test3Cfg");
////////////////////////////////////CfgTools Map Load///////////////////////////////////////////////////

	}

};


#define gCfgMgr CfgMgr::get_singleton()