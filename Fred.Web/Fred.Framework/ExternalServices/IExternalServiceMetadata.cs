namespace Fred.Framework.ExternalServices
{
    public interface IExternalServiceMetadata
    {
        string Url { get; }
        string TokenPath { get; }
    }
}
