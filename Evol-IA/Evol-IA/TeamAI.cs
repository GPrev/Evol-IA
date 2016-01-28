using PokeRules;
using System;
using System.Collections.Generic;


namespace Evol_IA
{
    public class TeamAI
    {
        //trainersnb = number of trainers, pools nb = number of pools, iterations nb = number of time we repeat the algorithm
        private int trainersnb, poolsnb, iterationsnb;
        private int pokemonnbteam;      //number of pokemons in a team
        private List<PokeData>[] teams; //this table contains all the teams which will be used
        private int[] scores;           //score[i]=score of the team teams[i]
        private PokeData[] pokemons;    //contains pokedata of all pokemons
        private Random r;               //better to have one random for all the algorithm than one per function

        public TeamAI(int number_trainers, int number_pokemon_team, List<PokeData> list_all_pokedata, int pools_number = 5, int iteration_number=5)
        {
            r = new Random();
            pokemonnbteam = number_pokemon_team;
            poolsnb = pools_number;
            iterationsnb = iteration_number;
            while ((number_trainers % poolsnb) != 0)  //to be sure every trainer will be in a pool
            {
                number_trainers=number_trainers+1; 
            }
            trainersnb = number_trainers;

            teams = new List<PokeData>[trainersnb];
            pokemons = new PokeData[list_all_pokedata.Count];
            scores = new int[trainersnb];
            initializePokemons(list_all_pokedata);  //initialize the pokedata table with the given pokedata list
            initializeTrainersTeams();              //initialize the teams table with random teams
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
            for (int i = 0; i < trainersnb; i++) {
                teams[i] = randomTeam();
            }
        }


