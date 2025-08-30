using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Entity
{
    public class NewsletterSubscriber : BaseEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }

        public DateTime? ConfirmedAt { get; set; }
        public DateTime? UnsubscribedAt { get; set; }

        // Yeni alanlar
        public string? ConfirmToken { get; set; }
        public DateTime? ConfirmTokenExpiresAt { get; set; }
    }

}
