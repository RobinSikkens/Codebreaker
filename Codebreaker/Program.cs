using Codebreaker;

// var p = new Play();

var wrong = new List<Guess>
{
    new Guess(Direction.L, Direction.L, Direction.D, Direction.L),
    new Guess(Direction.L, Direction.D, Direction.D, Direction.L),
    new Guess(Direction.R, Direction.L, Direction.U, Direction.D),
    new Guess(Direction.R, Direction.R, Direction.R, Direction.U),
    new Guess(Direction.R, Direction.D, Direction.R, Direction.R),
    new Guess(Direction.U, Direction.L, Direction.R, Direction.R),
    new Guess(Direction.D, Direction.L, Direction.L, Direction.L),
    new Guess(Direction.D, Direction.R, Direction.R, Direction.U),
    new Guess(Direction.D, Direction.R, Direction.U, Direction.R),
    new Guess(Direction.D, Direction.U, Direction.U, Direction.U),
    new Guess(Direction.D, Direction.U, Direction.U, Direction.D),
    new Guess(Direction.D, Direction.D, Direction.L, Direction.R),
    new Guess(Direction.D, Direction.D, Direction.R, Direction.L),
    new Guess(Direction.D, Direction.D, Direction.U, Direction.U),
    new Guess(Direction.D, Direction.D, Direction.U, Direction.D),
};

Console.WindowWidth = 40;
Console.BufferWidth = 40;

while (true)
{
    var solutions = new List<Guess>();
    var options = new List<Direction> { Direction.L, Direction.R, Direction.U, Direction.D };

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
    var guess = new Guess(Direction.L, Direction.L, Direction.R, Direction.R);
    var used = new Dictionary<Direction, int>();
    foreach (var dir in options)
        used.Add(dir, 0);

    foreach (var d in guess.guess)
        used[d]++;

    Console.ForegroundColor = ConsoleColor.DarkGray;
    var wCount = solutions.Where(s => wrong.Contains(s)).Count();
    Console.WriteLine($"[{wCount}/{solutions.Count}] {(float)wCount / (float)solutions.Count * 100}%");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"{guess}  ");


    while (true)
    {

        var correct = Console.ReadKey(true).KeyChar.ToString();
        Console.ForegroundColor = ConsoleColor.Green;
        string almost = null;
        if (string.IsNullOrEmpty(correct) || correct.StartsWith('q'))
            return;
        if (correct.StartsWith("\r") || correct.StartsWith("4"))
        {
            correct = "4";
            almost = "0";
            Console.Write($"{correct} ");
        }
        else
        {
            Console.Write($"{correct} ");
            almost = Console.ReadKey(true).KeyChar.ToString();
            if (almost.StartsWith("\r"))
                almost = "0";
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{almost}\n");
        if (string.IsNullOrEmpty(almost) || almost.StartsWith('q'))
            return;

        Console.ForegroundColor = ConsoleColor.White;
        solutions = solutions.Where(s => s.CouldProduce(guess!, int.Parse(correct), int.Parse(almost ?? "0"))).ToList();

        if (solutions.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geen oplossingen mogelijk... Check input.\n");
            break;
        }

        var nextGuess = solutions[0];
        var nextValue = solutions[0].MinValue(solutions);

        foreach(var solution in solutions)
        {
            var val = solution.MinValue(solutions);
            if(val > nextValue)
            {
                nextValue = val;
                nextGuess = solution;
            } else if (val == nextValue)
            {
                if (solution.UseScore(used) < nextGuess.UseScore(used))
                {
                    nextGuess = solution;
                    nextValue = val;
                }
            }
        }


        Console.ForegroundColor = ConsoleColor.DarkGray;
        //Console.WriteLine($"MinValue: {nextGuess.MinValue(solutions)}  AVG: {nextGuess.Value(solutions)}");
        var wrongCount = solutions.Where(s => wrong.Contains(s)).Count();
        Console.WriteLine($"[{wrongCount}/{solutions.Count}] {(float)wrongCount / (float)solutions.Count * 100}%");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"{nextGuess}  ");
        if (solutions.Count == 2)
            Console.Write($"[{string.Join(' ',solutions.Where(s => !s.Equals(nextGuess)))}] ");

        foreach (var dir in nextGuess.guess)
            used[dir]++;

        if (solutions.Count <= 1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!\n");
            Console.ForegroundColor = ConsoleColor.White;
            break;
        }
        guess = nextGuess;
    }
}


