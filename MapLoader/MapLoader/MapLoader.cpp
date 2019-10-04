#include "MapLoader.h"

MapLoader* MapLoader::instance = 0;

PLUGIN_API void loadMap()
{
	MapLoader::getInstance()->loadMap();
}

PLUGIN_API void saveItem(int t, float x, float y, float z)
{
	MapLoader::getInstance()->saveItem(t, x, y, z);
}

PLUGIN_API void clearFile()
{
	MapLoader::getInstance()->clearFile();
}

PLUGIN_API int getObjectAmount()
{
	return MapLoader::getInstance()->getObjectAmount();
}

PLUGIN_API int getType(int obj)
{
	return MapLoader::getInstance()->getType(obj);
}

PLUGIN_API float getX(int obj)
{
	return MapLoader::getInstance()->getX(obj);
}

PLUGIN_API float getY(int obj)
{
	return MapLoader::getInstance()->getY(obj);
}

PLUGIN_API float getZ(int obj)
{
	return MapLoader::getInstance()->getZ(obj);
}

void MapLoader::loadMap()
{
	type.clear();
	x.clear();
	y.clear();
	z.clear();
	std::ifstream saveFile;
	saveFile.open(filepath);
	std::string data;

	while (getline(saveFile, data))
	{
		std::vector<std::string> parsedData = tokenize(data);
		type.push_back(std::atoi(parsedData[0].c_str()));
		x.push_back(std::stof(parsedData[1]));
		y.push_back(std::stof(parsedData[2]));
		z.push_back(std::stof(parsedData[3]));
	}
	saveFile.close();
}

void MapLoader::saveItem(int t, float x, float y, float z)
{
	std::ofstream saveFile;
	saveFile.open(filepath, std::fstream::app);
	saveFile << t << "," << x << "," << y << "," << z << "," << "\n";
	saveFile.close();
}

void MapLoader::clearFile()
{
	std::ofstream saveFile;
	saveFile.open(filepath);
	saveFile.clear();
	saveFile.close();
}

int MapLoader::getObjectAmount()
{
	return type.size();
}

int MapLoader::getType(int obj)
{
	return type[obj];
}

float MapLoader::getX(int obj)
{
	return x[obj];
}

float MapLoader::getY(int obj)
{
	return y[obj];
}

float MapLoader::getZ(int obj)
{
	return z[obj];
}

std::vector<std::string> MapLoader::tokenize(std::string text)
{
	char token = ',';
	std::vector<std::string> temp;
	int lastTokenLocation = 0;

	for (int i = 0; i < text.size(); i++)
	{
		if (text[i] == token)
		{
			temp.push_back(text.substr(lastTokenLocation, i - lastTokenLocation));
			lastTokenLocation = i + 1;
		}
	}
	return temp;
}