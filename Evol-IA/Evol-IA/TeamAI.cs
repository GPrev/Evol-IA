﻿using PokeRules;
using System;
using System.Collections.Generic;

/*
    GROS PROBLEME : LE FAIT QU'IL Y AIE PLUSIEURS BATAILLES AVEC LES MEMES POKEMONS
    FAIRE UN TABLEAU DE POKEDATA A LA PLACE DU TABLEAU DE TRAINERS ?
    EN VRAI CA DOIT ETRE VIABLE JE PENSE
    JE SUIS MAUVAISEEEEEE
    MAIS OUI POKEDATA A LA PLACE DE TRAINERS CA DOIT ETRE VIABLEEEEEEEEEEEEEEEEEEE
    BORDEEEEEEEEEEEEEEEEEEEL
    PUTAIN DE BORDEL DE MERDEEEE
*/

namespace Evol_IA
{
    public class TeamAI
    {
        //nb dresseurs : nb de dresseurs qui vont s'entretuer ; nbpools = nombre de pools qu'on prend ; nbiterations = nb de fois où on va boucler
        private int nbdresseurs, nbpools, nbiterations;
        private int nbpokemonequipe;    //nombre de pokemons par equpe
        private List<PokeData>[] equipes;
        //private Trainer[] dresseurs;    //on les stocke dans un tableau pour avoir accès 
        private int[] scores;           //score[i]=nombre de victoires de dresseurs[i] pour le combat en cours (remis à 0 après les simulations)
        private PokeData[] pokemons;     //tableau de tous les pokémons possibles
        private Random r;

        public TeamAI(int n, int p, List<PokeData> pokeDatalist, int pool = 5, int nbit=5)
        {
            r = new Random();
            nbdresseurs = n;
            nbpokemonequipe = p;
            nbpools = pool;
            nbiterations = nbit;
            if ((n % nbpools) != 0)  //valable surtout si on rentre le chiffre. Si n est pas divisble par le nombre c'est nul...
            {
                nbpools = 2; //valeur arbitraire. On verra après pour changer. J'ai eu la flemme de m'en charger je l'avoue.
                //Du coup faudra toujours mettre un nombre pair de dresseurs pour le moment
            }
            equipes = new List<PokeData>[nbdresseurs];
            pokemons = new PokeData[pokeDatalist.Count];
            scores = new int[nbdresseurs];
            initializePokemons(pokeDatalist); //ça stocke dans le tableau pokemons la liste de pokedata
            initializeTrainersTeams();        //ça fout des équipes random à tous les dresseurs
        }


        public void initializePokemons(List<PokeData> pokelist)
        {
            int i = 0;
            foreach (PokeData pokedata in pokelist)
            {
                pokemons[i] = pokedata;
                i++;
            }
        }


        public void initializeTrainersTeams()
        {
            for (int i = 0; i < nbdresseurs; i++) {
                equipes[i] = randomTeam();
            }
        }


        public List<PokeData> randomTeam()
        {
            List<PokeData> poke = new List<PokeData>();

            for (int i = 0; i < nbpokemonequipe; i++)
            {
                int rand = r.Next(pokemons.Length);
                PokeData p = pokemons[rand];
                while (poke.Contains(p))                //on vérifie si le pokémon est déjà dans la liste. Si c'est le cas, on en prend un autre
                {
                    rand = r.Next(pokemons.Length);
                    p = pokemons[rand];
                }
              poke.Add(p);
            }
            return poke;
        }

        public List<PokeData> selectTeamAI()
        {
            /*La c'est marrant
            Les console.Writeline sont là pour les tests, je les laisse pour le moment */
            Console.WriteLine("Before everything ");
            List<PokeData> bestTeam = new List<PokeData>();
            //Pour créer des nombres random. Apparemment c'est mieux d'en créer un pour toute la fonction que d'en recréer à chaque fois que tu veux un nouveau nombre

            for (int i = 0; i < nbiterations; i++) {
                dresseurstoString();
                int nbdresseurspool = nbdresseurs / nbpools;
                //dans le tableau pool, pour le numéro de chaque pool, on prend les nbdresseurspool premiers dresseurs du tableau d'aléatoire (puis les suivants)
                //cela correspond à l'indice des dresseurs qui vont jouer dans chaque pool

                int[,] pools = poolAleat(nbdresseurspool);
                //pools : un double tableau rempli avec le numéro de la pool 
                //et des nombres aléatoires qui vont représenter les indices des dresseurs qui vont combattre dans la pool
                //Console.WriteLine(i+" Pools made");

                simulationBatailles(nbdresseurspool, pools); 
                //on fait les combats et on rempli à chaque fois le tableau score de chaque dresseur (en théorie)
                // CA MARCHE PAS
                //Console.WriteLine("Simulation faite");

                trierDresseursScore();
                //on trie le tableau dresseur dans l'ordre croissant des scores. Les Nbdresseurs / 2 derniers seront les parents sélectionnés
                //Console.WriteLine("Tri fait");

                faireTousLesEnfantsAleat();
                //on remplit le tableau dresseur en remplacaçant les n/2 moins bons dresseurs par des enfants faits totalement au pif
                // on prend 2 bons dresseurs au pif, età chaque fois, on prend la moitié des pokémons de l'un et de l'autre au pif
                //Console.WriteLine("Enfant faits");

                reinitialiserScores();
                //on remet les scores à 0 avant la simulation suivante
            }

            //quand on a finit de boucler, on prend le dernier dresseur (le plus fort après le tri)
            bestTeam = equipes[nbdresseurs - 1];

            return bestTeam;
        }
        

