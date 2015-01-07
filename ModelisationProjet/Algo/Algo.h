#ifdef WANTDLLEXP
#define DLL _declspec(dllexport)
#define EXTERNC extern "C"
#else
#define DLL
#define EXTERNC
#endif
#include <vector>
using std::vector;

class DLL Algo{
public:
	/*! \brief Algorithme de g�n�ration de la carte
	* \param taille de la carte
	* \return tableau a deux dimensions d'entiers. Chaque entier correspond � un type de case (Desert, Eau, Foret, Plaine ou Montagne) */
	DLL static int** generationMap(int taille);

	/*! \brief Algorithme du placement des joueurs en d�but de jeu
	* \param taille de la carte
	* \return tableau d'entier � une dimension contenant les coordonn�es de d�part de chaque joueur */
	DLL static int* placementJoueur(int taille);

	/*! \brief Algorithme de suggestion des cases o� l'on peut se d�placer
	* \param taille de la carte
	* \param xActuel coordonn�e des abscisses correspondant � la position o� se trouve le personnage
	* \param yActuel coordonn�e des ordonn�es correspondant � la position o� se trouve le personnage
	* \param type correspond au type du personnage selectionn� (Elfe, Pirate, Nain ou Orc)
	* \param carteElement est un tableau d'entiers � double dimension repr�sentant la carte. Chaque entier correspond � un type de case (Desert, Eau, Foret, Plaine ou Montagne)
	* \param ptMouvement correspond au nombre de points de mouvement que le personnage selectionn� poss�de
	* \return un tableau de bool�en � deux dimensions de la taille de la carte indiquant les cases o� le personnage peut se d�placer. Vrai le personnage peut se d�placer. Faux sinon. */
	DLL static bool** suggestionCase(int taille, int xActuel, int yActuel, int type, int** carteElement, double ptMouvement);
}; 

