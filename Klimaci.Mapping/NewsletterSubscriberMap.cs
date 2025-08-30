using Klimaci.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimaci.Mapping
{
    public class NewsletterSubscriberMap : BaseMap<NewsletterSubscriber>
    {
        public override void Configure(EntityTypeBuilder<NewsletterSubscriber> b)
        {
            base.Configure(b);
            b.ToTable("NewsletterSubscribers");

            b.Property(x => x.Email).HasMaxLength(200).IsRequired();
            b.Property(x => x.Name).HasMaxLength(150);

            b.Property(x => x.ConfirmedAt).HasColumnType("datetime2");
            b.Property(x => x.UnsubscribedAt).HasColumnType("datetime2");

            b.HasIndex(x => x.Email).IsUnique();
            b.HasIndex(x => x.CreatedDate);
        }
    }
}
