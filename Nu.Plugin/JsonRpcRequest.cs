using System.Text.Json;

namespace Nu.Plugin
{
    internal class JsonRpcRequest
    {
        readonly JsonDocument _jsonDoc;
        readonly bool _isValid = true;

        public JsonRpcRequest(string json)
        {
            if (string.IsNullOrEmpty(json?.Trim()))
            {
                _isValid = false;
            }
            else
            {
                _jsonDoc = JsonDocument.Parse(json);

                var rootElement = _jsonDoc.RootElement;

                if (!rootElement.TryGetProperty("jsonrpc", out var jsonRpcValue)
                    || jsonRpcValue.GetString() != "2.0")
                {
                    _isValid = false;
                }

                if (!rootElement.TryGetProperty("method", out var methodValue))
                {
                    _isValid = false;
                }

                Method = methodValue.GetString();
            }
        }

        public string Method { get; }

        public T GetParams<T>()
        {
            return JsonSerializer.Deserialize<T>(_jsonDoc.RootElement.GetProperty("params").GetRawText());
        }

        public bool IsValid => _isValid;
    }
}