        public List<PokeData> randomTeam()
        {
            List<PokeData> poke = new List<PokeData>();

            for (int i = 0; i < pokemonnbteam; i++)
            {
                int rand = r.Next(pokemons.Length);
                PokeData p = pokemons[rand];
                while (poke.Contains(p))  //if the pokemon is already in the team, we choose another one
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
            List<PokeData> bestTeam = new List<PokeData>();

            for (int i = 0; i < iterationsnb; i++) {    //the algorithm will be done interationsnb times
                int pooltrainersnb = trainersnb / poolsnb;
                //pooltrainersnb = number of trainer in each pool = number of trainers / numer of pools

                int[,] pools = randomPool(pooltrainersnb);
                //The first column contains the number of each pool 
                //The others columns contains pooltrainersnb numbers, which are the indexs of the trainers who will fight in this pool

                fightsSimulating(pooltrainersnb, pools); 
                //We do all the fights and fill the scores tab with the results

                teamsScoresSort();
                //the teams and scores tables are now sorted by the scores of each team, the worest team is at index 0

                doAllRandomChildren();
                //the first part of the table is now fill with children of the best teams

                reinitializeScores();
                //set all scores at 0
            }

            //once it has done enough iterations, it return the best team
            bestTeam = teams[trainersnb - 1];

            return bestTeam;
        }
        

        public void fightsSimulating(int tnb, int[,] pools)
        {
            //tnb = number of trainers per pool

            for (int i = 0; i < poolsnb; i++)  //for each pool
            {
                for (int j = 0; j < tnb; j++) //for each team in the pool
                {
                    for (int k = j + 1; k < tnb; k++) //each teams fights every other in the same pool
                    {
                        List<Pokemon> p1 = new List<Pokemon>();
                        List<Pokemon> p2 = new List<Pokemon>();

                        //we do pokemons team with the pokedata sotred in teams
                        foreach (PokeData p in teams[pools[i, j]])
                        {
                            p1.Add(new Pokemon(p));
                        }

                        foreach (PokeData p in teams[pools[i, k]])
                        {
                            p2.Add(new Pokemon(p));
                        }
                        Trainer t1 = new Trainer("1", p1);
                        Trainer t2 = new Trainer("2", p2);

                        List<Trainer> trainers = new List<Trainer>();
                        trainers.Add(t1);
                        trainers.Add(t2);

                        //we make them fight with minmax AI
                        List<Intelligence> ais = new List<Intelligence>();

                        ais.Add(new MinMaxAI(trainers[0],2)); // AI 1 (2 = nb d'itérations ; 0 = myId)
                        ais.Add(new MinMaxAI(trainers[1],2)); // AI 2

                        Battle battle = new Battle(ais);

                        Trainer w = trainers[battle.PlayBattle()]; //simulate the fight

                        if (t1.Equals(w)) { scores[pools[i, j]]++; scores[pools[i, k]]--; } //the winner earns 1 points, the loser loses 1
                        else { scores[pools[i, j]]--; scores[pools[i, k]]++; }
                    }
                }
            }
        }


        public List<PokeData> doRandomChild(int t1, int t2) //t1 and t2 are the index of the two parents in the team table
        {
            List <PokeData> team = new List<PokeData>();
            List<PokeData> teamp1 = teams[t1];
            List<PokeData> teamp2 = teams[t2];

            PokeData[] teamdes2parents = new PokeData[teamp1.Count+teamp2.Count]; //this table will contains the pokemons of the two parents
            int i = 0;

            foreach (PokeData p in teamp1) { teamdes2parents[i] = p; i++; }

            foreach (PokeData p in teamp2) { teamdes2parents[i] = p; i++; }
            //we had the parents' pokemons in the table

            for (int j = 0; j+1 < pokemonnbteam; j=j+2)
            {
                int r1 = r.Next(0, pokemonnbteam);                  //gives a random index of one of the first parent's pokemon
                int r2 = r.Next(pokemonnbteam, pokemonnbteam * 2);  //gives a random index of one of the second parent's pokemon

                while (team.Contains(teamdes2parents[r1])) //if the pokemon is already in the child's team, we choose another one
                {
                    r1 = r.Next(0, pokemonnbteam);
                }

                team.Add(teamdes2parents[r1]);

                while (team.Contains(teamdes2parents[r2]))
                {
                    r2 = r.Next(pokemonnbteam, pokemonnbteam * 2);
                }

                team.Add(teamdes2parents[r2]);
            }

            if(pokemonnbteam % 2 == 1) //if the number of pokemons per team is an odd number, we take a last random pokemon from one of the parents
            {
                int rand = r.Next(teamdes2parents.Length);
                while (team.Contains(teamdes2parents[rand]))
                {   
                    rand = r.Next(teamdes2parents.Length);
                }

                team.Add(teamdes2parents[rand]);
            }

            //5% chance of mutation which gives a random pokemon which does not from the parents
            int mut = r.Next(100);
            if (mut < 5)
            {
                int rm = r.Next(pokemons.Length);
                int rp = r.Next(pokemons.Length);

                while (!team.Contains(pokemons[rm]))
                {
                    rm = r.Next(pokemons.Length);
                }


                while (team.Contains(pokemons[rp]))
                {
                    rp = r.Next(pokemons.Length);
                }
                //rp is the index in the pokemons table of the pokemons it will add
                team.Remove(pokemons[rm]);
                team.Add(pokemons[rp]);
            }

            return team;
        }

        public void doAllRandomChildren()
        {

            for (int i = 0; i < trainersnb / 2; i++)
            {
                int r1 = r.Next(trainersnb / 2, trainersnb); //we take 2 random trainers in the better half
                int r2 = r.Next(trainersnb / 2, trainersnb);
                teams[i] = doRandomChild(r1, r2);
            }

        }

        public void reinitializeScores()
        {
            for(int i = 0; i < trainersnb; i++)
            {
                scores[i] = 0;
            }
        }

        public int[,] randomPool(int tnb)
        {
            //tnb = number of trainers in each pool
            int[,] pools = new int[poolsnb, tnb];
            int[] randomtab = randomIntTab(); //table fill in with numbers between 0 and trainersnb (each number appears once)

            for (int i = 0; i < poolsnb; i++) //i = index of the pool
            {
                for (int j = 0; j < tnb; j++) //j = index of trainer in the pool
                {
                    pools[i, j] = randomtab[i * tnb + j]; 
                }
            }
            return pools;
        }

        public int[] randomIntTab()
        {
            //return a table with all numbers from 0 to trainersnb-1 which had trainersnb random permutations to make it random
            int[] randomtable = new int[trainersnb];

            for (int i = 0; i < trainersnb; i++)
            {
                randomtable[i] = i;
            }

            for (int i = 0; i < trainersnb; i++)
            {
                int a = r.Next(trainersnb);
                int b = r.Next(trainersnb);
                int tmp = randomtable[a];
                randomtable[a] = randomtable[b];
                randomtable[b] = tmp;
            }
            return randomtable;
        }

        public void teamsScoresSort()
        {
            //Optimazed bubble sort algorithm
            int n = trainersnb;
            bool echange = true;
            while ((n > 0) && echange)
            {
                echange = false;
                for (int j = 0; j < n-1; j++)
                {
                    echange = false;
                    if (scores[j] > scores[j + 1])
                    {
                        List<PokeData> tmpt = teams[j];
                        int tmps = scores[j];
                        teams[j] = teams[j + 1];
                        scores[j] = scores[j + 1];
                        teams[j + 1] = tmpt;
                        scores[j + 1] = tmps;
                        echange = true;
                    }
                }
                n--;
            }
        }


        //displays in the console all the teams contained in the teams table
        //currently not used but was used for testing and debbuging
        public void displayTrainers()
        {
            for(int i = 0; i < trainersnb; i++)
            {
                String t = "";
                foreach(PokeData p in teams[i])
                {
                    t += " " + p.Name;
                }
                Console.WriteLine(i + " " + t);
            }
        }

    }
}