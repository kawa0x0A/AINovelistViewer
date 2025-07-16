using Microsoft.JSInterop;

namespace AINovelistViewer
{
    public class JavascriptUtility : IAsyncDisposable
    {
        private static IJSObjectReference? clipboardObjectReference = null;
        private static IJSObjectReference? exportTextObjectReference = null;

        public static async Task<string> PasteFromClipboardAsync(IJSRuntime jSRuntime)
        {
            if (clipboardObjectReference is null)
            {
                clipboardObjectReference = await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/clipboard.js");
            }

            return await clipboardObjectReference.InvokeAsync<string>("pasteText");
        }

        public static async Task CopyToClipboardAsync(IJSRuntime jSRuntime, string text)
        {
            if (clipboardObjectReference is null)
            {
                clipboardObjectReference = await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/clipboard.js");
            }

            await clipboardObjectReference.InvokeVoidAsync("copyText", text);
        }

        public static async Task ExportTextAsync(IJSRuntime jSRuntime, string fileName, string text)
        {
            if (exportTextObjectReference is null)
            {
                exportTextObjectReference = await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/file.js");
            }

            using var stream = new MemoryStream();

            stream.Write(System.Text.Encoding.UTF8.GetBytes(text));

            stream.Seek(0, SeekOrigin.Begin);

            using var streamRef = new DotNetStreamReference(stream);

            await exportTextObjectReference.InvokeVoidAsync("downloadTextFile", fileName, streamRef);
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (clipboardObjectReference is not null)
            {
                try
                {
                    await clipboardObjectReference.DisposeAsync();
                }
                catch (JSDisconnectedException)
                {
                }
            }

            if (exportTextObjectReference is not null)
            {
                try
                {
                    await exportTextObjectReference.DisposeAsync();
                }
                catch (JSDisconnectedException)
                {
                }
            }
        }
    }
}
