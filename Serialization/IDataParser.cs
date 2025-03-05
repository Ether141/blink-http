namespace BlinkHttp.Serialization;

internal interface IDataParser
{
    void Parse(RequestContent content);
}
