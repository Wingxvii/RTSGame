#pragma once

#include "PluginSettings.h"
#include <string>
#include <fstream>
#include <vector>

class PLUGIN_API MapLoader
{
public:
	static MapLoader* getInstance() {
		if (!instance) {
			instance = new MapLoader();
		}
		return instance;
	}

	void loadMap();
	void saveItem(int t, float x, float y, float z);
	void clearFile();
	int getObjectAmount();
	int getType(int obj);
	float getX(int obj);
	float getY(int obj);
	float getZ(int obj);
private:
	static MapLoader* instance;

	MapLoader() {

	};

	std::string filepath = "map.txt";
	std::vector<std::string> tokenize(std::string text);
	std::vector<int> type;
	std::vector<float> x;
	std::vector<float> y;
	std::vector<float> z;
};

#ifdef __cplusplus
extern "C"
{
#endif
	// Put your functions here
	PLUGIN_API void loadMap();
	PLUGIN_API void saveItem(int t, float x, float y, float z);
	PLUGIN_API void clearFile();
	PLUGIN_API int getObjectAmount();
	PLUGIN_API int getType(int obj);
	PLUGIN_API float getX(int obj);
	PLUGIN_API float getY(int obj);
	PLUGIN_API float getZ(int obj);
#ifdef __cplusplus
}
#endif