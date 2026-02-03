using System.IO;
using DotNet.Testcontainers.Configurations;
using Xcepto.Interfaces;

namespace Xcepto.Testcontainers.Internal;

internal sealed class LoggingOutputConsumer : IOutputConsumer
{
    private readonly LineForwardingStream _stdout;
    private readonly LineForwardingStream _stderr;

    internal LoggingOutputConsumer(ILoggingProvider logger, string prefix = "", bool verbose = true)
    {
        _stderr = new LineForwardingStream(line =>
        {
            if (verbose)
                logger.LogDebug(Format(prefix, "stderr", line));
        });
        _stdout = new LineForwardingStream(line => logger.LogDebug(Format(prefix, "stdout", line)));
    }

    public bool Enabled => true;
    public Stream Stdout => _stdout;
    public Stream Stderr => _stderr;

    public void Dispose()
    {
        _stdout.Dispose();
        _stderr.Dispose();
    }

    private static string Format(string prefix, string stream, string line)
        => string.IsNullOrWhiteSpace(prefix)
            ? $"[container:{stream}] {line}"
            : $"[{prefix}:{stream}] {line}";
}
