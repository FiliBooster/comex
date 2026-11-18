using System.Diagnostics;

namespace Comex;

internal static class EntradaConsole
{
    public static string? ReadLine()
    {
        AcompanharCursor();
        return Console.ReadLine();
    }

    public static ConsoleKeyInfo ReadKey(bool intercept = false)
    {
        AcompanharCursor();
        return Console.ReadKey(intercept);
    }

    private static void AcompanharCursor()
    {
        if (!OperatingSystem.IsWindows() || Console.IsOutputRedirected || Console.IsInputRedirected)
        {
            return;
        }

        try
        {
            // Reaplicar a posição pede ao host do console que mostre o cursor sem alterar o histórico.
            var (coluna, linha) = Console.GetCursorPosition();
            Console.SetCursorPosition(coluna, linha);
        }
        catch (IOException ex)
        {
            Trace.TraceWarning("Não foi possível acompanhar o cursor do console: {0}", ex.Message);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Trace.TraceWarning("A posição do cursor mudou antes da leitura: {0}", ex.Message);
        }
    }
}
