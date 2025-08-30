using System;
using Mapster;

// Helpers
using Klimaci.Core.Extensions; // SlugHelper

// Entities
using Klimaci.DTO.BlogDTOs;
using Klimaci.DTO.ContactDTOs;
using Klimaci.DTO.FaqDTOs;
using Klimaci.DTO.MediaDTOs;
using Klimaci.DTO.PageDTOs;
using Klimaci.DTO.PartnerDTOs;
using Klimaci.DTO.ProjectDTOs;
using Klimaci.DTO.ServiceItemDTOs;
using Klimaci.DTO.SiteSettingDTOs;
using Klimaci.DTO.TestimonialDTOs;
using Klimaci.DTO;
using Klimaci.Entity;

namespace Klimaci.Services.MapsterMap
{
    public static class MapsterConfig
    {
        private static bool _inited;
        public static void Register()
        {
            if (_inited) return;

            // ============ MEDIA ============
            TypeAdapterConfig<MediaAddDTO, Media>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Ignore(d => d.AutoID)
                .Ignore(d => d.CreatedDate)
                .Ignore(d => d.ModifiedDate)
                .Ignore(d => d.Status);

            TypeAdapterConfig<MediaUpdateDTO, Media>
                .NewConfig()
                .IgnoreNullValues(true)
                .Ignore(d => d.Id)
                .Ignore(d => d.AutoID)
                .Ignore(d => d.CreatedDate)
                .Ignore(d => d.ModifiedDate)
                .Ignore(d => d.Status);

            TypeAdapterConfig<Media, MediaListItemDTO>
                .NewConfig();

            // ============ PAGE ============
            TypeAdapterConfig<PageAddDTO, Page>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID)
                .Ignore(d => d.CreatedDate)
                .Ignore(d => d.ModifiedDate)
                .Ignore(d => d.Status);

            TypeAdapterConfig<PageUpdateDTO, Page>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id)
                .Ignore(d => d.AutoID)
                .Ignore(d => d.CreatedDate)
                .Ignore(d => d.ModifiedDate)
                .Ignore(d => d.Status);

            TypeAdapterConfig<Page, PageListItemDTO>
                .NewConfig()
                .Map(d => d.CoverUrl, _ => (string)null);

            TypeAdapterConfig<Page, PageDetailDTO>
                .NewConfig()
                .Map(d => d.MediaIds, _ => new System.Collections.Generic.List<Guid>()) // join'den doldurulur
                .Map(d => d.CoverMediaId, _ => (Guid?)null);

            TypeAdapterConfig<Page, CommandResultDTO>
                .NewConfig()
                .Map(d => d.Id, s => s.Id)
                .Map(d => d.Slug, s => s.Slug);

            // ============ SERVICE ITEM ============
            TypeAdapterConfig<ServiceItemAddDTO, ServiceItem>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<ServiceItemUpdateDTO, ServiceItem>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<ServiceItem, ServiceItemListItemDTO>
                .NewConfig()
                .Map(d => d.CoverUrl, _ => (string)null);

            TypeAdapterConfig<ServiceItem, CommandResultDTO>
                .NewConfig().Map(d => d.Id, s => s.Id).Map(d => d.Slug, s => s.Slug);

            // ============ PARTNER ============
            TypeAdapterConfig<PartnerAddDTO, Partner>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<PartnerUpdateDTO, Partner>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<Partner, PartnerListItemDTO>
                .NewConfig()
                .Map(d => d.CoverUrl, _ => (string)null);

            TypeAdapterConfig<Partner, CommandResultDTO>
                .NewConfig().Map(d => d.Id, s => s.Id).Map(d => d.Slug, s => s.Slug);

            // ============ TESTIMONIAL ============
            TypeAdapterConfig<TestimonialAddDTO, Testimonial>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<TestimonialUpdateDTO, Testimonial>
                .NewConfig()
                .IgnoreNullValues(true)
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<Testimonial, TestimonialListItemDTO>
                .NewConfig();

            // ============ CONTACT MESSAGE ============
            TypeAdapterConfig<ContactMessageAddDTO, ContactMessage>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<ContactMessageUpdateDTO, ContactMessage>
                .NewConfig()
                .IgnoreNullValues(true)
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<ContactMessage, ContactMessageListItemDTO>
                .NewConfig();

            // ============ SITE SETTING ============
            TypeAdapterConfig<SiteSettingUpdateDTO, SiteSetting>
                .NewConfig()
                .IgnoreNullValues(true)
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            // ============ FAQ ============
            TypeAdapterConfig<FaqItemAddDTO, FaqItem>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<FaqItemUpdateDTO, FaqItem>
                .NewConfig()
                .IgnoreNullValues(true)
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<FaqItem, FaqItemListItemDTO>
                .NewConfig();

            // ============ BLOG ============
            TypeAdapterConfig<BlogPostAddDTO, BlogPost>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<BlogPostUpdateDTO, BlogPost>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<BlogPost, BlogPostListItemDTO>
                .NewConfig()
                .Map(d => d.CoverUrl, _ => (string)null);

            TypeAdapterConfig<CategoryAddDTO, Category>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<CategoryUpdateDTO, Category>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<TagAddDTO, Tag>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<TagUpdateDTO, Tag>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<BlogPost, CommandResultDTO>
                .NewConfig().Map(d => d.Id, s => s.Id).Map(d => d.Slug, s => s.Slug);
            TypeAdapterConfig<Category, CommandResultDTO>
                .NewConfig().Map(d => d.Id, s => s.Id).Map(d => d.Slug, s => s.Slug);
            TypeAdapterConfig<Tag, CommandResultDTO>
                .NewConfig().Map(d => d.Id, s => s.Id).Map(d => d.Slug, s => s.Slug);

            // ============ PROJECT / BRAND ============
            TypeAdapterConfig<ProjectAddDTO, Project>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<ProjectUpdateDTO, Project>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<Project, ProjectListItemDTO>
                .NewConfig()
                .Map(d => d.CoverUrl, _ => (string)null);

            TypeAdapterConfig<Project, CommandResultDTO>
                .NewConfig().Map(d => d.Id, s => s.Id).Map(d => d.Slug, s => s.Slug);

            TypeAdapterConfig<BrandAddDTO, Brand>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<BrandUpdateDTO, Brand>
                .NewConfig()
                .IgnoreNullValues(true)
                .Map(d => d.Slug, s => string.IsNullOrWhiteSpace(s.Slug) ? SlugHelper.Make(s.Title) : SlugHelper.Make(s.Slug))
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<Brand, CommandResultDTO>
                .NewConfig().Map(d => d.Id, s => s.Id).Map(d => d.Slug, s => s.Slug);

            // ============ APPOINTMENT ============
            TypeAdapterConfig<AppointmentRequestAddDTO, AppointmentRequest>
                .NewConfig()
                .Map(d => d.Id, _ => Guid.NewGuid())
                .Map(d => d.IsProcessed, _ => false)
                .Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<AppointmentRequestUpdateDTO, AppointmentRequest>
                .NewConfig()
                .IgnoreNullValues(true)
                .Ignore(d => d.Id).Ignore(d => d.AutoID).Ignore(d => d.CreatedDate).Ignore(d => d.ModifiedDate).Ignore(d => d.Status);

            TypeAdapterConfig<AppointmentRequest, AppointmentRequestListItemDTO>
                .NewConfig();

            // ============ COMMON SHORTCUTS ============

            _inited = true;
        }
    }
}
