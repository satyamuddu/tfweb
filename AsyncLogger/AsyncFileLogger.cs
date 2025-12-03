namespace AsyncLogger;

using System;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;


public enum LogLevel
{
    Debug = 1,
    Info = 2,
    Warning = 3,
    Error = 4,
    Critical = 5
}

public class AsyncFileLogger : IDisposable
{
    private readonly BlockingCollection<string> _logQueue = new();
    private readonly CancellationTokenSource _cts = new();
    private readonly Task _workerTask;

    private readonly string _logDirectory;
    private readonly long _maxFileSizeBytes;
    private string _currentLogPath;

    // Minimum level required to log a message
    public LogLevel MinimumLevel { get; set; } = LogLevel.Debug;

    public AsyncFileLogger(
        string logDirectory = "Logs",
        long maxFileSizeBytes = 2_000_000,
        LogLevel minimumLevel = LogLevel.Debug)
    {
        _logDirectory = logDirectory;
        _maxFileSizeBytes = maxFileSizeBytes;
        MinimumLevel = minimumLevel;

        Directory.CreateDirectory(_logDirectory);
        _currentLogPath = GetLogFilePath();

        _workerTask = Task.Run(LogWriterLoop);
    }

    private string GetLogFilePath()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        return Path.Combine(_logDirectory, $"log_{date}.txt");
    }

    public void Log(LogLevel level, string message)
    {
        // Filter out lower-severity logs
        if (level < MinimumLevel)
            return;

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        string formatted = $"{timestamp} [{level}] {message}";

        // Queue it for async logging
        _logQueue.Add(formatted);
    }

    private async Task LogWriterLoop()
    {
        foreach (var message in _logQueue.GetConsumingEnumerable(_cts.Token))
        {
            try
            {
                await RollFileIfNeededAsync();
                await File.AppendAllTextAsync(_currentLogPath, message + Environment.NewLine);
            }
            catch
            {
                // Keep logger stable, avoid throwing
            }
        }
    }

    private async Task RollFileIfNeededAsync()
    {
        var info = new FileInfo(_currentLogPath);

        if (!info.Exists)
            return;

        if (info.Length > _maxFileSizeBytes)
        {
            string rolledName = Path.Combine(
                _logDirectory,
                $"log_{DateTime.Now:yyyy-MM-dd_HHmmss}.txt");

            File.Move(_currentLogPath, rolledName);

            _currentLogPath = GetLogFilePath();
            await File.WriteAllTextAsync(_currentLogPath, "");
        }
    }

    public void Dispose()
    {
        _logQueue.CompleteAdding();
        _cts.Cancel();
        try { _workerTask.Wait(); } catch { }
    }
}
