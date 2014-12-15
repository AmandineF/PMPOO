#ifndef __WRAPPER__
#define __WRAPPER__
#endif

#include "../Algo/Algo.h"

#pragma comment(lib, "../Debug/Algo.lib")

using namespace System;

namespace Wrapper {
	public ref class WrapperAlgo{
	private:
		Algo* algo;
	public:
		int** generationMap(int taille){ return algo->generationMap(taille); }
		int* placementJoueur(int taille){ return algo->placementJoueur(taille); }
		bool** suggestionCase(int taille, int xActuel, int yActuel, int type, int** carteElement, double ptMouvement){
			return algo->suggestionCase(taille, xActuel, yActuel, type, carteElement, ptMouvement);
		}
		bool** suggestion(int tailleMap, int xActuel, int yActuel, int type, array<array<int>^>^ carteElement, double ptMouvement);
	};
}
