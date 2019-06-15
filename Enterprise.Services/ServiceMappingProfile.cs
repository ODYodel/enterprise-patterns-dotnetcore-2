using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Enterprise.DAL;
using Enterprise.DTO;
using Enterprise.DTO;
using Enterprise.Models;

namespace Enterprise.Services
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<Widgets, WidgetDTO>();
            CreateMap<WidgetDTO, Widgets>()
                .ForMember(src => src.Id, opts => opts.Ignore());
            CreateMap<WidgetResourcesDTO, WidgetResources>();
            CreateMap<WidgetResources, WidgetResourcesDTO>();
            CreateMap<Widgets, WidgetGridDTO>()
                .ForMember(e => e.WidgetTypes,
                        c => c.MapFrom(EnumerableExpressionHelper.CreateEnumToStringExpression((Widgets e) => e.WidgetTypes)))
                .ForMember(e => e.FeatureImagePath,
                        c => c.MapFrom(e => e.WidgetResources.FeatureIamagePath));
            CreateMap<IRepository<Widgets>, IRepository<WidgetDTO>>();
            CreateMap<IRepository<WidgetDTO>, IRepository<Widgets>>();
        }
    }
    public static class EnumerableExpressionHelper
    {
        public static Expression<Func<TSource, String>> CreateEnumToStringExpression<TSource, TMember>(
            Expression<Func<TSource, TMember>> memberAccess, string defaultValue = "")
        {
            var type = typeof(TMember);
            if (!type.IsEnum)
            {
                throw new InvalidOperationException("TMember must be an Enum type");
            }

            var enumNames = Enum.GetNames(type);
            var enumValues = (TMember[])Enum.GetValues(type);

            var inner = (Expression)Expression.Constant(defaultValue);

            var parameter = memberAccess.Parameters[0];

            for (int i = 0; i < enumValues.Length; i++)
            {
                inner = Expression.Condition(
                Expression.Equal(memberAccess.Body, Expression.Constant(enumValues[i])),
                Expression.Constant(enumNames[i]),
                inner);
            }

            var expression = Expression.Lambda<Func<TSource, String>>(inner, parameter);

            return expression;
        }
    }
}
