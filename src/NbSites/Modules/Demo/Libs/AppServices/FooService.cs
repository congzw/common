using System;

namespace NbSites.Areas.Web.Demo.Libs.AppServices
{
    public interface IFooSingleton
    {
        string GetDesc();
    }
    public interface IFooScoped
    {
        string GetDesc();
    }
    public interface IFooTransient
    {
        string GetDesc();
    }
    
    public class FooService : IFooSingleton, IFooScoped,  IFooTransient
    {
        public Guid Id { get; set; }

        public FooService()
        {
            Id = Guid.NewGuid();
        }

        public string GetDesc()
        {
            return Id.ToString("N");
        }
    }
}
