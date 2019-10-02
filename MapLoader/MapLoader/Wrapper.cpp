#include "Wrapper.h"

MapLoader mapLoader;

PLUGIN_API void loadMap()
{
	return mapLoader.loadMap();
}

PLUGIN_API void saveMap(float t, float x, float y, float z)
{
	return mapLoader.saveMap(t, x, y, z);
}

PLUGIN_API void clearFile()
{
	return mapLoader.clearFile();
}

PLUGIN_API int getObjectAmount()
{
	return mapLoader.getObjectAmount();
}

PLUGIN_API float getType(int obj)
{
	return mapLoader.getType(obj);
}

PLUGIN_API float getX(int obj)
{
	return mapLoader.getX(obj);
}

PLUGIN_API float getY(int obj)
{
	return mapLoader.getY(obj);
}

PLUGIN_API float getZ(int obj)
{
	return mapLoader.getZ(obj);
}
