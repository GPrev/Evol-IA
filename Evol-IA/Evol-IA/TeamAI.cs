using PokeRules;
using System;
using System.Collections.Generic;


namespace Evol_IA
{
    public class TeamAI
    {
        //nb dresseurs : nb de dresseurs qui vont s'entretuer ; nbpools = nombre de pools qu'on prend ; nbiterations = nb de fois où on va boucler
        private int nbdresseurs, nbpools, nbiterations;
        private int nbpokemonequipe;    //nombre de pokemons par equpe
        private Trainer[] dresseurs;    //on les stocke dans un tableau pour avoir accès 
        private int[] scores;           //score[i]=nombre de victoires de dresseurs[i] pour le combat en cours (remis à 0 après les simulations)
        private PokeData[] pokemons;     //tableau de tous les pokémons possibles

        public TeamAI(int n, int p, List<PokeData> pokeDatalist, int pool = 5, int nbit=5)
        {
            nbdresseurs = n;
            nbpokemonequipe = p;
            nbpools = pool;
            nbiterations = nbit;
            if ((n % nbpools) != 0)  //valable surtout si on rentre le chiffre. Si n est pas divisble par le nombre c'est nul...
            {
                nbpools = 2; //valeur arbitraire. On verra après pour changer. J'ai eu la flemme de m'en charger je l'avoue.
                //Du coup faudra toujours mettre un nombre pair de dresseurs pour le moment
            }
            dresseurs = new Trainer[nbdresseurs];
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
                dresseurs[i] = new Trainer("D" + i, randomTeam());
            }
        }


        public List<Pokemon> randomTeam()
        {
            Random r = new Random();
            List<Pokemon> poke = new List<Pokemon>();

            for (int i = 0; i < nbpokemonequipe; i++)
            {
                int rand = r.Next(pokemons.Length);
                Pokemon p = new Pokemon(pokemons[rand]);
                while (poke.Contains(p))                //on vérifie si le pokémon est déjà dans la liste. Si c'est le cas, on en prend un autre
                {
                    rand = r.Next(pokemons.Length);
                    p = new Pokemon(pokemons[rand]);
                }
              poke.Add(p);
            }
            return poke;
        }

