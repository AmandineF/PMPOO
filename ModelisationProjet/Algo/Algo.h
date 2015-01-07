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
	/*! \brief Algorithme de génération de la carte
	* \param taille de la carte
	* \return tableau a deux dimensions d'entiers. Chaque entier correspond à un type de case (Desert, Eau, Foret, Plaine ou Montagne) */
	DLL static int** generationMap(int taille);

	/*! \brief Algorithme du placement des joueurs en début de jeu
	* \param taille de la carte
	* \return tableau d'entier à une dimension contenant les coordonnées de départ de chaque joueur */
	DLL static int* placementJoueur(int taille);

	/*! \brief Algorithme de suggestion des cases où l'on peut se déplacer
	* \param taille de la carte
	* \param xActuel coordonnée des abscisses correspondant à la position où se trouve le personnage
	* \param yActuel coordonnée des ordonnées correspondant à la position où se trouve le personnage
	* \param type correspond au type du personnage selectionné (Elfe, Pirate, Nain ou Orc)
	* \param carteElement est un tableau d'entiers à double dimension représentant la carte. Chaque entier correspond à un type de case (Desert, Eau, Foret, Plaine ou Montagne)
	* \param ptMouvement correspond au nombre de points de mouvement que le personnage selectionné possède
	* \return un tableau de booléen à deux dimensions de la taille de la carte indiquant les cases où le personnage peut se déplacer. Vrai le personnage peut se déplacer. Faux sinon. */
	DLL static bool** suggestionCase(int taille, int xActuel, int yActuel, int type, int** carteElement, double ptMouvement);
}; 

