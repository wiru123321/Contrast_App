
#include "stdafx.h"
#include "CDll.h"
#include<iostream>
#include<cmath>
#include "nmmintrin.h" // for SSE4.2
#include"cstdint"


CDLL_API int nCDll = 0;
CDLL_API int fnCDll(void)
{
	return 42;
}
CDll::CDll()
{
	return;
}
extern "C"

	void __declspec(dllexport)  BitmapConvert(unsigned char* pixels, unsigned char* LUT)
	{
		__m128i tab; //vector tab
		tab = _mm_loadu_si128((__m128i*)LUT); // loading 128 bits of tab LUT
		_mm_storeu_si128((__m128i*)pixels, tab); //  Store tab(LUT) in pixels tab
	}
