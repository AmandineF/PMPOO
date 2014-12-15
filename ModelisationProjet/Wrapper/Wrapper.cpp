#include "Wrapper.h"

bool** Wrapper::WrapperAlgo::suggestion(int tailleMap, int xActuel, int yActuel, int type, array<array<int>^>^ carteElement, double ptMouvement){
	int** mapBis = new int*[tailleMap];
	for (int i = 0; i < tailleMap; i++) {
		mapBis[i] = new int[tailleMap];
		for (int j = 0; j < tailleMap; j++) {
			mapBis[i][j] = (int)carteElement[i][j];
		}
	}
	bool** mapBool = Algo::suggestionCase(tailleMap, xActuel, yActuel, type, mapBis, ptMouvement);
	return mapBool;
}