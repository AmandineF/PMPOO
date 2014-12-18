#ifdef WANTDLLEXP
#define DLL _declspec(dllexport)
#define EXTERNC extern "C"
#else
#define DLL
#define EXTERNC
#endif
#include <vector>
using std::vector;
/*enum CaseMap {
	DESERT = 1,
	FORET = 2,
	MONTAGNE = 3,
	PLAINE = 4
};*/

class DLL Algo{
public:
	DLL static int** generationMap(int taille);
	DLL static int* placementJoueur(int taille);
	DLL static bool** suggestionCase(int taille, int xActuel, int yActuel, int type, int**, double ptMouvement);
}; 

