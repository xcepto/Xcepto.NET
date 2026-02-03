using System;
using System.IO;
using System.Text;

namespace Xcepto.Testcontainers.Internal;

internal sealed class LineForwardingStream : Stream
{
    private readonly Action<string> _sink;
    private readonly object _gate = new();
    private readonly Decoder _decoder = Encoding.UTF8.GetDecoder();
    private readonly StringBuilder _buffer = new();

    public LineForwardingStream(Action<string> sink) => _sink = sink;

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (count <= 0) return;

        char[] chars = new char[Encoding.UTF8.GetMaxCharCount(count)];
        int charCount;
        lock (_gate)
        {
            charCount = _decoder.GetChars(buffer, offset, count, chars, 0, flush: false);
            AppendAndFlushLines(chars, charCount);
        }
    }

    private void AppendAndFlushLines(char[] chars, int charCount)
    {
        for (int i = 0; i < charCount; i++)
        {
            char c = chars[i];
            if (c == '\n')
            {
                var line = _buffer.ToString().TrimEnd('\r');
                _buffer.Clear();
                if (line.Length > 0) _sink(line);
            }
            else
            {
                _buffer.Append(c);
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            lock (_gate)
            {
                var tail = _buffer.ToString().TrimEnd('\r', '\n');
                _buffer.Clear();
                if (tail.Length > 0) _sink(tail);
            }
        }
        base.Dispose(disposing);
    }

    // Stream boilerplate
    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override long Length => throw new NotSupportedException();
    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
    public override void Flush() { }
    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
}