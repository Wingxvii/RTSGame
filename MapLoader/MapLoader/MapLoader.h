#pragma once

#include "PluginSettings.h"
#include <string>
#include <fstream>
#include <vector>

class PLUGIN_API MapLoader
{
public:
	void loadMap();
	void saveMap(float t, float x, float y, float z);
	void clearFile();
	int getObjectAmount();
	float getType(int obj);
	float getX(int obj);
	float getY(int obj);
	float getZ(int obj);
private:
	std::string filepath = "Assets/Resources/map.txt";
	std::vector<std::string> tokenize(std::string text);
	std::vector<float> type;
	std::vector<float> x;
	std::vector<float> y;
	std::vector<float> z;
};