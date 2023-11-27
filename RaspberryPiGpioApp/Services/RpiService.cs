using Grpc.Net.Client;
using RaspberryPiGpioApp;

namespace RaspberryPiGpioApp.Services;
public class RpiService
{

    #region Fields
    string baseUrl;
    GrpcChannel channel;
    Commander.CommanderClient client;
    int port;
    #endregion

    #region Constructors
    public RpiService()
    {
        this.baseUrl = "192.168.7.242";
        this.port = 5065;
        this.channel = GrpcChannel.ForAddress($"http://{this.baseUrl}:{this.port}");
        this.client = new Commander.CommanderClient(channel);
    }
    #endregion

    #region Public methods
    public async Task<bool> ClosePin(int pinNumber)
    {
        var result = await this.client.ClosePinAsync(new ClosePinRequest { PinNumber = pinNumber });
        return result.IsPinClosed;
    }
    public void Connet()
    {
        this.channel = GrpcChannel.ForAddress($"http://{this.baseUrl}:{this.port}");
        this.client = new Commander.CommanderClient(channel);
    }
    public async Task<string> GetNumberingScheme()
    {
        var result = await this.client.GetNumberingSchemeAsync(new GetNumberingSchemeRequest());
        return result.NumberingScheme;
    }
    public async Task<int> GetPinCount()
    {
        var result = await this.client.GetPinCountAsync(new GetPinCountRequest());
        return result.PinCount;
    }
    public async Task<string> GetPinMode(int pinNumber)
    {
        var result = await this.client.GetPinModeAsync(new GetPinModeRequest { PinNumber = pinNumber });
        return result.PinMode;
    }
    public async Task<bool> OpenPin(int pinNumber)
    {
        var result = await this.client.OpenPinAsync(new OpenPinRequest { PinNumber = pinNumber });
        return result.IsPinOpen;
    }
    public async Task<string> ReadAsync(int pinNumber)
    {
        var result = await this.client.ReadAsync(new ReadRequest { PinNumber = pinNumber });
        return result.PinValue;
    }
    public void SetBaseAddress(string address) => this.baseUrl = address;
    public async Task<bool> SetPinMode(int pinNumber, string pinMode)
    {
        var result = await this.client.SetPinModeAsync(new SetPinModeRequest { PinNumber = pinNumber, PinMode = pinMode });
        return result.IsSet;
    }
    public void SetPort(int port) => this.port = port;
    public async Task<bool> WriteAsync(int pinNumber, string pinValue)
    {
        var result = await this.client.WriteAsync(new WriteRequest { PinNumber = pinNumber, PinValue = pinValue });
        return result.DidWrite;
    }
    #endregion

    #region Public properties
    public string BaseUrl => this.baseUrl;
    public int Port => this.port;
    #endregion

}
