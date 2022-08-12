
public class Guess
{
    public readonly Direction[] guess;
    public static (int, int)[] cs = new[] {(0, 0), (1, 0), (0, 1), (2, 0), (0, 2), (1, 1), (3, 0), (0, 3), (2, 1), (1, 2), (4, 0), (0, 4), (3, 1), (1, 3), (2, 2)};
    public Guess(Direction a, Direction b, Direction c, Direction d)
    {
        guess = new Direction[]{a, b, c, d};
    }

    public bool CouldProduce(Guess g, int correct, int present)
    {
        var c = 0;
        var p = 0;
        var others = new List<Direction>();
        var check = new List<int>();
        for(int i = 0; i < 4; i++)
        {
            if (g.guess[i] == guess[i])
            {
                c++;
            } else {
                others.Add(g.guess[i]);
                check.Add(i);
            }
        }
        foreach (var o in check)
        {
            if (others.Contains(guess[o]))
            {
                others.Remove(guess[o]);
                p++;
            }
        }
        return correct == c && present == p;
    }

    public float Value(List<Guess> solutions)
    {
        var sum = 0f;
        var div = 0;
        foreach(var a in cs)
        {
            var elim = solutions.Where(s => !s.CouldProduce(this, a.Item1, a.Item2)).Count();
            if (elim == solutions.Count)
                continue;

            sum += elim;
            div++;
        }
        return sum / div;
    }

    public int UseScore(Dictionary<Direction, int> uses)
    {
        var score = 0;

        foreach(var d in guess)
        {
            score += uses[d];
        }

        return score;
    }

    public int MinValue(List<Guess> solutions)
    {
        var min = int.MaxValue;
        foreach (var a in cs)
        {
            var val = solutions.Where(s => !s.CouldProduce(this, a.Item1, a.Item2)).Count();
            if (val < min)
                min = val;
        }
        return min;
    }

    public override string ToString()
    {
        return $"{guess[0]}{guess[1]}{guess[2]}{guess[3]}";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != this.GetType())
            return false;

        for (int i = 0; i < 4; i++)
        {
            if ((obj as Guess)!.guess[i] != this.guess[i])
                return false;
        }

        return true;
    }
}


