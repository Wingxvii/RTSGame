#pragma once
#ifdef MAPLOADER_EXPORTS
#define PLUGIN_API __declspec(dllexport)
#elif MAPLOADER_IMPORTS
#define PLUGIN_API __declspec(dllimport)
#else
#define PLUGIN_API
#endif
