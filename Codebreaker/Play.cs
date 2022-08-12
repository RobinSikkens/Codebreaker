using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreaker
{
    public class Play
    {
        static List<Direction> options = new List<Direction> { Direction.L, Direction.R, Direction.U, Direction.D };
        private List<Guess> solutions;
        private Guess nextGuess;
        private Dictionary<Direction, int> used;

        private int turns = 0;

        public Play()
        {
            Setup();

            var five_min = 0;
            var five_val = 0;

            foreach (var s in this.solutions!)
            {
                var turns = PlayMinValue(s);
                if (turns == 5)
                {
                    Console.WriteLine(this.nextGuess);
                    five_min++;
                }
                    

                //var turns2 = PlayValue(s);
                //if (turns2 == 5)
                //    five_val++;
            }

            Console.WriteLine($"Min {five_min},  AVG {five_val}");
        }

        private void Setup()
        {
            solutions = new List<Guess>();

            foreach (var i in options)
            {
                foreach (var j in options)
                {
                    foreach (var k in options)
                    {
                        foreach (var l in options)
                        {
                            solutions.Add(new Guess((Direction)i, (Direction)j, (Direction)k, (Direction)l));
                        }
                    }
                }
            }
            nextGuess = new Guess(Direction.L, Direction.L, Direction.R, Direction.R);
            used = new Dictionary<Direction, int>();
            foreach (var dir in options)
                used.Add(dir, 0);

            this.turns = 0;
        }

        public int PlayMinValue(Guess answer)
        {
            var sim = new Sim(answer);
            Setup();

            while (true)
            {   
                foreach (var dir in this.nextGuess.guess)
                    used[dir]++;
                var result = sim.TryGuess(this.nextGuess!);
                int correct = result.Item1;
                int almost = result.Item2;
                this.turns++;
                if (correct == 4)
                {
                    //Console.WriteLine(this.turns);
                    return this.turns;
                }

                
                solutions = solutions.Where(s => s.CouldProduce(this.nextGuess!, correct, almost)).ToList();

                var nextGuess = solutions[0];
                var nextValue = solutions[0].MinValue(solutions);

                foreach (var solution in solutions)
                {
                    var val = solution.MinValue(solutions);
                    if (val > nextValue)
                    {
                        nextValue = val;
                        nextGuess = solution;
                    }
                    else if (val == nextValue)
                    {
                        if (solution.UseScore(used) < nextGuess.UseScore(used))
                        {
                            nextGuess = solution;
                            nextValue = val;
                        }
                    }
                }

                this.nextGuess = nextGuess;
            }
        }

        public int PlayValue(Guess answer)
        {
            var sim = new Sim(answer);
            Setup();

            while (true)
            {
                foreach (var dir in this.nextGuess.guess)
                    used[dir]++;
                var result = sim.TryGuess(this.nextGuess!);
                int correct = result.Item1;
                int almost = result.Item2;
                this.turns++;
                if (correct == 4)
                {
                    //Console.WriteLine(this.turns);
                    return this.turns;
                }


                solutions = solutions.Where(s => s.CouldProduce(this.nextGuess!, correct, almost)).ToList();

                var nextGuess = solutions[0];
                var nextValue = solutions[0].Value(solutions);

                foreach (var solution in solutions)
                {
                    var val = solution.Value(solutions);
                    if (val > nextValue)
                    {
                        nextValue = val;
                        nextGuess = solution;
                    }
                    else if (val == nextValue)
                    {
                        if (solution.UseScore(used) < nextGuess.UseScore(used))
                        {
                            nextGuess = solution;
                            nextValue = val;
                        }
                    }
                }

                this.nextGuess = nextGuess;
            }
        }
    }
}