        public void simulationBatailles(int nbd, int[,] pools)
        {
            for (int i = 0; i < nbpools; i++)  //pour chaque pool
            {
                for (int j = 0; j < nbd; j++) //pour chaque dresseur dans la pool
                {
                    for (int k = j + 1; k < nbd; k++) //il combat ceux qu'il a pas encore combattu
                    {
                        List<Pokemon> p1 = new List<Pokemon>();
                        List<Pokemon> p2 = new List<Pokemon>();

                        foreach (PokeData p in equipes[pools[i, j]])
                        {
                            p1.Add(new Pokemon(p));
                        }

                        foreach (PokeData p in equipes[pools[i, k]])
                        {
                            p2.Add(new Pokemon(p));
                        }
                        Trainer t1 = new Trainer("1", p1);
                        Trainer t2 = new Trainer("2", p2);

                        List<Trainer> trainers = new List<Trainer>();
                        trainers.Add(t1);
                        trainers.Add(t2);

                        List<Intelligence> ais = new List<Intelligence>();
                        ais.Add(new MinMaxAI(trainers[0],2,0)); // AI 1 (2 = nb d'itérations ; 0 = myId)
                        ais.Add(new MinMaxAI(trainers[1],2,1)); // AI 2
                        Console.WriteLine("Avant la bataille"+i+j+k);

                        Battle battle = new Battle(ais);
                        Console.WriteLine("Pendant la bataille ?" + i + j + k);

                        Trainer w = trainers[battle.PlayBattle()]; //il boucle pendant un temps insupportable
                        Console.WriteLine("Après la bataille"+i+j+k);

                        if (t1.Equals(w)) { scores[pools[i, j]]++; scores[pools[i, k]]--; } //on rajoute un point au vainqueur, on enlève 1 point au perdant
                        else { scores[pools[i, j]]--; scores[pools[i, k]]++; }
                    }
                }
            }
        }


        public List<PokeData> faireEnfantAleat(int t1, int t2) //t1 et t2 les indices des 2 parents
        {
            List <PokeData> team = new List<PokeData>();
            List<PokeData> teamp1 = equipes[t1];
            List<PokeData> teamp2 = equipes[t2];
            //Console.WriteLine("Faire 1 enfant");
            PokeData[] teamdes2parents = new PokeData[nbpokemonequipe*2]; //tableau qui va contenir les pokémons des deux parents
            int i = 0;
            //Console.WriteLine("Début remplissage 1er parent");

            foreach (PokeData p in teamp1) { teamdes2parents[i] = p; i++; }
            //Console.WriteLine("Fin remplissage 1er parent, début 2eme");
            foreach (PokeData p in teamp2) { teamdes2parents[i] = p; i++; }
            //Console.WriteLine("       "+i);

            //Console.WriteLine("Fin 2eme parent");


            for (int j = 0; j+1 < nbpokemonequipe; j=j+2)
            {
                Console.WriteLine("Debut boucle faire 1 enfant"); 
                //de temps en temps il part en boucle infinie. Par plaisir.
                //Check la min max ia qui a eu un léger doute. Ou pas en fait. Je sais pas trop.

                int r1 = r.Next(0, nbpokemonequipe); //l'indice d'un des pokémons de la team du premier parent
                int r2 = r.Next(nbpokemonequipe, nbpokemonequipe * 2); //l'indice d'un des pokémons de la team du deuxième parent
                Console.WriteLine("Avant les while");

                while (team.Contains(teamdes2parents[r1])) //si le pokémon est déjà présent, on en prend un autre
                {
                    r1 = r.Next(0, nbpokemonequipe);
                }
                Console.WriteLine("Entre les while");
                while (team.Contains(teamdes2parents[r2]))
                {
                    r2 = r.Next(nbpokemonequipe, nbpokemonequipe * 2);
                }
                Console.WriteLine("Après les while");

                team.Add(teamdes2parents[r1]);
                team.Add(teamdes2parents[r2]);
            }

            if(nbpokemonequipe % 2 == 1) //si le nombre de pokémons par équipe est impair, on en rajoute un totalement au pif
            {
                Console.WriteLine("Dans le début du if");

                //Console.WriteLine("If faire 1 enfant avec nombre impair de pokemon");
                int rand = r.Next(teamdes2parents.Length);
                while (team.Contains(teamdes2parents[rand])){   //il est des fois parti en boucle infinie sans raison, et des fois non. J'ai pas l'impression qu'il y aie de problèmes dans le code
                    //en vrai j'ai plus l'impression que le problème de boucle infiie était lié à MVS qu'au code...
                    //Console.WriteLine("-----------------------------"+rand+"------------------------------");
                    rand = r.Next(teamdes2parents.Length);
                    //Console.WriteLine("_____________________________"+rand+"______________________________");

                }
                team.Add(teamdes2parents[rand]);
                Console.WriteLine("Vers la fin du if, encore dedans");

            }
            //Console.WriteLine("Fin Faire 1 enfant");

            String t = "";
            foreach (PokeData p in team)
            {
                t += " " + p.Name;
            }
            Console.WriteLine("----Enfant :  " + t);
        

            return team;
        }

