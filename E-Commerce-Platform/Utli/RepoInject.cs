using Core.Helper;
using Core.Helper.OrderState;
using Core.IRepos;
using Infrastructure.Repos;


namespace E_Commerce_Platform.Utli
{
    public static class RepoInject
    {
            public static IServiceCollection InjectApplicationRepos(this IServiceCollection services)
            {

              // inject all repos here
               services.AddTransient<IOrderRepo, OrderRepo>();
             

                return services;
            }
        }
    }


