namespace Codebreaker
{
    public class Sim
    {
        private readonly Guess answer;

        public Sim(Guess answer)
        {
            this.answer = answer;
        }

        public (int, int) TryGuess(Guess g)
        {
            var c = 0;
            var p = 0;
            var others = new List<Direction>();
            var check = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (g.guess[i] == answer.guess[i])
                {
                    c++;
                }
                else
                {
                    others.Add(g.guess[i]);
                    check.Add(i);
                }
            }
            foreach (var o in check)
            {
                if (others.Contains(answer.guess[o]))
                {
                    others.Remove(answer.guess[o]);
                    p++;
                }
            }

            return (c, p);
        }


    }
}
