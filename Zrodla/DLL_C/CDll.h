#pragma once

#ifdef CDLL_EXPORTS
#define CDLL_API __declspec(dllexport)
#else
#define CDLL_API __declspec(dllimport)
#endif


class CDLL_API CDll {
public:
	CDll(void);
	
};

extern CDLL_API int nCDll;

CDLL_API int fnCDll(void);

