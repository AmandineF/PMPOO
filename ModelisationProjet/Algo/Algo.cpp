#include "Algo.h"
#include <stdio.h>      /* printf, scanf, puts, NULL */
#include <stdlib.h>     /* srand, rand */
#include <time.h>       /* time */
#include <iostream>
#include <list>


int** Algo::generationMap(int taille){
	int i, j, Resrand, cptDesert, cptMontagne, cptForet, cptPlaine,cptMer;
	cptDesert = 0;
	cptMontagne = 0;
	cptForet = 0;
	cptPlaine = 0;
	cptMer = 0;
	int** carte = new int*[taille];
	for (i = 0; i<taille; i++) {
		carte[i] = new int[taille];
	}
	srand(time(NULL));
	bool b = true;
	for (i = 0; i < taille; i++)
	{
		for (j = 0; j < taille; j++)
		{
			while (b)
			{
				Resrand = rand() % 5 + 1;
				if (Resrand == 1 && (cptDesert < ((taille * taille) / 5)))
				{
					carte[i][j] = Resrand;
					cptDesert++;
					b = false;
				}
				if (Resrand == 2 && (cptForet < ((taille * taille) / 5)))
				{
					carte[i][j] = Resrand;
					cptForet++;
					b = false;
				}
				if (Resrand == 3 && (cptMontagne < ((taille * taille) / 5)))
				{
					carte[i][j] = Resrand;
					cptMontagne++;
					b = false;
				}
				if (Resrand == 4 && (cptPlaine < ((taille * taille) / 5)))
				{
					carte[i][j] = Resrand;
					cptPlaine++;
					b = false;
				}
				
				if (Resrand == 5 && (cptMer < ((taille * taille) / 5)))
				{
					carte[i][j] = Resrand;
					cptMer++;
					b = false;
				}
			}
			b = true;
		}
	}
	return carte;
}

