

bool exit = false;
var historial = new List<ResultadoPartida>();

(int a, int b, int correctAnswer, char simbolo) GenerarPregunta(Operacion operacion, Dificultad dificultad, Random random)
{

    if (operacion == Operacion.Random)
    {
        operacion = (Operacion)random.Next(0, 4);
    }
    var (min, max) = dificultad switch
    {
        Dificultad.Facil => (1, 10),
        Dificultad.Media => (1, 50),
        Dificultad.Dificil => (1, 100),
        _ => (1, 10)
    };

    switch (operacion)
    {
        case Operacion.Suma:
            {
                int a = random.Next(min, max + 1);
                int b = random.Next(min, max + 1);
                return (a, b, a + b, '+');
            }
        case Operacion.Resta:
            {
                int a = random.Next(min, max + 1);
                int b = random.Next(min, max + 1);
                if (b > a) (a, b) = (b, a);
                return (a, b, a - b, '-');
            }
        case Operacion.Multiplicacion:
            {
                int a = random.Next(min, max + 1);
                int b = random.Next(min, max + 1);
                return (a, b, a * b, '*');
            }
        case Operacion.Division:
            {
                int maxDivisor = dificultad switch
                {
                    Dificultad.Facil => 5,
                    Dificultad.Media => 10,
                    Dificultad.Dificil => 12,
                    _ => 5
                };
                int divisor = random.Next(1, maxDivisor + 1);
                int maxCociente = 100 / divisor;
                int cociente = random.Next(0, maxCociente + 1);
                int dividendo = divisor * cociente;
                return (dividendo, divisor, cociente, '/');
            }
        default:
            throw new ArgumentOutOfRangeException(nameof(operacion));
    }
}
Dificultad PreguntarDificultad()
{
    Console.Clear();
    Console.WriteLine("Elige la dificultad:");
    Console.WriteLine("1. Fácil");
    Console.WriteLine("2. Media");
    Console.WriteLine("3. Difícil");
    Console.Write("Opción: ");

    return Console.ReadLine() switch
    {
        "2" => Dificultad.Media,
        "3" => Dificultad.Dificil,
        _ => Dificultad.Facil
    };
}
void PlayGame(Operacion operacion, Dificultad dificultad, List<ResultadoPartida> historial)
{
    Console.Clear();
    Console.WriteLine(operacion);
    Console.WriteLine("Responde 5 preguntas.");

    var random = new Random();
    int score = 0;

    var cronometro = System.Diagnostics.Stopwatch.StartNew();

    for (int i = 0; i < 5; i++)
    {
        var pregunta = GenerarPregunta(operacion, dificultad, random);

        Console.Write($"{pregunta.a} {pregunta.simbolo} {pregunta.b} = ");
        string? answer = Console.ReadLine();

        if (int.TryParse(answer, out int userAnswer) && userAnswer == pregunta.correctAnswer)
        {
            Console.WriteLine("¡Correcto!");
            score++;
        }
        else
        {
            Console.WriteLine($"Incorrecto. La respuesta correcta es {pregunta.correctAnswer}.");
        }
    }
    cronometro.Stop();
    historial.Add(new ResultadoPartida(operacion, dificultad, score, 5, cronometro.Elapsed));
    Console.WriteLine($"Tu puntuación es: {score}/5");
    Console.WriteLine($"Tiempo: {cronometro.Elapsed:mm\\:ss}");
    Console.WriteLine("Presiona cualquier tecla para continuar...");
    Console.ReadKey();
}
void MostrarHistorial(List<ResultadoPartida> historial)
{
    Console.Clear();
    Console.WriteLine("Historial de partidas: ");

    if (historial.Count == 0)
    {
        Console.WriteLine("Todavía no has jugado ninguna partida.");
    }
    else
    {
        foreach (var partida in historial)
        {
            Console.WriteLine(partida);
        }
    }

    Console.WriteLine("Presiona cualquier tecla para volver al menú...");
    Console.ReadKey();
}
while (!exit)
{
    Console.Clear();
    Console.WriteLine("Bienvenido al juego de matemáticas");
    Console.WriteLine("1. Suma");
    Console.WriteLine("2. Resta");
    Console.WriteLine("3. Multiplicación");
    Console.WriteLine("4. División");
    Console.WriteLine("5. Ver historial");
    Console.WriteLine("6. Juego aleatorio");
    Console.WriteLine("0. Salir");
    Console.Write("Selecciona una opción: ");

    string? input = Console.ReadLine();

    switch (input)
    {
        case "1":
            PlayGame(Operacion.Suma,PreguntarDificultad(), historial);
            break;
        case "2":
            PlayGame(Operacion.Resta,PreguntarDificultad(), historial);
            break;
        case "3":
            PlayGame(Operacion.Multiplicacion,PreguntarDificultad(), historial);
            break;
        case "4":
            PlayGame(Operacion.Division,PreguntarDificultad(), historial);
            break;
        case "5":
            MostrarHistorial(historial);
            break;
        case "6":
            PlayGame(Operacion.Random,PreguntarDificultad(), historial);
            break;
        case "0":
            exit = true;
            break;
        default:
            Console.WriteLine("Opción no válida. Presiona cualquier tecla para continuar...");
            Console.ReadKey();
            break;
    }

}
enum Operacion
{
    Suma,
    Resta,
    Multiplicacion,
    Division,
    Random
}

enum Dificultad
{
    Facil,
    Media,
    Dificil
}
record ResultadoPartida(Operacion Operacion,Dificultad Dificultad, int Score, int Total, TimeSpan Duracion);