        public void faireTousLesEnfantsAleat()
        {
            Console.WriteLine("Fonction faire tous les enfants");

            for (int i = 0; i < nbdresseurs / 2; i++)
            {
                int r1 = r.Next(nbdresseurs / 2, nbdresseurs); //on prend deux parents au pif dans la deuxième moitié de dresseurs (ceux avec un bon score)
                int r2 = r.Next(nbdresseurs / 2, nbdresseurs);
                equipes[i] = faireEnfantAleat(r1, r2);
            }
            Console.WriteLine("Enfants faits");

        }

        public void reinitialiserScores()
        {
            for(int i = 0; i < nbdresseurs; i++)
            {
                scores[i] = 0;
            }
        }

        public int[,] poolAleat(int nbd)
        {
            //nbd = nombre de dresseurs par pool
            int[,] pools = new int[nbpools, nbd];
            int[] aleat = aleatIntTab(); //un tableau rempli d'aléatoires entre 0 et nb dresseurs

            for (int i = 0; i < nbpools; i++) //i numéro de la pool
            {
                for (int j = 0; j < nbd; j++) //j numéro du dresseur dans sa pool
                {
                    pools[i, j] = aleat[i * nbd + j]; 
                }
            }
            return pools;
        }

        public int[] aleatIntTab()
        {
            //renvoie un tableau contenant des chiffres de 0 à nbdresseurs - 1 ayant subit n permutations pour le rendre le plus aléatoire possible
            int[] aleat = new int[nbdresseurs];

            for (int i = 0; i < nbdresseurs; i++)
            {
                aleat[i] = i;
            }

            for (int i = 0; i < nbdresseurs; i++)
            {
                int a = r.Next(nbdresseurs);
                int b = r.Next(nbdresseurs);
                int tmp = aleat[a];
                aleat[a] = aleat[b];
                aleat[b] = tmp;
            }
            return aleat;
        }

        public void trierDresseursScore()
        {
            //Console.WriteLine("Je suis au début de la fonction qui bug");
            /* on trie le table des scores dans l'ordre croissant pour prendre les n-1 dernier*/
            /* c'est un algorithme de tri à bulles opti chopé sur internet*/
            int n = nbdresseurs;
            bool echange = true;
            while ((n > 0) && echange)
            {
                //Console.WriteLine("Je suis dans la première boucle");
                echange = false;
                for (int j = 0; j < n-1; j++)
                {
                    echange = false;
                    //Console.WriteLine("Je suis dans le for");
                    if (scores[j] > scores[j + 1])
                    {
                        //Console.WriteLine("Je suis dans le if");
                        List<PokeData> tmpt = equipes[j];
                        int tmps = scores[j];
                        equipes[j] = equipes[j + 1];
                        scores[j] = scores[j + 1];
                        equipes[j + 1] = tmpt;
                        scores[j + 1] = tmps;
                        echange = true;
                        //Console.WriteLine("Je suis à la fin du if");
                    }
                    //Console.WriteLine("Je ne suis plus dans le if");
                }
                //Console.WriteLine("Je ne suis plus dans le for");
                n--;

            }
            //Console.WriteLine("Je suis à la fin de la fonction");
        }


        public void dresseurstoString()
        {
            for(int i = 0; i < nbdresseurs; i++)
            {
                String t = "";
                foreach(PokeData p in equipes[i])
                {
                    t += " " + p.Name;
                }
                Console.WriteLine(i + " " + t);
            }
        }

    }
}