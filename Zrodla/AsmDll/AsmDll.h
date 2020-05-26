// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the BLENDALGORITHMASM_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// ASMDLL_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef ASMDLL_EXPORTS
#define ASMDLL_API __declspec(dllexport)
#else
#define ASMDLL_API __declspec(dllimport)
#endif

// This class is exported from the AsmDll.dll
class ASMDLL_API CAsmDll {
public:
	CAsmDll(void);
	// TODO: add your methods here.
};

extern ASMDLL_API int nAsmDll;

ASMDLL_API int fnAsmDll(void);
