using Newtonsoft.Json.Linq;
public interface ISave
{
    public JObject Serialize();
    public void Deserialize(string jsonString);
}
