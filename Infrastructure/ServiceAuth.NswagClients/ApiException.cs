
/// <summary>
/// Исключение, которое выкидывает SwaggerGen при получении ответа с кодом >= 300
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("NSwag", "13.0.5.0 (NJsonSchema v10.0.22.0 (Newtonsoft.Json v11.0.0.0))")]
public partial class ApiException : Exception
{
    /// <summary>
    /// Статус код ответа сервиса
    /// </summary>
    public int StatusCode { get; private set; }

    /// <summary>
    /// Ответ в виде строки
    /// </summary>
    public string Response { get; private set; }

    /// <summary>
    /// Набор заголовков ответа
    /// </summary>
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="statusCode">Код ответа</param>
    /// <param name="response">Ответ в формате строки</param>
    /// <param name="headers">Набор заголовков ответа</param>
    /// <param name="innerException">Внутреннее исключение</param>
    public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
    : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + response.Substring(0, response.Length >= 512 ? 512 : response.Length), innerException)
    {
        StatusCode = statusCode;
        Response = response;
        Headers = headers;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
    }
}

/// <summary>
/// Исключение, которое выкидывает SwaggerGen при получении ответа с кодом >= 300, если был указан тип возвращаемых данных для этого кода
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("NSwag", "13.0.5.0 (NJsonSchema v10.0.22.0 (Newtonsoft.Json v11.0.0.0))")]
public partial class ApiException<TResult> : ApiException
{
    /// <summary>
    /// Результат вызова метода (данные)
    /// </summary>
    public TResult Result { get; private set; }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <param name="statusCode">Код ответа</param>
    /// <param name="response">Ответ в формате строки</param>
    /// <param name="headers">Набор заголовков ответа</param>
    /// <param name="result">Результат вызова метода (данные)</param>
    /// <param name="innerException">Внутреннее исключение</param>
    public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result, Exception innerException)
    : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }
}