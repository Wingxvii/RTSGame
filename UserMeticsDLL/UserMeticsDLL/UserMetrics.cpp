#include "UserMetrics.h"

UserMetrics* UserMetrics::instance = 0;

void UserMetrics::updateFile()
{
	if (update)
	{
		std::ofstream saveFile;
		saveFile.open(filePath, std::fstream::app);
		saveFile.clear();
		saveFile << "Buildings Built: " << UserMetrics::buildingBuilt << "\n";
		saveFile << "Turrets Built: " << UserMetrics::turretBuilt << "\n";
		saveFile << "Droids Built: " << UserMetrics::droidBuilt << "\n";
		saveFile << "Credits Earned: " << UserMetrics::creditEarned << "\n";
		saveFile << "Credits Spent: " << UserMetrics::creditSpent << "\n";

		saveFile.close();
		update = false;
	}
}

void UserMetrics::clearFile()
{
	std::ofstream saveFile;
	saveFile.open(filePath);
	saveFile.clear();
	saveFile.close();
}

void UserMetrics::buildingIncrease()
{
	buildingBuilt++;
	update = true;
}

void UserMetrics::turretIncrease()
{
	turretBuilt++;
	update = true;
}

void UserMetrics::droidIncrease()
{
	droidBuilt++;
	update = true;
}

void UserMetrics::creditEarnedIncrease(int amount)
{
	creditEarned = creditEarned + amount;
	update = true;
}

void UserMetrics::creditSpentIncrease(int amount)
{
	creditSpent = creditSpent + amount;
	update = true;
}

PLUGIN_API void UpdateFile()
{
	UserMetrics::getInstance()->updateFile();
}

PLUGIN_API void ClearFile()
{
	UserMetrics::getInstance()->clearFile();
}

PLUGIN_API void BuildingIncrease()
{
	UserMetrics::getInstance()->buildingIncrease();
}

PLUGIN_API void TurretIncrease()
{
	UserMetrics::getInstance()->turretIncrease();
}

PLUGIN_API void DroidIncrease()
{
	UserMetrics::getInstance()->droidIncrease();
}

PLUGIN_API void CreditEarnedIncrease(int amount)
{
	UserMetrics::getInstance()->creditEarnedIncrease(amount);
}

PLUGIN_API void CreditSpentIncrease(int amount)
{
	UserMetrics::getInstance()->creditSpentIncrease(amount);
}
