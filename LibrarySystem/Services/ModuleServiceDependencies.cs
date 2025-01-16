using LibrarySystem.Services.Authentications.Implementations;
using LibrarySystem.Services.Authentications.Interfaces;
using LibrarySystem.Services.Implementations;
using LibrarySystem.Services.Interfaces;

namespace LibrarySystem.Services
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IBorrowRequestService, BorrowRequestService>();
            services.AddTransient<IUserService, UserService>();
            return services;
        }
    }
}
