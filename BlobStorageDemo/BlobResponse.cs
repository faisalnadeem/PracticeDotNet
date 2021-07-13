using System;

namespace BlobStorageDemo
{
    public class BlobResponse
    {
        public BlobResponse(Uri uri, string saSToken, DateTimeOffset tokenExpiry)
        {
            Uri = uri;
            SaSToken = saSToken;
            TokenExpires = tokenExpiry;
        }
        public Uri Uri { get; }

        public string SaSToken { get; }

        public DateTimeOffset TokenExpires { get; }

        public override string ToString()
        {
            return string.Concat(Uri, SaSToken);
        }
    }
}