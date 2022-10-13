using System.Collections.Generic;
using System.Threading.Tasks;


public interface IIAPGameService : IService
{
    Task Initialize(Dictionary<string, string> products);
    bool IsReady();
    Task<bool> StartPurchase(string product);

    string GetLocalizedPrice(string product);
}

