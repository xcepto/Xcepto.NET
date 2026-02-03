using System;
using DotNet.Testcontainers.Configurations;
using Microsoft.Extensions.Logging;

namespace Xcepto.Testcontainers.Interfaces;

public interface ITestContainerSupport: IDisposable
{
    public IOutputConsumer CreateOutputConsumer(string prefix, bool verbose = true);
    public ILogger CreateLogger(string prefix, bool verbose = true);
}