        public List<Pokemon> selectTeamAI()
        {
            /*La c'est marrant
            Les console.Writeline sont là pour les tests, je les laisse pour le moment */
            Console.WriteLine("Before everything ");
            List<Pokemon> bestTeam = new List<Pokemon>();
            Random r = new Random();
            //Pour créer des nombres random. Apparemment c'est mieux d'en créer un pour toute la fonction que d'en recréer à chaque fois que tu veux un nouveau nombre

            for (int i = 0; i < nbiterations; i++) { 

                int nbdresseurspool = nbdresseurs / nbpools;
                //dans le tableau pool, pour le numéro de chaque pool, on prend les nbdresseurspool premiers dresseurs du tableau d'aléatoire (puis les suivants)
                //cela correspond à l'indice des dresseurs qui vont jouer dans chaque pool

                int[,] pools = poolAleat(nbdresseurspool, r);
                //pools : un double tableau rempli avec le numéro de la pool 
                //et des nombres aléatoires qui vont représenter les indices des dresseurs qui vont combattre dans la pool
                Console.WriteLine(i+" Pools made");

                simulationBatailles(nbdresseurspool, pools); 
                //on fait les combats et on rempli à chaque fois le tableau score de chaque dresseur (en théorie)
                // CA MARCHE PAS
                Console.WriteLine("Simulation faite");

                trierDresseursScore();
                //on trie le tableau dresseur dans l'ordre croissant des scores. Les Nbdresseurs / 2 derniers seront les parents sélectionnés
                Console.WriteLine("Tri fait");

                faireTousLesEnfantsAleat(r);
                //on remplit le tableau dresseur en remplacaçant les n/2 moins bons dresseurs par des enfants faits totalement au pif
                // on prend 2 bons dresseurs au pif, età chaque fois, on prend la moitié des pokémons de l'un et de l'autre au pif
                Console.WriteLine("Enfant faits");

                reinitialiserScores();
                //on remet les scores à 0 avant la simulation suivante
            }

            //quand on a finit de boucler, on prend le dernier dresseur (le plus fort après le tri)
            bestTeam = dresseurs[nbdresseurs - 1].Team;
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

                        List<Trainer> trainers = new List<Trainer>();
                        trainers.Add(dresseurs[pools[i,j]]);
                        trainers.Add(dresseurs[pools[i,k]]);

                        List<Intelligence> ais = new List<Intelligence>();
                        ais.Add(new MinMaxAI(trainers[0],2,0)); // AI 1 (2 = nb d'itérations ; 0 = myId)
                        ais.Add(new MinMaxAI(trainers[1],2,1)); // AI 2
                        Console.WriteLine("Avant la bataille"+i+j+k);

                        Battle battle = new Battle(ais);
                        Console.WriteLine("Pendant la bataille ?" + i + j + k);

                        Trainer w = battle.PlayBattle(); //il boucle pendant un temps insupportable
                        Console.WriteLine("Après la bataille"+i+j+k);

                        if (dresseurs[pools[i, j]].Equals(w)) { scores[pools[i, j]]++; scores[pools[i, k]]--; } //on rajoute un point au vainqueur, on enlève 1 point au perdant
                        else { scores[pools[i, j]]--; scores[pools[i, k]]++; }
                    }
                }
            }
        }


        public Trainer faireEnfantAleat(int t1, int t2, Random r) //t1 et t2 les indices des 2 parents
        {
            List <Pokemon> team = new List<Pokemon>();
            List<Pokemon> teamp1 = dresseurs[t1].Team;
            List<Pokemon> teamp2 = dresseurs[t2].Team;

            Pokemon[] teamdes2parents = new Pokemon[nbpokemonequipe*2]; //tableau qui va contenir les pokémons des deux parents
            int i = 0;
            foreach (Pokemon p in teamp1) { teamdes2parents[i] = p; i++; }

            foreach (Pokemon p in teamp2) { teamdes2parents[i] = p; i++; }

            for(int j = 0; j < nbpokemonequipe; j=j+2)
            {
                int r1 = r.Next(0, nbpokemonequipe); //l'indice d'un des pokémons de la team du premier parent
                int r2 = r.Next(nbpokemonequipe, nbpokemonequipe * 2); //l'indice d'un des pokémons de la team du deuxième parent
                while (team.Contains(teamdes2parents[r1])) //si le pokémon est déjà présent, on en prend un autre
                {
                    r1 = r.Next(0, nbpokemonequipe);
                }
                while (team.Contains(teamdes2parents[r2]))
                {
                    r2 = r.Next(0, nbpokemonequipe);
                }
                team.Add(teamdes2parents[r1]);
                team.Add(teamdes2parents[r2]);
            }

            if(nbpokemonequipe % 2 == 1) //si le nombre de pokémons par équipe est impair, on en rajoute un totalement au pif
            {
                int rand = r.Next(nbpokemonequipe);
                team.Add(teamdes2parents[rand]);
            }
            Trainer t = new Trainer("D" + t1 + t2, team);
            return t;
        }

        public void faireTousLesEnfantsAleat(Random r)
        {
            for(int i = 0; i < nbdresseurs / 2; i++)
            {
                int r1 = r.Next(nbdresseurs / 2, nbdresseurs); //on prend deux parents au pif dans la deuxième moitié de dresseurs (ceux avec un bon score)
                int r2 = r.Next(nbdresseurs / 2, nbdresseurs);
                dresseurs[i] = faireEnfantAleat(r1, r2,r);
            }
        }

        public void reinitialiserScores()
        {
            for(int i = 0; i < nbdresseurs; i++)
            {
                scores[i] = 0;
            }
        }

        public int[,] poolAleat(int nbd,Random r)
        {
            //nbd = nombre de dresseurs par pool
            int[,] pools = new int[nbpools, nbd];
            int[] aleat = aleatIntTab(r); //un tableau rempli d'aléatoires entre 0 et nb dresseurs

            for (int i = 0; i < nbpools; i++) //i numéro de la pool
            {
                for (int j = 0; j < nbd; j++) //j numéro du dresseur dans sa pool
                {
                    pools[i, j] = aleat[i * nbd + j]; 
                }
            }
            return pools;
        }

        public int[] aleatIntTab(Random r)
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
            /* on trie le table des scores dans l'ordre croissant pour prendre les n-1 dernier*/
            /* c'est un algorithme de tri à bulles opti chopé sur internet*/
            int n = nbdresseurs;
            bool echange = true;
            while ((n > 0) && echange)
            {
                echange = false;
                for (int j = 0; j < n; j++)
                {
                    if (scores[j] > scores[j + 1])
                    {
                        Trainer tmpt = dresseurs[j];
                        int tmps = scores[j];
                        dresseurs[j] = dresseurs[j + 1];
                        scores[j] = scores[j + 1];
                        dresseurs[j + 1] = tmpt;
                        scores[j + 1] = tmps;
                        echange = true;
                    }
                }
                n--;
            }
        }




    }
}