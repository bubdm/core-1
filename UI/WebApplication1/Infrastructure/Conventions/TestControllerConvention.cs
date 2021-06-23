using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApplication1.Infrastructure.Conventions
{
    public class TestControllerConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            //controller.Actions.Add(new ActionModel());
        }
    }
}