int* Algo::placementJoueur(int taille){
	int Resrand;
	srand(time(NULL));
	int* tableauJoueur = new int[4];

	Resrand = rand() % 2 + 1;
	if (Resrand == 1)
	{
		tableauJoueur[0] = 0;
		tableauJoueur[1] = 0;
		tableauJoueur[2] = taille - 1;
		tableauJoueur[3] = taille - 1;

	}
	else
	{
		tableauJoueur[0] = taille - 1;
		tableauJoueur[1] = taille - 1;
		tableauJoueur[2] = 0;
		tableauJoueur[3] = 0;
	}
	return tableauJoueur;

}
// type = 1 -> Nain ; type = 2 -> Elfe ; type = 3 -> Orc ; type = 4 -> Pirate
bool** Algo::suggestionCase(int tailleMap, int xActuel, int yActuel, int type, int** carteElement, double ptMouvement){
	int i, j;

	// initialisation carteBool � faux 
	bool** carteBool = new bool*[tailleMap];
	for (i = 0; i < tailleMap; i++){
		carteBool[i] = new bool[tailleMap];
		for (j = 0; j < tailleMap; j++){
				carteBool[i][j] = false;
		}
	}
	switch (type){
		//L'unit� s�lectionn� est un Nain (case 1) / L'unit� est un Orc (case 3)
	case 1 : case 3 :
		// Cas Nain sur case Montagne
		if (carteElement[xActuel][yActuel] == 1 && ptMouvement == 1 && type == 1){
			for (i = 0; i < tailleMap; i++){
				for (j = 0; j < tailleMap; j++){
					if (carteElement[i][j] == 1){
						if (i == xActuel && j == yActuel){
						}
						else{
							carteBool[i][j] = true;
						}
					}
				}
			}
		}
		
		if (xActuel % 2 == 0){
			//Cas case en haut a gauche
			if ((xActuel - 1) >= 0 && (yActuel - 1) >= 0){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel - 1] = true;
				}
				else if(ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel - 1] == 2){
					carteBool[xActuel - 1][yActuel - 1] = true;
				}
			}
			//Cas case a gauche
			if ((yActuel - 1) >= 0){
				if (ptMouvement == 1){
				carteBool[xActuel][yActuel - 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel - 1] == 2){
					carteBool[xActuel][yActuel - 1] = true;
				}
			}
			//Cas case en bas a gauche
			if ((xActuel + 1) < tailleMap && (yActuel - 1) >= 0){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel - 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel - 1] == 2){
					carteBool[xActuel + 1][yActuel - 1] = true;
				}
			}
			//Cas case en bas a droite
			if ((xActuel + 1) < tailleMap){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel] == 2){
					carteBool[xActuel + 1][yActuel] = true;
				}
			}
			//Cas case a droite
			if ((yActuel + 1) < tailleMap){
				if (ptMouvement == 1){
					carteBool[xActuel][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel + 1] == 2){
					carteBool[xActuel][yActuel + 1] = true;

				}
			}
			//Cas case en haut a droite
			if ((xActuel - 1) >= 0){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel] == 2){
					carteBool[xActuel - 1][yActuel] = true;
				}
			}
		}
		else
		{
			//Cas case en haut a gauche
			if ((xActuel - 1) >= 0){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel] == 2){
					carteBool[xActuel - 1][yActuel] = true;
				}
			}
			//Cas case a gauche
			if ((yActuel - 1) >= 0){
				if (ptMouvement == 1){
					carteBool[xActuel][yActuel - 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel - 1] == 2){
					carteBool[xActuel][yActuel - 1] = true;
				}

			}
			//Cas case en bas a gauche
			if ((xActuel + 1) < tailleMap){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel] == 2){
					carteBool[xActuel + 1][yActuel] = true;
				}
			}
			//Cas case en bas a droite
			if ((xActuel + 1) < tailleMap && (yActuel + 1) < tailleMap){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel + 1] == 2){
					carteBool[xActuel + 1][yActuel + 1] = true;
				}
			}
			//Cas case a droite
			if ((yActuel + 1) < tailleMap){
				if (ptMouvement == 1){
					carteBool[xActuel][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel + 1] == 2){
					carteBool[xActuel][yActuel + 1] = true;
				}
			}
			//Cas case en haut a droite
			if ((xActuel - 1) >= 0 && (yActuel + 1) < tailleMap){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel + 1] == 2){
					carteBool[xActuel - 1][yActuel + 1] = true;
				}
			}
		}
		break;

		//L'unit� s�lectionn� est un Elfe
	case 2 :
		if (xActuel % 2 == 0){
			//Cas case en haut a gauche
			if ((xActuel - 1) >= 0 && (yActuel - 1) >= 0 && carteElement[xActuel - 1][yActuel - 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel - 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel - 1] == 2){
					carteBool[xActuel - 1][yActuel - 1] = true;
				}
			}
			//Cas case a gauche
			if ((yActuel - 1) >= 0 && carteElement[xActuel][yActuel - 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel][yActuel - 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel - 1] == 2){
					carteBool[xActuel][yActuel - 1] = true;
				}
			}
			//Cas case en bas a gauche
			if ((xActuel + 1) < tailleMap && (yActuel - 1) >= 0 && carteElement[xActuel + 1][yActuel - 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel - 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel - 1] == 2){
					carteBool[xActuel + 1][yActuel - 1] = true;
				}
			}
			//Cas case en bas a droite
			if ((xActuel + 1) < tailleMap  && carteElement[xActuel + 1][yActuel] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel] == 2){
					carteBool[xActuel + 1][yActuel] = true;
				}
			}
			//Cas case a droite
			if ((yActuel + 1) < tailleMap  && carteElement[xActuel][yActuel + 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel + 1] == 2){
					carteBool[xActuel][yActuel + 1] = true;
				}
			}
			//Cas case en haut a droite
			if ((xActuel - 1) >= 0 && carteElement[xActuel - 1][yActuel] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel] == 2){
					carteBool[xActuel - 1][yActuel] = true;
				}
			}
		}
		else
		{
			//Cas case en haut a gauche
			if ( (xActuel - 1) >= 0 && carteElement[xActuel - 1][yActuel] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel] == 2){
					carteBool[xActuel - 1][yActuel] = true;
				}
			}
			//Cas case a gauche
			if ((yActuel - 1) >= 0 && carteElement[xActuel][yActuel - 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel][yActuel - 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel - 1] == 2){
					carteBool[xActuel][yActuel - 1] = true;
				}
			}
			//Cas case en bas a gauche
			if ((xActuel + 1) < tailleMap && carteElement[xActuel + 1][yActuel] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel] == 2){
					carteBool[xActuel + 1][yActuel] = true;
				}
			}
			//Cas case en bas a droite
			if ((xActuel + 1) < tailleMap && (yActuel + 1) < tailleMap && carteElement[xActuel + 1][yActuel + 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel + 1][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel + 1][yActuel + 1] == 2){
					carteBool[xActuel + 1][yActuel + 1] = true;
				}
			}
			//Cas case a droite
			if ((yActuel + 1) < tailleMap && carteElement[xActuel][yActuel + 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel][yActuel + 1] == 2){
					carteBool[xActuel][yActuel + 1] = true;
				}
			}
			//Cas case en haut a droite
			if ((xActuel - 1) >= 0 && (yActuel + 1) < tailleMap && carteElement[xActuel - 1][yActuel + 1] != 4){
				if (ptMouvement == 1){
					carteBool[xActuel - 1][yActuel + 1] = true;
				}
				else if (ptMouvement == 0.5 && carteElement[xActuel - 1][yActuel + 1] == 2){
					carteBool[xActuel - 1][yActuel + 1] = true;
				}
			}
		}
		break;

		//L'unit� s�lectionn� est un Pirate (case 4)
		case 4:
			if (xActuel % 2 == 0){
				//Cas case en haut a gauche
				if ((xActuel - 1) >= 0 && (yActuel - 1) >= 0){
					if (ptMouvement == 1){
						carteBool[xActuel - 1][yActuel - 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel - 1][yActuel - 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel - 1][yActuel - 1] = true;
					}
				}
				//Cas case a gauche
				if ((yActuel - 1) >= 0){
					if (ptMouvement == 1){
						carteBool[xActuel][yActuel - 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel][yActuel - 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel][yActuel - 1] = true;
					}
				}
				//Cas case en bas a gauche
				if ((xActuel + 1) < tailleMap && (yActuel - 1) >= 0){
					if (ptMouvement == 1){
						carteBool[xActuel + 1][yActuel - 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel + 1][yActuel - 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel + 1][yActuel - 1] = true;
					}
				}
				//Cas case en bas a droite
				if ((xActuel + 1) < tailleMap){
					if (ptMouvement == 1){
						carteBool[xActuel + 1][yActuel] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel + 1][yActuel] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel + 1][yActuel] = true;
					}
				}
				//Cas case a droite
				if ((yActuel + 1) < tailleMap ){
					if (ptMouvement == 1){
						carteBool[xActuel][yActuel + 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel][yActuel + 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel][yActuel + 1] = true;
					}
				}
				//Cas case en haut a droite
				if ((xActuel - 1) >= 0){
					if (ptMouvement == 1){
						carteBool[xActuel - 1][yActuel] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel - 1][yActuel] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel - 1][yActuel] = true;
					}
				}
			}
			else
			{
				//Cas case en haut a gauche
				if ((xActuel - 1) >= 0){
					if (ptMouvement == 1){
						carteBool[xActuel - 1][yActuel] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel - 1][yActuel] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel - 1][yActuel] = true;
					}
				}
				//Cas case a gauche
				if ((yActuel - 1) >= 0){
					if (ptMouvement == 1){
						carteBool[xActuel][yActuel - 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel][yActuel - 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel][yActuel - 1] = true;
					}
				}
				//Cas case en bas a gauche
				if ((xActuel + 1) < tailleMap){
					if (ptMouvement == 1){
						carteBool[xActuel + 1][yActuel] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel + 1][yActuel] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel + 1][yActuel] = true;
					}
				}
				//Cas case en bas a droite
				if ((xActuel + 1) < tailleMap && (yActuel + 1) < tailleMap ){
					if (ptMouvement == 1){
						carteBool[xActuel + 1][yActuel + 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel + 1][yActuel + 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel + 1][yActuel + 1] = true;
					}
				}
				//Cas case a droite
				if ((yActuel + 1) < tailleMap){
					if (ptMouvement == 1){
						carteBool[xActuel][yActuel + 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel][yActuel + 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel][yActuel + 1] = true;
					}
				}
				//Cas case en haut a droite
				if ((xActuel - 1) >= 0 && (yActuel + 1) < tailleMap){
					if (ptMouvement == 1){
						carteBool[xActuel - 1][yActuel + 1] = true;
					}
					else if (ptMouvement == 0 && carteElement[xActuel - 1][yActuel + 1] == 5 && carteElement[xActuel][yActuel] == 5){
						carteBool[xActuel - 1][yActuel + 1] = true;
					}
				}
			}
			break;
	default: break;
	}

	return carteBool;
}