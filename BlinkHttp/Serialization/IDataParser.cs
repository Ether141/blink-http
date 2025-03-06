namespace BlinkHttp.Serialization;

internal interface IDataParser
{
    RequestValue[] Parse(RequestContent content);
}
