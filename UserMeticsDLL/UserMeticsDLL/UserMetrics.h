#pragma once

#include "PluginSeetings.h"
#include <string>
#include <fstream>

class PLUGIN_API UserMetrics
{
public:
	static UserMetrics* getInstance() {
		if (!instance) {
			instance = new UserMetrics();
		}
		return instance;
	}

	void updateFile();
	void clearFile();
	void buildingIncrease();
	void turretIncrease();
	void droidIncrease();
	void creditEarnedIncrease(int amount);
	void creditSpentIncrease(int amount);

private:
	static UserMetrics* instance;

	UserMetrics()
		: update(false), buildingBuilt(0), turretBuilt(0), droidBuilt(0),
		creditEarned(0), creditSpent(0)
	{
	};

	bool update;
	int buildingBuilt;
	int turretBuilt;
	int droidBuilt;
	int creditEarned;
	int creditSpent;

	std::string filePath = "UserMetrics.txt";

};


#ifdef __cplusplus
extern "C"
{
#endif
	// Put your functions here
	PLUGIN_API void UpdateFile();
	PLUGIN_API void ClearFile();
	PLUGIN_API void BuildingIncrease();
	PLUGIN_API void TurretIncrease();
	PLUGIN_API void DroidIncrease();
	PLUGIN_API void CreditEarnedIncrease(int amount);
	PLUGIN_API void CreditSpentIncrease(int amount);
#ifdef __cplusplus
}
#endif