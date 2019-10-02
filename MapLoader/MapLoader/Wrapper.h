#pragma once
#include "PluginSettings.h"
#include "MapLoader.h"

#ifdef __cplusplus
extern "C"
{
#endif
	// Put your functions here
	PLUGIN_API void loadMap();
	PLUGIN_API void saveItem(float t, float x, float y, float z);
	PLUGIN_API void clearFile();
	PLUGIN_API int getObjectAmount();
	PLUGIN_API float getType(int obj);
	PLUGIN_API float getX(int obj);
	PLUGIN_API float getY(int obj);
	PLUGIN_API float getZ(int obj);
#ifdef __cplusplus
}
#endif