using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Klimaci.WebUI.Infrastructure
{
    /// <summary>
    /// Belirtilen area'daki tüm controller'lara AuthorizeFilter(policy) ekler.
    /// </summary>
    public sealed class AdminAreaAuthorization : IApplicationModelConvention
    {
        private readonly string _area;
        private readonly string _policy;

        public AdminAreaAuthorization(string area, string policy)
        {
            _area = area ?? throw new ArgumentNullException(nameof(area));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var hasArea = controller.Attributes
                    .OfType<AreaAttribute>()
                    .Any(a => string.Equals(a.RouteValue, _area, StringComparison.OrdinalIgnoreCase));

                if (hasArea)
                {
                    controller.Filters.Add(new AuthorizeFilter(_policy));
                }
            }
        }
    }
}
