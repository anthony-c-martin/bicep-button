using System.Diagnostics.CodeAnalysis;
using System.Collections.Concurrent;
using System.Text;
using Bicep.Core.Diagnostics;
using Bicep.Core.FileSystem;

public class UriFileResolver : IFileResolver
{
    private readonly ConcurrentDictionary<Uri, string> fileCache = new();

    private string Download(Uri fileUri)
        => fileCache.GetOrAdd(
            fileUri,
            fileUri => {
                var client = new HttpClient();
                
                return client.GetStringAsync(fileUri).Result;
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

    public bool TryRead(Uri fileUri, [NotNullWhen(true)] out string? fileContents, [NotNullWhen(false)] out DiagnosticBuilder.ErrorBuilderDelegate? failureBuilder)
    {
        fileContents = Download(fileUri);
        failureBuilder = null;
        return true;
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
}