using System;
using System.IO;

namespace BlobStorageDemo
{
    public class Blob
    {
        public Blob(Stream content, string reference)
        {
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
        }

        public Stream Content { get; }

        public string Reference { get; }

    }
}