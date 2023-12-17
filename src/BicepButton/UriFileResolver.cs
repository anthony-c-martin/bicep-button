using System.Diagnostics.CodeAnalysis;
using System.Collections.Concurrent;
using System.Text;
using Bicep.Core.Diagnostics;
using Bicep.Core.FileSystem;

public class UriFileResolver : IFileResolver
{
    private readonly ConcurrentDictionary<Uri, byte[]> fileCache = new();

    private byte[] Download(Uri fileUri)
        => fileCache.GetOrAdd(
            fileUri,
            fileUri => {
                var client = new HttpClient();
                
                return client.GetByteArrayAsync(fileUri).Result;
            });

    public bool DirExists(Uri fileUri)
    {
        throw new NotImplementedException();
    }

    public bool FileExists(Uri uri)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Uri> GetDirectories(Uri fileUri, string pattern = "")
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Uri> GetFiles(Uri fileUri, string pattern = "")
    {
        throw new NotImplementedException();
    }

    public string GetRelativePath(string relativeTo, string path)
    {
        return Path.GetRelativePath(relativeTo, path).Replace('\\', '/');
    }

    public IDisposable? TryAcquireFileLock(Uri fileUri)
    {
        throw new NotImplementedException();
    }

    public bool TryRead(Uri fileUri, [NotNullWhen(true)] out string? fileContents, [NotNullWhen(false)] out DiagnosticBuilder.ErrorBuilderDelegate? failureBuilder, Encoding fileEncoding, int maxCharacters, [NotNullWhen(true)] out Encoding? detectedEncoding)
    {
        throw new NotImplementedException();
    }

    public bool TryReadAsBase64(Uri fileUri, [NotNullWhen(true)] out string? fileBase64, [NotNullWhen(false)] out DiagnosticBuilder.ErrorBuilderDelegate? failureBuilder, int maxCharacters = -1)
    {
        throw new NotImplementedException();
    }

    public bool TryReadAtMostNCharacters(Uri fileUri, Encoding fileEncoding, int n, [NotNullWhen(true)] out string? fileContents)
    {
        throw new NotImplementedException();
    }

    public Uri? TryResolveFilePath(Uri parentFileUri, string childFilePath)
    {
        if (!Uri.TryCreate(parentFileUri, childFilePath, out var relativeUri))
        {
            return null;
        }

        return relativeUri;
    }

    public void Write(Uri fileUri, Stream contents)
    {
        throw new NotImplementedException();
    }

    public ResultWithDiagnostic<string> TryRead(Uri fileUri)
        => TryRead(fileUri, Encoding.UTF8, -1).IsSuccess(out var result, out var errorBuilder)
            ? new(result.Contents) : new(errorBuilder!);

    public ResultWithDiagnostic<FileWithEncoding> TryRead(Uri fileUri, Encoding fileEncoding, int maxCharacters)
    {
        try
        {
            var bytes = Download(fileUri);

            var contents = fileEncoding.GetString(bytes) ?? throw new InvalidOperationException("Could not decode file contents");
            return new(new FileWithEncoding(contents, fileEncoding));
        }
        catch (Exception exception)
        {
            return new(x => x.ErrorOccurredReadingFile(exception.Message));
        }
    }

    public ResultWithDiagnostic<string> TryReadAtMostNCharacters(Uri fileUri, Encoding fileEncoding, int n)
    {
        throw new NotImplementedException();
    }

    public ResultWithDiagnostic<string> TryReadAsBase64(Uri fileUri, int maxCharacters = -1)
    {
        throw new NotImplementedException();
    }
}