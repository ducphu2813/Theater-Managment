using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace PaymentService.Helper;

public class UrlHelperFactory
{
    private readonly IActionContextAccessor _actionContextAccessor;
    
    public UrlHelperFactory(IActionContextAccessor actionContextAccessor)
    {
        _actionContextAccessor = actionContextAccessor;
    }
    
    public IUrlHelper GetUrlHelper()
    {
        var actionContext = _actionContextAccessor.ActionContext;
        return new UrlHelper(actionContext);
    }
}