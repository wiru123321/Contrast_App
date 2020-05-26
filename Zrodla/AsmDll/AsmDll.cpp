// AsmDll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "AsmDll.h"



ASMDLL_API int nAsmDll=0;


ASMDLL_API int fnAsmDll(void)
{
	return 42;
}


CAsmDll::CAsmDll()
{
	return;
}
